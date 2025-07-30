# Professional Migration Management System
# Cross-platform script for Windows and Linux
# Handles any migration inconsistencies automatically

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("status", "fix-inconsistencies", "clean-orphans", "force-rebuild", "add-migration", "schema-check")]
    [string]$Action,
    
    [Parameter(Mandatory=$false)]
    [string]$MigrationName,
    
    [switch]$Force,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

# Modern OS detection using PowerShell's built-in platform detection
# This is more reliable than environment variables and works across all PowerShell versions
$ScriptIsWindows = $PSVersionTable.PSPlatform -eq 'Win32NT'
$ScriptIsLinux = $PSVersionTable.PSPlatform -eq 'Unix'
$ScriptIsMacOS = $PSVersionTable.PSPlatform -eq 'Unix' -and $env:OSTYPE -like "*darwin*"

# Cross-platform path separator
$PathSeparator = if ($ScriptIsWindows) { "\" } else { "/" }

# Cross-platform file operations
function Remove-ItemCrossPlatform {
    param([string]$Path)
    if ($ScriptIsWindows) {
        Remove-Item $Path -Force -ErrorAction SilentlyContinue
    } else {
        rm -f $Path 2>$null
    }
}

function Test-PathCrossPlatform {
    param([string]$Path)
    if ($ScriptIsWindows) {
        Test-Path $Path
    } else {
        Test-Path $Path
    }
}

function Get-ChildItemCrossPlatform {
    param([string]$Path, [string]$Filter)
    if ($ScriptIsWindows) {
        Get-ChildItem $Path -Filter $Filter
    } else {
        Get-ChildItem $Path -Filter $Filter
    }
}

function New-ItemCrossPlatform {
    param([string]$Path, [string]$ItemType)
    if ($ScriptIsWindows) {
        New-Item -ItemType Directory -Path $Path -Force | Out-Null
    } else {
        mkdir -p $Path 2>$null
    }
}

function Copy-ItemCrossPlatform {
    param([string]$Source, [string]$Destination)
    if ($ScriptIsWindows) {
        Copy-Item $Source $Destination -Force
    } else {
        cp -r $Source $Destination 2>$null
    }
}

function Join-PathCrossPlatform {
    param([string]$Path1, [string]$Path2)
    if ($ScriptIsWindows) {
        Join-Path $Path1 $Path2
    } else {
        "$Path1$PathSeparator$Path2" -replace "\\+", "/"
    }
}

function Test-ContainersRunning {
    Write-Host "Checking container availability..." -ForegroundColor Yellow
    
    $requiredContainers = @("ugh-db", "ugh-backend")
    $missingContainers = @()
    
    foreach ($container in $requiredContainers) {
        $status = docker ps --filter "name=$container" --format "{{.Status}}" 2>&1
        if ($LASTEXITCODE -ne 0 -or -not $status) {
            $missingContainers += $container
        } else {
            Write-Host "  ✓ $container is running" -ForegroundColor Green
        }
    }
    
    if ($missingContainers.Count -gt 0) {
        Write-Host "[ERROR] Missing containers: $($missingContainers -join ', ')" -ForegroundColor Red
        Write-Host "Please start the application with: docker compose up -d" -ForegroundColor Yellow
        return $false
    }
    
    return $true
}

function Get-AppliedMigrations {
    $result = docker exec ugh-db mysql -uuser -ppassword db --skip-column-names -e "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId"
    return $result | Where-Object { $_ -and $_ -match '^\d{14}_' }
}

function Get-CodeMigrations {
    # First check if EF tools are available in container
    $efCheck = docker exec ugh-backend dotnet ef --version 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[WARNING] EF tools not available in container. Installing..." -ForegroundColor Yellow
        # Try to install EF tools in container
        docker exec ugh-backend dotnet tool install --global dotnet-ef 2>&1 | Out-Null
        if ($LASTEXITCODE -ne 0) {
            Write-Host "[WARNING] Failed to install EF tools in container. Trying alternative approach..." -ForegroundColor Yellow
            # Alternative: Try to restore tools first
            docker exec ugh-backend dotnet tool restore 2>&1 | Out-Null
            if ($LASTEXITCODE -ne 0) {
                throw "Failed to install/restore EF tools in container. Please ensure dotnet-ef is available."
            }
        }
    }
    
    $result = docker exec ugh-backend dotnet ef migrations list 2>&1
    if ($LASTEXITCODE -ne 0) { 
        Write-Host "[ERROR] EF command failed: $result" -ForegroundColor Red
        Write-Host "[INFO] This might be due to missing EF tools in the container." -ForegroundColor Yellow
        Write-Host "[INFO] Try running: docker exec ugh-backend dotnet tool install --global dotnet-ef" -ForegroundColor Gray
        return @()
    }
    return ($result -split "`n" | Where-Object { $_ -match '^\d{14}_' } | ForEach-Object { $_.Trim() })
}

function Get-FilesystemMigrations {
    $migrationPath = Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations"
    $files = Get-ChildItemCrossPlatform $migrationPath "*.cs" | Where-Object {
        $_.Name -match '^\d{14}_.*\.cs$' -and $_.Name -notlike "*ModelSnapshot*" -and $_.Name -notlike "*.Designer.cs"
    }
    return $files | ForEach-Object { $_.Name -replace '\.cs$', '' }
}

function Show-Status {
    Write-Host "=== MIGRATION SYSTEM STATUS ===" -ForegroundColor Cyan
    Write-Host ""
    
    $dbMigrations = Get-AppliedMigrations
    $codeMigrations = Get-CodeMigrations
    $fileMigrations = Get-FilesystemMigrations
    
    $allMigrations = @($dbMigrations + $codeMigrations + $fileMigrations) | Sort-Object | Get-Unique
    $inconsistencies = @()
    
    Write-Host "Migration Analysis:" -ForegroundColor Yellow
    foreach ($migration in $allMigrations) {
        $inDB = $dbMigrations -contains $migration
        $inCode = $codeMigrations -contains $migration
        $inFiles = $fileMigrations -contains $migration
        
        $status = "OK"
        $color = "Green"
        
        if ($inDB -and $inCode -and $inFiles) {
            $status = "CONSISTENT"
        } elseif ($inDB -and -not $inCode) {
            $status = "MISSING IN CODE"
            $color = "Red"
            $inconsistencies += [PSCustomObject]@{
                Migration = $migration
                Issue = "AppliedButNotInCode"
                Action = "Remove from database or recreate code files"
            }
        } elseif ($inCode -and -not $inDB) {
            $status = "NOT APPLIED"
            $color = "Yellow"
            $inconsistencies += [PSCustomObject]@{
                Migration = $migration
                Issue = "CodeButNotApplied"
                Action = "Apply to database or remove from code"
            }
        } elseif ($inFiles -and -not $inCode) {
            $status = "ORPHANED FILE"
            $color = "Red"
            $inconsistencies += [PSCustomObject]@{
                Migration = $migration
                Issue = "OrphanedFile"
                Action = "Remove files or regenerate EF metadata"
            }
        }
        
        Write-Host "  [$status] $migration" -ForegroundColor $color
    }
    
    Write-Host ""
    if ($inconsistencies.Count -eq 0) {
        Write-Host "[SYSTEM HEALTHY] No migration inconsistencies found" -ForegroundColor Green
    } else {
        Write-Host "[INCONSISTENCIES FOUND] $($inconsistencies.Count) issues detected:" -ForegroundColor Red
        foreach ($issue in $inconsistencies) {
            Write-Host "  • $($issue.Migration): $($issue.Issue)" -ForegroundColor Red
        }
        Write-Host ""
        Write-Host "SOLUTION: Run with -Action fix-inconsistencies" -ForegroundColor Yellow
    }
    
    return $inconsistencies
}

function Fix-Inconsistencies {
    Write-Host "=== FIXING MIGRATION INCONSISTENCIES ===" -ForegroundColor Cyan
function Schema-Check {
    Write-Host "=== SCHEMA CHECK (Model vs. DB) ===" -ForegroundColor Cyan
    Write-Host "Checking for missing columns or tables in the database..." -ForegroundColor Yellow

    # Use dotnet ef migrations script to generate SQL for the current model
    $scriptResult = docker exec ugh-backend dotnet ef migrations script --idempotent 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "[ERROR] Could not generate migration script: $scriptResult" -ForegroundColor Red
        return
    }

    # Heuristic: If the script contains 'ALTER TABLE' or 'ADD COLUMN', there are pending model changes
    $hasAlter = $scriptResult -match 'ALTER TABLE' -or $scriptResult -match 'ADD COLUMN'
    if ($hasAlter) {
        Write-Host "[WARNING] Detected model changes not present in the database!" -ForegroundColor Red
        Write-Host "You may have missing columns or tables (e.g., AddressId)." -ForegroundColor Red
        Write-Host "\nNext steps:" -ForegroundColor Yellow
        Write-Host "  1. Remove the model snapshot (UghContextModelSnapshot.cs)" -ForegroundColor Gray
        Write-Host "  2. Re-add a migration (e.g., migration.ps1 -Action add-migration -MigrationName 'FixSchema')" -ForegroundColor Gray
        Write-Host "  3. Apply the migration to update the DB schema" -ForegroundColor Gray
        if (-not $Force -and -not $DryRun) {
            $confirm = Read-Host "Do you want to auto-remove the snapshot and create a new migration now? (y/N)"
            if ($confirm -ne 'y' -and $confirm -ne 'Y') {
                Write-Host "Cancelled" -ForegroundColor Gray
                return
            }
        }
        # Remove snapshot
        $snapshotPath = Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations$PathSeparator UghContextModelSnapshot.cs"
        if (Test-PathCrossPlatform $snapshotPath) {
            Remove-ItemCrossPlatform $snapshotPath
            Write-Host "[OK] Removed model snapshot: $snapshotPath" -ForegroundColor Green
        } else {
            Write-Host "[INFO] Model snapshot already removed or missing." -ForegroundColor Yellow
        }
        # Add migration
        $migrationName = "FixSchema"
        Write-Host "Creating migration: $migrationName..." -ForegroundColor Yellow
        $addResult = docker exec ugh-backend dotnet ef migrations add $migrationName 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "[OK] Migration created: $migrationName" -ForegroundColor Green
            Write-Host "Apply with: docker exec ugh-backend dotnet ef database update" -ForegroundColor Gray
        } else {
            Write-Host "[ERROR] Migration creation failed: $addResult" -ForegroundColor Red
        }
    } else {
        Write-Host "[OK] No schema differences detected. DB matches model." -ForegroundColor Green
    }
}
    Write-Host ""
    
switch ($Action) {
    "status" { Show-Status }
    "fix-inconsistencies" { Fix-Inconsistencies }
    "clean-orphans" { Clean-Orphans }
    "force-rebuild" { Force-Rebuild }
    "add-migration" { Add-Migration }
    "schema-check" { Schema-Check }
    default { Write-Host "Unknown action: $Action" -ForegroundColor Red }
}
    foreach ($issue in $issues) {
        Write-Host "  • $($issue.Migration): $($issue.Action)" -ForegroundColor Yellow
    }
    
    Write-Host ""
    if (-not $Force -and -not $DryRun) {
        $confirm = Read-Host "Apply these fixes? (y/N)"
        if ($confirm -ne 'y' -and $confirm -ne 'Y') {
            Write-Host "Cancelled" -ForegroundColor Gray
            return
        }
    }
    
    foreach ($issue in $issues) {
        Write-Host ""
        Write-Host "Fixing: $($issue.Migration)" -ForegroundColor Cyan
        
        switch ($issue.Issue) {
            "AppliedButNotInCode" {
                Write-Host "  Removing from database (code takes precedence)..." -ForegroundColor Yellow
                if (-not $DryRun) {
                    $query = "DELETE FROM __EFMigrationsHistory WHERE MigrationId = '$($issue.Migration)'"
                    docker exec ugh-db mysql -uuser -ppassword db -e $query
                    if ($LASTEXITCODE -eq 0) {
                        Write-Host "  [OK] Removed from database" -ForegroundColor Green
                    } else {
                        Write-Host "  [ERROR] Failed to remove from database" -ForegroundColor Red
                    }
                } else {
                    Write-Host "  [DRY-RUN] Would remove from database" -ForegroundColor Gray
                }
            }
            
            "OrphanedFile" {
                Write-Host "  Removing orphaned files..." -ForegroundColor Yellow
                if (-not $DryRun) {
                    $files = @(
                        (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$($issue.Migration).cs"),
                        (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$($issue.Migration).Designer.cs")
                    )
                    foreach ($file in $files) {
                        if (Test-PathCrossPlatform $file) {
                            Remove-ItemCrossPlatform $file
                            Write-Host "  [OK] Removed $file" -ForegroundColor Green
                        }
                    }
                } else {
                    Write-Host "  [DRY-RUN] Would remove migration files" -ForegroundColor Gray
                }
            }
            
            "CodeButNotApplied" {
                Write-Host "  Migration exists in code but not applied to database" -ForegroundColor Yellow
                
                # For test migrations or migrations with specific patterns, auto-remove
                if ($issue.Migration -match "(Test|AddLastLogin|Documentation)" -or 
                    $issue.Migration -like "*Test*") {
                    Write-Host "  Auto-removing test migration..." -ForegroundColor Yellow
                    if (-not $DryRun) {
                        $files = @(
                            (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$($issue.Migration).cs"),
                            (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$($issue.Migration).Designer.cs")
                        )
                        foreach ($file in $files) {
                            if (Test-PathCrossPlatform $file) {
                                Remove-ItemCrossPlatform $file
                                Write-Host "  [OK] Removed $file" -ForegroundColor Green
                            }
                        }
                        
                        # Also rebuild the snapshot to clean EF cache
                        Write-Host "  Rebuilding model snapshot..." -ForegroundColor Yellow
                        $result = docker exec ugh-backend dotnet ef migrations remove --force 2>&1
                        if ($LASTEXITCODE -eq 0) {
                            Write-Host "  [OK] Model snapshot updated" -ForegroundColor Green
                        }
                    } else {
                        Write-Host "  [DRY-RUN] Would remove test migration files and update snapshot" -ForegroundColor Gray
                    }
                } else {
                    Write-Host "  Use 'dotnet ef database update' to apply or remove the migration manually" -ForegroundColor Yellow
                }
            }
        }
    }
    
    Write-Host ""
    Write-Host "[REPAIR COMPLETE] Run status again to verify" -ForegroundColor Green
    
    # Update documentation after repairs
    Update-MigrationDocumentation
}

function Clean-Orphans {
    Write-Host "=== CLEANING ALL ORPHANED FILES ===" -ForegroundColor Cyan
    
    $fileMigrations = Get-FilesystemMigrations
    $codeMigrations = Get-CodeMigrations
    
    $orphans = $fileMigrations | Where-Object { $codeMigrations -notcontains $_ }
    
    if ($orphans.Count -eq 0) {
        Write-Host "No orphaned files found" -ForegroundColor Green
        return
    }
    
    Write-Host "Found $($orphans.Count) orphaned migration files:" -ForegroundColor Yellow
    foreach ($orphan in $orphans) {
        Write-Host "  • $orphan" -ForegroundColor Yellow
    }
    
    if (-not $Force -and -not $DryRun) {
        Write-Host ""
        $confirm = Read-Host "Remove all orphaned files? (y/N)"
        if ($confirm -ne 'y' -and $confirm -ne 'Y') {
            Write-Host "Cancelled" -ForegroundColor Gray
            return
        }
    }
    
    foreach ($orphan in $orphans) {
        if (-not $DryRun) {
            $files = @(
                (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$orphan.cs"),
                (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$orphan.Designer.cs")
            )
            foreach ($file in $files) {
                if (Test-PathCrossPlatform $file) {
                    Remove-ItemCrossPlatform $file
                    Write-Host "[OK] Removed $file" -ForegroundColor Green
                }
            }
        } else {
            Write-Host "[DRY-RUN] Would remove $orphan files" -ForegroundColor Gray
        }
    }
    
    if (-not $DryRun -and $orphans.Count -gt 0) {
        Write-Host ""
        Write-Host "[CLEANUP COMPLETE] Removed $($orphans.Count) orphaned files" -ForegroundColor Green
        
        # Update documentation after cleanup
        Update-MigrationDocumentation
    }
}

function Add-Migration {
    if (-not $MigrationName) {
        Write-Host "[ERROR] Migration name is required for add-migration action" -ForegroundColor Red
        Write-Host "Usage: .\migration.ps1 -Action add-migration -MigrationName 'YourMigrationName'" -ForegroundColor Yellow
        return
    }
    
    Write-Host "=== ADDING MIGRATION ===" -ForegroundColor Cyan
    Write-Host "Migration Name: $MigrationName" -ForegroundColor Gray
    Write-Host ""
    
    # Validate migration name
    if ($MigrationName -notmatch '^[A-Za-z][A-Za-z0-9_]*$') {
        Write-Host "[ERROR] Invalid migration name. Use only letters, numbers, and underscores, starting with a letter." -ForegroundColor Red
        return
    }
    
    # Check if migration already exists (both local and container)
    Write-Host "Checking for existing migrations..." -ForegroundColor Yellow
    try {
        $existingMigrations = Get-CodeMigrations
        $duplicateCheck = $existingMigrations | Where-Object { $_ -like "*$MigrationName*" }
        
        if ($duplicateCheck) {
            Write-Host "[WARNING] Similar migration name found:" -ForegroundColor Yellow
            foreach ($existing in $duplicateCheck) {
                Write-Host "  • $existing" -ForegroundColor Yellow
            }
            
            if (-not $Force) {
                Write-Host ""
                $confirm = Read-Host "Continue anyway? (y/N)"
                if ($confirm -ne 'y' -and $confirm -ne 'Y') {
                    Write-Host "Cancelled" -ForegroundColor Gray
                    return
                }
            }
        }
    } catch {
        Write-Host "[WARNING] Could not check existing migrations: $($_.Exception.Message)" -ForegroundColor Yellow
    }
    
    # Create migration directly in container (more reliable)
    Write-Host "Creating migration in container..." -ForegroundColor Yellow
    
    if (-not $DryRun) {
        $migrationResult = docker exec ugh-backend dotnet ef migrations add $MigrationName 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "[OK] Migration created successfully in container" -ForegroundColor Green
            
            # Show created files from container
            $containerFiles = docker exec ugh-backend ls Migrations/ | Where-Object { $_ -like "*$MigrationName*" }
            if ($containerFiles) {
                Write-Host ""
                Write-Host "Created files in container:" -ForegroundColor Gray
                foreach ($file in $containerFiles) {
                    Write-Host "  • $file" -ForegroundColor Gray
                }
            }
            
            # Copy files from container to local (if needed)
            Write-Host ""
            Write-Host "Syncing files to local filesystem..." -ForegroundColor Yellow
            foreach ($file in $containerFiles) {
                docker exec ugh-backend cat "Migrations/$file" | Out-File (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\$file") -Encoding UTF8
                Write-Host "  • Synced: $file" -ForegroundColor Gray
            }
            
            Write-Host ""
            Write-Host "Next steps:" -ForegroundColor Yellow
            Write-Host "  1. Review the generated migration files" -ForegroundColor Gray
            Write-Host "  2. Apply with: docker exec ugh-backend dotnet ef database update" -ForegroundColor Gray
            Write-Host "  3. Or check status: .\migration.ps1 -Action status" -ForegroundColor Gray
            
            # Update documentation after successful migration creation
            Update-MigrationDocumentation
            
        } else {
            # Handle different types of results
            if ($migrationResult -like "*No changes were detected*") {
                Write-Host "[INFO] No model changes detected - no migration created" -ForegroundColor Yellow
                Write-Host "This is normal if your models haven't changed since the last migration" -ForegroundColor Gray
            } else {
                Write-Host "[ERROR] Migration creation failed:" -ForegroundColor Red
                Write-Host $migrationResult -ForegroundColor Red
            }
        }
    } else {
        Write-Host "[DRY-RUN] Would create migration: $MigrationName in container" -ForegroundColor Gray
    }
}

function Force-Rebuild {
    Write-Host "=== FORCE REBUILD MIGRATION SYSTEM ===" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "[EXTREME CAUTION] This will completely rebuild the migration system!" -ForegroundColor Red
    Write-Host "Only use this if the system is completely broken." -ForegroundColor Red
    Write-Host ""
    
    if (-not $Force) {
        $confirm = Read-Host "Type 'NUCLEAR' to continue with complete rebuild"
        if ($confirm -ne "NUCLEAR") {
            Write-Host "Cancelled" -ForegroundColor Gray
            return
        }
    }
    
    Write-Host "Starting nuclear rebuild..." -ForegroundColor Yellow
    
    # 1. Backup current state
    Write-Host "1. Creating backup..." -ForegroundColor Yellow
    $backupDir = "migration-backup-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
    New-ItemCrossPlatform $backupDir "Directory"
    Copy-ItemCrossPlatform (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations\*") $backupDir -Force
    Write-Host "   Backup created in: $backupDir" -ForegroundColor Green
    
    # 2. Clear migration history
    Write-Host "2. Clearing migration history..." -ForegroundColor Yellow
    if (-not $DryRun) {
        docker exec ugh-db mysql -uuser -ppassword db -e "DELETE FROM __EFMigrationsHistory"
    }
    
    # 3. Remove all migration files except model snapshot
    Write-Host "3. Removing migration files..." -ForegroundColor Yellow
    if (-not $DryRun) {
        Get-ChildItemCrossPlatform (Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend$PathSeparator Migrations") -Filter "*.cs" | Where-Object { 
            $_.Name -notlike "*ModelSnapshot*" 
        } | Remove-ItemCrossPlatform -Force
    }
    
    # 4. Create initial migration
    Write-Host "4. Creating fresh initial migration..." -ForegroundColor Yellow
    if (-not $DryRun) {
        $backendPath = Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator Backend"
        Push-Location $backendPath
        try {
            $result = dotnet ef migrations add InitialMigration --force 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "   [OK] Initial migration created" -ForegroundColor Green
            } else {
                throw "Failed to create initial migration: $result"
            }
        } finally {
            Pop-Location
        }
    }
    
    Write-Host ""
    Write-Host "[REBUILD COMPLETE] System has been reset to clean state" -ForegroundColor Green
    Write-Host "You can now apply migrations with: dotnet ef database update" -ForegroundColor Yellow
    
    # Update documentation after rebuild
    Update-MigrationDocumentation
}

function Update-MigrationDocumentation {
    Write-Host "Updating MIGRATION-SYSTEM.md documentation..." -ForegroundColor Yellow
    $docPath = Join-PathCrossPlatform $PSScriptRoot "..$PathSeparator..$PathSeparator docs$PathSeparator MIGRATION-SYSTEM.md"
    if (-not (Test-PathCrossPlatform $docPath)) {
        Write-Host "Warning: MIGRATION-SYSTEM.md not found at $docPath" -ForegroundColor Yellow
        return
    }
    
    try {
        # Get current migration data
        $dbMigrations = Get-AppliedMigrations
        $totalMigrations = $dbMigrations.Count
        $currentDate = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        
        # Read current documentation
        $content = Get-Content $docPath -Raw
        
        # Update migration count
        $content = $content -replace '\*\*Total Migrations:\*\* \d+', "**Total Migrations:** $totalMigrations"
        $content = $content -replace '\*\*Applied Migrations:\*\* \d+', "**Applied Migrations:** $totalMigrations"
        
        # Update last updated timestamp
        $content = $content -replace '\*Last updated on [^\*]+\*', "*Last updated on $currentDate*"
        $content = $content -replace 'Last updated: [^\r\n]+', "Last updated: $currentDate by Battle-Tested Migration Management System"
        
        # Update evolution section to reflect current script name
        $content = $content -replace '- \*\*V4\*\*: migration-repair\.ps1 \(current[^)]*\)', "- **V4**: migration.ps1 (current - proven, reliable, battle-tested)"
        
        # Update script references in documentation
        $content = $content -replace 'migration-repair\.ps1', 'migration.ps1'
        
        # Write updated content
        Set-Content -Path $docPath -Value $content -Encoding UTF8
        
        Write-Host "   [OK] Documentation updated successfully" -ForegroundColor Green
        Write-Host "   - Total migrations: $totalMigrations" -ForegroundColor Gray
        Write-Host "   - Last updated: $currentDate" -ForegroundColor Gray
        
    } catch {
        Write-Host "   [WARNING] Failed to update documentation: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

# Main execution
try {
    # Check if containers are running first
    if (-not (Test-ContainersRunning)) {
        exit 1
    }
    
    switch ($Action.ToLower()) {
        "status" { Show-Status }
        "fix-inconsistencies" { Fix-Inconsistencies }
        "clean-orphans" { Clean-Orphans }
        "add-migration" { Add-Migration }
        "force-rebuild" { Force-Rebuild }
        "schema-check" { Schema-Check }
        default { Write-Host "Unknown action: $Action" -ForegroundColor Red }
    }
} catch {
    Write-Host "[ERROR] $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

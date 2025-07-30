param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("status", "add", "fix", "clean", "force-rebuild", "schema-check")]
    [string]$Action,
    [string]$MigrationName = ""
)

$ErrorActionPreference = "Stop"

function Test-ContainersRunning {
    Write-Host "Checking container availability..." -ForegroundColor Yellow
    $requiredContainers = @("ugh-db", "ugh-backend")
    $missingContainers = @()
    foreach ($container in $requiredContainers) {
        $status = docker ps --filter "name=$container" --format "{{.Status}}"
        if (-not $status) {
            $missingContainers += $container
        } else {
            Write-Host "  [OK] $container is running" -ForegroundColor Green
        }
    }
    if ($missingContainers.Count -gt 0) {
        Write-Host "[ERROR] Missing containers: $($missingContainers -join ', ')" -ForegroundColor Red
        Write-Host "Please start the application with: docker compose up -d" -ForegroundColor Yellow
        return $false
    }
    return $true
}

function Test-EfTools {
    Write-Host "Checking EF Tools in container..." -ForegroundColor Yellow
    $result = docker exec ugh-backend dotnet ef --version 2>$null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Installing EF Tools in container..." -ForegroundColor Yellow
        docker exec ugh-backend dotnet tool install --global dotnet-ef
        if ($LASTEXITCODE -ne 0) {
            Write-Host "[ERROR] Failed to install EF Tools" -ForegroundColor Red
            return $false
        }
    }
    Write-Host "  [OK] EF Tools available" -ForegroundColor Green
    return $true
}

function Show-Status {
    Write-Host "=== MIGRATION SYSTEM STATUS ===" -ForegroundColor Cyan
    Write-Host ""
    
    # Database migrations
    Write-Host "Database Migrations:" -ForegroundColor Yellow
    $oldErrorAction = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        $dbMigrationsRaw = docker exec ugh-db mysql -uuser -ppassword db --skip-column-names --silent -e "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId" 2>&1
        $dbMigrations = $dbMigrationsRaw | Where-Object { $_ -and $_ -notlike "*Warning*" -and $_ -notlike "*MigrationId*" }
        if ($dbMigrations.Count -gt 0) {
            foreach ($migration in $dbMigrations) {
                Write-Host "  [DB] $migration" -ForegroundColor Green
            }
        } else {
            Write-Host "  [INFO] No migrations found in database" -ForegroundColor Yellow
        }
    } catch {
        Write-Host "  [ERROR] Cannot read database migrations" -ForegroundColor Red
    }
    $ErrorActionPreference = $oldErrorAction
    
    Write-Host ""
    Write-Host "Code Migrations:" -ForegroundColor Yellow
    $migrationsPath = Join-Path $PSScriptRoot "..\..\Backend\Migrations"
    if (Test-Path $migrationsPath) {
        $codeMigrations = Get-ChildItem -Path $migrationsPath -Filter "*.cs" | Where-Object { $_.Name -notlike "*.Designer.cs" -and $_.Name -notlike "*ModelSnapshot.cs" } | Sort-Object Name
        foreach ($migration in $codeMigrations) {
            Write-Host "  [CODE] $($migration.BaseName)" -ForegroundColor Blue
        }
    } else {
        Write-Host "  [ERROR] Migrations directory not found" -ForegroundColor Red
    }
}

function Add-Migration {
    if ([string]::IsNullOrWhiteSpace($MigrationName)) {
        Write-Host "[ERROR] Migration name is required. Use: -MigrationName 'YourMigrationName'" -ForegroundColor Red
        return
    }
    
    Write-Host "Adding migration: $MigrationName" -ForegroundColor Yellow
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $fullName = "${timestamp}_$MigrationName"
    
    $result = docker exec ugh-backend dotnet ef migrations add $fullName --project /app/Backend
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[SUCCESS] Migration '$fullName' created successfully" -ForegroundColor Green
    } else {
        Write-Host "[ERROR] Failed to create migration" -ForegroundColor Red
    }
}

function Fix-Inconsistencies {
    Write-Host "Checking for migration inconsistencies..." -ForegroundColor Yellow
    
    # Get database migrations
    $oldErrorAction = $ErrorActionPreference
    $ErrorActionPreference = "Continue"
    try {
        $dbMigrationsRaw = docker exec ugh-db mysql -uuser -ppassword db --skip-column-names --silent -e "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId" 2>&1
        $dbMigrations = $dbMigrationsRaw | Where-Object { $_ -and $_ -notlike "*Warning*" -and $_ -notlike "*MigrationId*" }
        if (-not $dbMigrations -or $dbMigrations.Count -eq 0) {
            Write-Host "[INFO] No migrations found in database" -ForegroundColor Yellow
            $ErrorActionPreference = $oldErrorAction
            return
        }
    } catch {
        Write-Host "[ERROR] Cannot read database migrations" -ForegroundColor Red
        $ErrorActionPreference = $oldErrorAction
        return
    }
    $ErrorActionPreference = $oldErrorAction
    
    # Get code migrations
    $migrationsPath = Join-Path $PSScriptRoot "..\..\Backend\Migrations"
    if (-not (Test-Path $migrationsPath)) {
        Write-Host "[ERROR] Migrations directory not found" -ForegroundColor Red
        return
    }
    
    $codeMigrations = Get-ChildItem -Path $migrationsPath -Filter "*.cs" | Where-Object { $_.Name -notlike "*.Designer.cs" -and $_.Name -notlike "*ModelSnapshot.cs" } | Sort-Object Name
    
    $dbMigrationNames = $dbMigrations | ForEach-Object { $_.Split('_', 2)[1] }
    $codeMigrationNames = $codeMigrations | ForEach-Object { $_.BaseName.Split('_', 2)[1] }
    
    $orphanDb = $dbMigrationNames | Where-Object { $_ -notin $codeMigrationNames }
    $orphanCode = $codeMigrationNames | Where-Object { $_ -notin $dbMigrationNames }
    
    if ($orphanDb.Count -gt 0) {
        Write-Host "[WARNING] Orphaned database migrations:" -ForegroundColor Yellow
        foreach ($orphan in $orphanDb) {
            Write-Host "  [DB] $orphan" -ForegroundColor Red
        }
    }
    
    if ($orphanCode.Count -gt 0) {
        Write-Host "[WARNING] Orphaned code migrations:" -ForegroundColor Yellow
        foreach ($orphan in $orphanCode) {
            Write-Host "  [CODE] $orphan" -ForegroundColor Red
        }
    }
    
    if ($orphanDb.Count -eq 0 -and $orphanCode.Count -eq 0) {
        Write-Host "[OK] No inconsistencies found" -ForegroundColor Green
    }
}

function Clean-Orphans {
    Write-Host "Cleaning orphaned migration files..." -ForegroundColor Yellow
    
    $migrationsPath = Join-Path $PSScriptRoot "..\..\Backend\Migrations"
    if (-not (Test-Path $migrationsPath)) {
        Write-Host "[ERROR] Migrations directory not found" -ForegroundColor Red
        return
    }
    
    $backupDir = "migration-backup-" + (Get-Date -Format "yyyyMMdd-HHmmss")
    New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
    
    $orphanedFiles = Get-ChildItem -Path $migrationsPath -Filter "*.cs" | Where-Object { 
        $_.Name -notlike "*.Designer.cs" -and $_.Name -notlike "*ModelSnapshot.cs" 
    }
    
    foreach ($file in $orphanedFiles) {
        $designerFile = Join-Path $migrationsPath "$($file.BaseName).Designer.cs"
        if (Test-Path $designerFile) {
            Write-Host "Backing up: $($file.Name)" -ForegroundColor Yellow
            Copy-Item $file.FullName (Join-Path $backupDir $file.Name)
            Copy-Item $designerFile (Join-Path $backupDir "$($file.BaseName).Designer.cs")
            Remove-Item $file.FullName -Force
            Remove-Item $designerFile -Force
        }
    }
    
    Write-Host "[SUCCESS] Orphaned files backed up to: $backupDir" -ForegroundColor Green
}

function Force-Rebuild {
    Write-Host "=== FORCE REBUILD - DESTRUCTIVE OPERATION ===" -ForegroundColor Red
    Write-Host "This will:"
    Write-Host "  1. Drop all migrations from database"
    Write-Host "  2. Remove all migration files"
    Write-Host "  3. Create a new initial migration"
    Write-Host ""
    
    $confirm = Read-Host "Type 'NUCLEAR' to continue with complete rebuild"
    if ($confirm -ne "NUCLEAR") {
        Write-Host "Operation cancelled" -ForegroundColor Yellow
        return
    }
    
    Write-Host "Starting nuclear rebuild..." -ForegroundColor Red
    
    # Drop migrations from database
    Write-Host "Dropping migrations from database..." -ForegroundColor Yellow
    docker exec ugh-db mysql -uuser -ppassword db --silent -e "DROP TABLE IF EXISTS __EFMigrationsHistory" 2>$null
    
    # Remove migration files
    Write-Host "Removing migration files..." -ForegroundColor Yellow
    $migrationsPath = Join-Path $PSScriptRoot "..\..\Backend\Migrations"
    if (Test-Path $migrationsPath) {
        Get-ChildItem -Path $migrationsPath -Filter "*.cs" | Remove-Item -Force
    }
    
    # Create new initial migration
    Write-Host "Creating new initial migration..." -ForegroundColor Yellow
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $result = docker exec ugh-backend dotnet ef migrations add InitialCreate_$timestamp --project /app/Backend
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[SUCCESS] Nuclear rebuild completed" -ForegroundColor Green
    } else {
        Write-Host "[ERROR] Failed to create new migration" -ForegroundColor Red
    }
}

function Schema-Check {
    Write-Host "Checking schema consistency..." -ForegroundColor Yellow
    
    # Check if models have changed since last migration
    $timestamp = Get-Date -Format "yyyyMMdd-HHmmss"
    $result = docker exec ugh-backend dotnet ef migrations add SchemaCheck_$timestamp --project /app/Backend 2>&1
    
    if ($result -like "*No files were generated*" -or $result -like "*models haven't changed since the last migration*") {
        Write-Host "[OK] Schema is consistent with migrations" -ForegroundColor Green
        # Remove the test migration if it was created
        docker exec ugh-backend dotnet ef migrations remove --project /app/Backend --force 2>$null
    } else {
        Write-Host "[WARNING] Schema changes detected:" -ForegroundColor Yellow
        Write-Host $result -ForegroundColor Cyan
        # Remove the test migration if it was created
        docker exec ugh-backend dotnet ef migrations remove --project /app/Backend --force 2>$null
    }
}

try {
    if (-not (Test-ContainersRunning)) { exit 1 }
    if (-not (Test-EfTools)) { exit 1 }
    
    switch ($Action.ToLower()) {
        "status" { Show-Status }
        "add" { Add-Migration }
        "fix" { Fix-Inconsistencies }
        "clean" { Clean-Orphans }
        "force-rebuild" { Force-Rebuild }
        "schema-check" { Schema-Check }
        default { Write-Host "Unknown action: $Action" -ForegroundColor Red }
    }
} catch {
    Write-Host "[ERROR] Migration operation failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}
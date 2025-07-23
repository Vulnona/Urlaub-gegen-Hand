# Enhanced Migration Management System for UGH
# Provides full control while maintaining automation
# Commands: validate, cleanup, run, add, remove, status, sync

param(
    [Parameter(Mandatory=$true)]
    [ValidateSet("validate", "cleanup", "run", "add", "remove", "status", "sync", "orphans")]
    [string]$Action,
    
    [Parameter(Mandatory=$false)]
    [string]$MigrationName,
    
    [Parameter(Mandatory=$false)]
    [switch]$Force,
    
    [Parameter(Mandatory=$false)]
    [switch]$DryRun,
    
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

$ErrorActionPreference = "Stop"

# Configuration
$script:config = @{
    DbContainer = "ugh-db"
    BackendContainer = "ugh-backend"
    DbName = "db"
    DbUser = "root"
    DbPasswordFile = ".docker\db\secrets\.db-root-password.txt"
    MigrationPath = "Backend\Migrations"
    DocsPath = "Docs\MIGRATION-SYSTEM.md"
    BackupPath = "/tmp/migration-backups"
    MaxBackups = 10
    ProjectPath = "Backend"
}

function Show-Help {
    Write-Host "Enhanced Migration Management System for UGH" -ForegroundColor Cyan
    Write-Host "=============================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Commands:" -ForegroundColor White
    Write-Host "  validate       Check system integrity and migration status" -ForegroundColor Gray
    Write-Host "  cleanup        Clean old backups and temporary files" -ForegroundColor Gray
    Write-Host "  run            Execute full migration pipeline with validation" -ForegroundColor Gray
    Write-Host "  status         Show detailed system overview" -ForegroundColor Gray
    Write-Host "  add [name]     Create new migration with Entity Framework" -ForegroundColor Gray
    Write-Host "  remove         Remove last migration (use -Force)" -ForegroundColor Gray
    Write-Host "  sync           Synchronize migration history" -ForegroundColor Gray
    Write-Host "  orphans        Find and remove orphaned migration files" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor White
    Write-Host "  .\enhanced-migration.ps1 status" -ForegroundColor Gray
    Write-Host "  .\enhanced-migration.ps1 add -MigrationName 'AddUserProfile'" -ForegroundColor Gray
    Write-Host "  .\enhanced-migration.ps1 remove -Force" -ForegroundColor Gray
    Write-Host "  .\enhanced-migration.ps1 orphans -DryRun" -ForegroundColor Gray
}

function Get-DbPassword {
    $passwordPath = Join-Path $PSScriptRoot "../../$($script:config.DbPasswordFile)"
    if (Test-Path $passwordPath) {
        return Get-Content $passwordPath -Raw | ForEach-Object { $_.Trim() }
    }
    throw "Database password file not found: $passwordPath"
}

function Test-DockerContainers {
    Write-Host "Pruefe Docker Container..." -ForegroundColor Yellow
    
    # Database Container pruefen
    $dbStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String $script:config.DbContainer
    if (-not $dbStatus) {
        throw "Database Container '$($script:config.DbContainer)' nicht verfuegbar"
    }
    
    # Backend Container pruefen
    $backendStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String $script:config.BackendContainer
    if (-not $backendStatus) {
        throw "Backend Container '$($script:config.BackendContainer)' nicht verfuegbar"
    }
    
    Write-Host "[OK] Alle Container verfuegbar" -ForegroundColor Green
}

function Test-EFTools {
    Write-Host "Pruefe Entity Framework Tools..." -ForegroundColor Yellow
    
    try {
        $efVersion = docker exec $script:config.BackendContainer dotnet ef --version 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "[OK] EF Tools verfuegbar" -ForegroundColor Green
        } else {
            throw "EF Tools nicht verfuegbar"
        }
    } catch {
        Write-Host "[WARN] Installiere EF Tools..." -ForegroundColor Yellow
        docker exec $script:config.BackendContainer dotnet tool restore
        if ($LASTEXITCODE -ne 0) {
            throw "Fehler beim Installieren der EF Tools"
        }
        Write-Host "[OK] EF Tools installiert" -ForegroundColor Green
    }
}

function Test-DatabaseConnection {
    Write-Host "Pruefe Datenbankverbindung..." -ForegroundColor Yellow
    
    $password = Get-DbPassword
    try {
        # Teste direkt mit Start-Process für bessere Fehlerbehandlung
        $pinfo = New-Object System.Diagnostics.ProcessStartInfo
        $pinfo.FileName = "docker"
        $pinfo.Arguments = "exec $($script:config.DbContainer) mysql -u$($script:config.DbUser) -p`"$password`" -e `"SELECT 1;`""
        $pinfo.UseShellExecute = $false
        $pinfo.RedirectStandardOutput = $true
        $pinfo.RedirectStandardError = $true
        
        $process = New-Object System.Diagnostics.Process
        $process.StartInfo = $pinfo
        $process.Start() | Out-Null
        $process.WaitForExit()
        
        if ($process.ExitCode -eq 0) {
            Write-Host "[OK] Datenbankverbindung erfolgreich" -ForegroundColor Green
        } else {
            $stderr = $process.StandardError.ReadToEnd()
            throw "Datenbankverbindung fehlgeschlagen - Exit Code: $($process.ExitCode), Error: $stderr"
        }
    } catch {
        throw "Kann keine Verbindung zur Datenbank herstellen: $($_.Exception.Message)"
    }
}

function Get-DatabaseMigrations {
    try {
        $password = Get-DbPassword
        $query = "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId;"
        $cmd = "docker exec $($script:config.DbContainer) mysql -u$($script:config.DbUser) -p`"$password`" $($script:config.DbName) -e `"$query`" 2>nul"
        $result = Invoke-Expression $cmd
        
        if ($LASTEXITCODE -eq 0) {
            return $result | Where-Object { $_ -and $_ -ne "MigrationId" }
        } else {
            return @()
        }
    } catch {
        return @()
    }
}

function Get-MigrationList {
    Write-Host "Lade Migration-Liste..." -ForegroundColor Yellow
    
    try {
        $pinfo = New-Object System.Diagnostics.ProcessStartInfo
        $pinfo.FileName = "docker"
        $pinfo.Arguments = "exec $($script:config.BackendContainer) dotnet ef migrations list"
        $pinfo.UseShellExecute = $false
        $pinfo.RedirectStandardOutput = $true
        $pinfo.RedirectStandardError = $true
        
        $process = New-Object System.Diagnostics.Process
        $process.StartInfo = $pinfo
        $process.Start() | Out-Null
        $process.WaitForExit()
        
        if ($process.ExitCode -eq 0) {
            $output = $process.StandardOutput.ReadToEnd()
            $migrations = $output -split "`n" | Where-Object { $_ -match '^\d{14}_' }
            Write-Host "[OK] $($migrations.Count) Migrationen gefunden" -ForegroundColor Green
            return $migrations
        } else {
            $stderr = $process.StandardError.ReadToEnd()
            throw "Fehler beim Laden der Migration-Liste: $stderr"
        }
    } catch {
        throw "Kann Migration-Liste nicht laden: $($_.Exception.Message)"
    }
}

function Update-MigrationDocumentation {
    Write-Host "Aktualisiere Migration-Dokumentation..." -ForegroundColor Yellow
    
    try {
        $docsPath = Join-Path $PSScriptRoot "../../$($script:config.DocsPath)"
        
        if (-not (Test-Path $docsPath)) {
            Write-Host "  Dokumentations-Datei nicht gefunden, ueberspringe Update" -ForegroundColor Yellow
            return
        }
        
        $allMigrations = Get-MigrationList
        $appliedMigrations = Get-DatabaseMigrations
        
        $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
        
        # Lese aktuellen Inhalt
        $content = Get-Content $docsPath -Raw
        
        # Aktualisiere Timestamp
        if ($content -match "Auto-generated on [^*]+") {
            $newTimestamp = "*Auto-generated on $timestamp*"
            $content = $content -replace "\*Auto-generated on [^*]+\*", $newTimestamp
        }
        
        # Generiere Migration Status Sektion
        $migrationStatus = @()
        $migrationStatus += ""
        $migrationStatus += "## Migration Status"
        $migrationStatus += ""
        $migrationStatus += "| Migration | Status | Applied |"
        $migrationStatus += "|-----------|--------|---------|"
        
        foreach ($migration in $allMigrations) {
            $cleanName = $migration -replace "^\d+_", "" -replace "_", " "
            $isPending = $migration -match "\(Pending\)"
            $isApplied = $appliedMigrations -contains $migration
            
            if ($isPending) {
                $status = "Pending"
                $applied = "No"
                $cleanName = $cleanName -replace " \(Pending\)", ""
            } elseif ($isApplied) {
                $status = "Applied" 
                $applied = "Yes"
            } else {
                $status = "Ready"
                $applied = "No"
            }
            
            $migrationStatus += "| $cleanName | $status | $applied |"
        }
        
        $migrationStatus += ""
        $migrationStatus += "**Total Migrations**: $($allMigrations.Count)"
        $migrationStatus += "**Applied Migrations**: $($appliedMigrations.Count)"
        $migrationStatus += "**Pending Migrations**: $(($allMigrations | Where-Object { $_ -match '\(Pending\)' }).Count)"
        $migrationStatus += ""
        $migrationStatus += "*Last updated: $timestamp*"
        
        # Einfache Aktualisierung - fuege am Ende hinzu wenn nicht vorhanden
        if ($content -notmatch "## Migration Status") {
            $content += "`n`n" + ($migrationStatus -join "`n")
        }
        
        # Schreibe aktualisierten Inhalt
        Set-Content -Path $docsPath -Value $content -NoNewline
        Write-Host "  [OK] Dokumentation erfolgreich aktualisiert" -ForegroundColor Green
        
    } catch {
        Write-Host "  [WARN] Konnte Dokumentation nicht aktualisieren: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

function Invoke-Validation {
    Write-Host "=== VALIDATION ===" -ForegroundColor Cyan
    
    try {
        Test-DockerContainers
        Test-EFTools
        Test-DatabaseConnection
        
        $migrations = Get-MigrationList
        
        Write-Host ""
        Write-Host "System Status:" -ForegroundColor Cyan
        Write-Host "  Database: OK Verbunden" -ForegroundColor Green
        Write-Host "  EF Tools: OK Verfuegbar" -ForegroundColor Green
        Write-Host "  Migrationen: $($migrations.Count) verfuegbar" -ForegroundColor Green
        
        if ($migrations.Count -eq 0) {
            Write-Host "  [WARN] Keine Migrationen gefunden" -ForegroundColor Yellow
        }
        
        Write-Host "[OK] Validation erfolgreich" -ForegroundColor Green
        return $true
        
    } catch {
        Write-Host "[ERROR] Validation fehlgeschlagen: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

function Invoke-Status {
    Write-Host "=== MIGRATION STATUS ===" -ForegroundColor Cyan
    
    if (-not (Invoke-Validation)) {
        return
    }
    
    $migrations = Get-MigrationList
    
    Write-Host ""
    Write-Host "Migration Details:" -ForegroundColor Cyan
    
    if ($migrations.Count -eq 0) {
        Write-Host "  Keine Migrationen vorhanden" -ForegroundColor Yellow
    } else {
        foreach ($migration in $migrations) {
            if ($migration -match "(Pending)") {
                Write-Host "  [PENDING] $migration" -ForegroundColor Yellow
            } else {
                Write-Host "  [APPLIED] $migration" -ForegroundColor Green
            }
        }
    }
    
    # Test Migration Status pruefen
    $testMigrations = $migrations | Where-Object { $_ -match "Test" }
    if ($testMigrations.Count -gt 0) {
        Write-Host ""
        Write-Host "[WARN] Test-Migrationen gefunden:" -ForegroundColor Yellow
        foreach ($testMig in $testMigrations) {
            Write-Host "  - $testMig" -ForegroundColor Yellow
        }
        Write-Host "Empfehlung: Test-Migrationen mit 'remove' entfernen" -ForegroundColor Yellow
    }
}

function Add-Migration {
    param([string]$Name)
    
    if (-not $Name) {
        throw "Migration-Name ist erforderlich fuer 'add' Befehl"
    }
    
    Write-Host "=== ADD MIGRATION ===" -ForegroundColor Cyan
    Write-Host "Migration Name: $Name" -ForegroundColor Gray
    
    if (-not (Invoke-Validation)) {
        throw "Validation fehlgeschlagen"
    }
    
    if (-not $DryRun) {
        Write-Host "Erstelle neue Migration..." -ForegroundColor Yellow
        
        try {
            $result = docker exec $script:config.BackendContainer dotnet ef migrations add $Name --output-dir Migrations 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "[OK] Migration '$Name' erfolgreich erstellt" -ForegroundColor Green
                Write-Host $result -ForegroundColor Gray
                
                # Aktualisiere Dokumentation
                Update-MigrationDocumentation
            } else {
                throw "Migration-Erstellung fehlgeschlagen: $result"
            }
        } catch {
            throw "Fehler beim Erstellen der Migration: $($_.Exception.Message)"
        }
    } else {
        Write-Host "[DRY-RUN] Wuerde Migration '$Name' erstellen" -ForegroundColor Yellow
    }
}

function Remove-Migration {
    Write-Host "=== REMOVE MIGRATION ===" -ForegroundColor Cyan
    
    if (-not (Invoke-Validation)) {
        throw "Validation fehlgeschlagen"
    }
    
    $migrations = Get-MigrationList
    if ($migrations.Count -eq 0) {
        Write-Host "Keine Migrationen zum Entfernen vorhanden" -ForegroundColor Yellow
        return
    }
    
    $lastMigration = $migrations | Select-Object -Last 1
    Write-Host "Letzte Migration: $lastMigration" -ForegroundColor Gray
    
    if (-not $Force -and -not $DryRun) {
        $confirm = Read-Host "Migration '$lastMigration' entfernen? (y/N)"
        if ($confirm -ne 'y' -and $confirm -ne 'Y') {
            Write-Host "Entfernen abgebrochen" -ForegroundColor Yellow
            return
        }
    }
    
    if (-not $DryRun) {
        Write-Host "Entferne Migration..." -ForegroundColor Yellow
        
        try {
            $result = docker exec $script:config.BackendContainer dotnet ef migrations remove --force 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "[OK] Migration erfolgreich entfernt" -ForegroundColor Green
                Write-Host $result -ForegroundColor Gray
                
                # Aktualisiere Dokumentation
                Update-MigrationDocumentation
            } else {
                throw "Migration-Entfernung fehlgeschlagen: $result"
            }
        } catch {
            throw "Fehler beim Entfernen der Migration: $($_.Exception.Message)"
        }
    } else {
        Write-Host "[DRY-RUN] Wuerde Migration '$lastMigration' entfernen" -ForegroundColor Yellow
    }
}

function Invoke-Sync {
    Write-Host "=== SYNC MIGRATIONS ===" -ForegroundColor Cyan
    
    if (-not (Invoke-Validation)) {
        throw "Validation fehlgeschlagen"
    }
    
    if (-not $DryRun) {
        Write-Host "Synchronisiere Datenbank..." -ForegroundColor Yellow
        
        try {
            $result = docker exec $script:config.BackendContainer dotnet ef database update 2>&1
            if ($LASTEXITCODE -eq 0) {
                Write-Host "[OK] Datenbank synchronisiert" -ForegroundColor Green
                Write-Host $result -ForegroundColor Gray
                
                # Aktualisiere Dokumentation nach erfolgreicher Synchronisation
                Update-MigrationDocumentation
            } else {
                throw "Synchronisation fehlgeschlagen: $result"
            }
        } catch {
            throw "Fehler bei der Synchronisation: $($_.Exception.Message)"
        }
    } else {
        Write-Host "[DRY-RUN] Wuerde Datenbank synchronisieren" -ForegroundColor Yellow
    }
}

function Invoke-Run {
    Write-Host "=== RUN FULL PIPELINE ===" -ForegroundColor Cyan
    
    Write-Host "1. Validation..." -ForegroundColor Yellow
    if (-not (Invoke-Validation)) {
        throw "Pipeline gestoppt: Validation fehlgeschlagen"
    }
    
    Write-Host "2. Synchronisation..." -ForegroundColor Yellow
    Invoke-Sync
    
    Write-Host "3. Status Check..." -ForegroundColor Yellow
    Invoke-Status
    
    Write-Host "[OK] Pipeline erfolgreich abgeschlossen" -ForegroundColor Green
}

function Get-OrphanMigrations {
    Write-Host "Suche nach Orphan-Migrationen..." -ForegroundColor Yellow
    
    try {
        # Hole EF-registrierte Migrationen
        $efMigrations = Get-MigrationList
        
        # Hole physische Migration-Dateien
        $migrationDir = Join-Path $PSScriptRoot "../../$($script:config.MigrationPath)"
        $physicalFiles = Get-ChildItem $migrationDir -Filter "*.cs" | Where-Object { 
            $_.Name -match "^\d{14}_.*\.cs$" -and $_.Name -notmatch "\.Designer\.cs$" 
        }
        
        $orphans = @()
        
        foreach ($file in $physicalFiles) {
            $migrationId = $file.Name -replace "\.cs$", ""
            $isRegistered = $efMigrations | Where-Object { $_ -eq $migrationId }
            
            if (-not $isRegistered) {
                $orphans += @{
                    File = $file.FullName
                    DesignerFile = $file.FullName -replace "\.cs$", ".Designer.cs"
                    MigrationId = $migrationId
                    Name = $migrationId -replace "^\d{14}_", ""
                }
            }
        }
        
        Write-Host "[OK] $($orphans.Count) Orphan-Migrationen gefunden" -ForegroundColor Green
        return $orphans
        
    } catch {
        throw "Fehler beim Suchen von Orphan-Migrationen: $($_.Exception.Message)"
    }
}

function Invoke-OrphanCleanup {
    Write-Host "=== ORPHAN CLEANUP ===" -ForegroundColor Cyan
    
    if (-not (Invoke-Validation)) {
        throw "Validation fehlgeschlagen"
    }
    
    $orphans = Get-OrphanMigrations
    
    if ($orphans.Count -eq 0) {
        Write-Host "Keine Orphan-Migrationen gefunden - System ist sauber!" -ForegroundColor Green
        return
    }
    
    Write-Host ""
    Write-Host "Gefundene Orphan-Migrationen:" -ForegroundColor Yellow
    foreach ($orphan in $orphans) {
        Write-Host "  [ORPHAN] $($orphan.MigrationId)" -ForegroundColor Red
        Write-Host "    - Hauptdatei: $($orphan.File)" -ForegroundColor Gray
        if (Test-Path $orphan.DesignerFile) {
            Write-Host "    - Designer: $($orphan.DesignerFile)" -ForegroundColor Gray
        }
    }
    
    Write-Host ""
    Write-Host "[WARNUNG] Diese Dateien sind nicht in EF registriert und können Probleme verursachen!" -ForegroundColor Yellow
    
    if (-not $Force -and -not $DryRun) {
        $confirm = Read-Host "Alle $($orphans.Count) Orphan-Migrationen loeschen? (y/N)"
        if ($confirm -ne 'y' -and $confirm -ne 'Y') {
            Write-Host "Cleanup abgebrochen" -ForegroundColor Yellow
            return
        }
    }
    
    if (-not $DryRun) {
        Write-Host "Loesche Orphan-Migrationen..." -ForegroundColor Yellow
        
        $deletedCount = 0
        foreach ($orphan in $orphans) {
            try {
                # Lösche Hauptdatei
                if (Test-Path $orphan.File) {
                    Remove-Item $orphan.File -Force
                    Write-Host "  [DELETED] $($orphan.File)" -ForegroundColor Red
                    $deletedCount++
                }
                
                # Lösche Designer-Datei falls vorhanden
                if (Test-Path $orphan.DesignerFile) {
                    Remove-Item $orphan.DesignerFile -Force
                    Write-Host "  [DELETED] $($orphan.DesignerFile)" -ForegroundColor Red
                }
                
            } catch {
                Write-Host "  [ERROR] Konnte $($orphan.File) nicht loeschen: $($_.Exception.Message)" -ForegroundColor Red
            }
        }
        
        Write-Host ""
        Write-Host "[OK] $deletedCount Orphan-Migrationen erfolgreich geloescht" -ForegroundColor Green
        
        # Aktualisiere Dokumentation nach Cleanup
        Update-MigrationDocumentation
        
    } else {
        Write-Host ""
        Write-Host "[DRY-RUN] Wuerde $($orphans.Count) Orphan-Migrationen loeschen:" -ForegroundColor Yellow
        foreach ($orphan in $orphans) {
            Write-Host "  - $($orphan.MigrationId)" -ForegroundColor Yellow
        }
    }
}

function Invoke-Cleanup {
    Write-Host "=== CLEANUP ===" -ForegroundColor Cyan
    
    Write-Host "Cleanup-Operationen..." -ForegroundColor Yellow
    
    if (-not $DryRun) {
        # Cleanup in Backend Container
        docker exec $script:config.BackendContainer find /tmp -name "*migration*" -type f -delete 2>/dev/null
        Write-Host "[OK] Temporaere Dateien bereinigt" -ForegroundColor Green
    } else {
        Write-Host "[DRY-RUN] Wuerde temporaere Dateien bereinigen" -ForegroundColor Yellow
    }
}

# Main Logic
try {
    if ($Help) {
        Show-Help
        exit 0
    }
    
    Write-Host "Enhanced Migration Management System" -ForegroundColor Cyan
    Write-Host "====================================" -ForegroundColor Cyan
    Write-Host "Action: $Action" -ForegroundColor Gray
    if ($DryRun) { Write-Host "Mode: DRY-RUN" -ForegroundColor Yellow }
    Write-Host ""
    
    switch ($Action.ToLower()) {
        "validate" { Invoke-Validation | Out-Null }
        "status" { Invoke-Status }
        "add" { Add-Migration -Name $MigrationName }
        "remove" { Remove-Migration }
        "sync" { Invoke-Sync }
        "run" { Invoke-Run }
        "cleanup" { Invoke-Cleanup }
        "orphans" { Invoke-OrphanCleanup }
        default { 
            Write-Host "[ERROR] Unbekannte Aktion: $Action" -ForegroundColor Red
            Show-Help
            exit 1
        }
    }
    
    Write-Host ""
    Write-Host "[OK] Enhanced Migration Management abgeschlossen" -ForegroundColor Green
    
} catch {
    Write-Host ""
    Write-Host "[ERROR] $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

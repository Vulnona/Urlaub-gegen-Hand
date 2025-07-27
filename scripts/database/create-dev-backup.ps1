# UGH Development Backup Creator
# Erstellt ein automatisches Backup der aktuellen Datenbank für Entwickler

param(
    [Parameter(Mandatory=$false)]
    [string]$BackupName = "dev-backup",
    [Parameter(Mandatory=$false)]
    [switch]$Force,
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

$ErrorActionPreference = "Stop"

function Show-Help {
    Write-Host "UGH Development Backup Creator" -ForegroundColor Cyan
    Write-Host "=================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\create-dev-backup.ps1" -ForegroundColor Gray
    Write-Host "  .\create-dev-backup.ps1 -BackupName my-backup" -ForegroundColor Gray
    Write-Host "  .\create-dev-backup.ps1 -Force" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor White
    Write-Host "  -BackupName      Name des Backups (default: dev-backup)" -ForegroundColor Gray
    Write-Host "  -Force           Überschreibt existierendes Backup ohne Bestätigung" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Dieses Skript:" -ForegroundColor White
    Write-Host "  1. Erstellt ein Backup der aktuellen Datenbank" -ForegroundColor Gray
    Write-Host "  2. Kopiert es als Standard-Startdatenbank" -ForegroundColor Gray
    Write-Host "  3. Aktualisiert die Docker-Konfiguration" -ForegroundColor Gray
}

function Test-Prerequisites {
    Write-Host "Prüfe Voraussetzungen..." -ForegroundColor Yellow
    
    # Docker prüfen
    try {
        $dockerVersion = docker --version
        Write-Host "✓ Docker gefunden: $dockerVersion" -ForegroundColor Green
    } catch {
        throw "Docker nicht gefunden. Bitte installieren Sie Docker Desktop."
    }
    
    # Container prüfen
    $containerStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String "ugh-db"
    if (-not $containerStatus) {
        throw "Container 'ugh-db' nicht gefunden oder nicht aktiv. Starten Sie zuerst das Projekt mit 'docker-compose up -d'"
    }
    
    Write-Host "✓ Container 'ugh-db' ist aktiv" -ForegroundColor Green
}

function Create-Backup {
    param([string]$Name)
    
    $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
    $backupFile = "ugh-dev-backup_$timestamp.sql"
    $backupPath = Join-Path (Get-Location) "backups" $backupFile
    $standardBackupPath = Join-Path (Get-Location) "backups" "standard_users.sql"
    
    Write-Host "Erstelle Backup der aktuellen Datenbank..." -ForegroundColor Yellow
    Write-Host "  Backup-Datei: $backupFile" -ForegroundColor Gray
    
    # Backup erstellen
    try {
        $dumpCommand = "docker exec ugh-db mysqldump --opt -uroot -p`"password`" db"
        Invoke-Expression $dumpCommand | Out-File -FilePath $backupPath -Encoding UTF8
        
        if ($LASTEXITCODE -eq 0) {
            $fileSize = (Get-Item $backupPath).Length / 1MB
            Write-Host "✓ Backup erfolgreich erstellt: $backupFile ($($fileSize.ToString('F2')) MB)" -ForegroundColor Green
        } else {
            throw "mysqldump fehlgeschlagen (Exit Code: $LASTEXITCODE)"
        }
    } catch {
        throw "Fehler beim Erstellen des Backups: $($_.Exception.Message)"
    }
    
    # Als Standard-Backup kopieren
    Write-Host "Kopiere als Standard-Startdatenbank..." -ForegroundColor Yellow
    try {
        Copy-Item -Path $backupPath -Destination $standardBackupPath -Force
        Write-Host "✓ Standard-Backup aktualisiert: standard_users.sql" -ForegroundColor Green
    } catch {
        throw "Fehler beim Kopieren des Standard-Backups: $($_.Exception.Message)"
    }
    
    return $backupPath
}

function Update-DockerConfig {
    Write-Host "Aktualisiere Docker-Konfiguration..." -ForegroundColor Yellow
    
    $composeFile = Join-Path (Get-Location) "compose.yaml"
    if (-not (Test-Path $composeFile)) {
        Write-Host "⚠ compose.yaml nicht gefunden, überspringe Konfigurations-Update" -ForegroundColor Yellow
        return
    }
    
    # Prüfe ob data.sql Verzeichnis existiert
    $dataSqlDir = Join-Path (Get-Location) "data.sql"
    if (-not (Test-Path $dataSqlDir)) {
        New-Item -ItemType Directory -Path $dataSqlDir -Force | Out-Null
        Write-Host "✓ data.sql Verzeichnis erstellt" -ForegroundColor Green
    }
    
    # Kopiere Standard-Backup als init.sql
    $initSqlPath = Join-Path $dataSqlDir "init.sql"
    $standardBackupPath = Join-Path (Get-Location) "backups" "standard_users.sql"
    
    try {
        Copy-Item -Path $standardBackupPath -Destination $initSqlPath -Force
        Write-Host "✓ Docker-Initialisierung aktualisiert: data.sql/init.sql" -ForegroundColor Green
    } catch {
        Write-Host "⚠ Fehler beim Aktualisieren der Docker-Initialisierung: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}

function Show-SuccessMessage {
    Write-Host ""
    Write-Host "🎉 Development Backup erfolgreich erstellt!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Nächste Schritte für andere Entwickler:" -ForegroundColor White
    Write-Host "  1. Projekt klonen" -ForegroundColor Gray
    Write-Host "  2. Docker Desktop starten" -ForegroundColor Gray
    Write-Host "  3. Ausführen: docker-compose up -d" -ForegroundColor Gray
    Write-Host "  4. Warten bis alle Services gestartet sind" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Die Datenbank wird automatisch mit dem aktuellen Stand initialisiert!" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Backup-Details:" -ForegroundColor White
    Write-Host "  - Standard-Backup: backups/standard_users.sql" -ForegroundColor Gray
    Write-Host "  - Docker-Init: data.sql/init.sql" -ForegroundColor Gray
    Write-Host "  - Erstellt: $(Get-Date -Format 'dd.MM.yyyy HH:mm:ss')" -ForegroundColor Gray
}

try {
    if ($Help) {
        Show-Help
        exit 0
    }
    
    # Prüfe ob Standard-Backup bereits existiert
    $standardBackupPath = Join-Path (Get-Location) "backups" "standard_users.sql"
    if ((Test-Path $standardBackupPath) -and (-not $Force)) {
        $response = Read-Host "Standard-Backup existiert bereits. Überschreiben? (j/N)"
        if ($response -ne "j" -and $response -ne "J" -and $response -ne "y" -and $response -ne "Y") {
            Write-Host "Abgebrochen." -ForegroundColor Yellow
            exit 0
        }
    }
    
    Test-Prerequisites
    $backupPath = Create-Backup -Name $BackupName
    Update-DockerConfig
    Show-SuccessMessage
    
} catch {
    Write-Host ("❌ Fehler: $($_.Exception.Message)") -ForegroundColor Red
    exit 1
} 
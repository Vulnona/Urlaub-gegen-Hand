# UGH Database Restore Tool
# PowerShell-Version des restoredb.sh Scripts


param(
    [Parameter(Mandatory=$true)]
    [string]$BackupFile,
    [Parameter(Mandatory=$false)]
    [string]$Container = "ugh-db",
    [Parameter(Mandatory=$false)]
    [string]$Database = "db",
    [Parameter(Mandatory=$false)]
    [string]$User = "root",
    [Parameter(Mandatory=$false)]
    [switch]$Force,
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Path $MyInvocation.MyCommand.Path -Parent
# Projektroot ist ein Verzeichnis über scripts/database
$projectRoot = Resolve-Path -Path (Join-Path $scriptRoot "..\..") | Select-Object -ExpandProperty Path
$composePath = Join-Path $projectRoot "compose.yaml"
# Node.js-Skript liegt im gleichen Ordner wie dieses Skript
$nodeScript = Join-Path $scriptRoot "get-mysql-creds.js"
$credsJson = node $nodeScript $composePath
if ($LASTEXITCODE -ne 0) {
    Write-Host "Fehler beim Auslesen der MySQL-Credentials aus compose.yaml." -ForegroundColor Red
    exit 1
}
$creds = $null
try { $creds = $credsJson | ConvertFrom-Json } catch { Write-Host "Fehler beim Parsen der Credentials: $_" -ForegroundColor Red; exit 1 }
$User = "root"
$Database = $creds.db
$passwordFile = Join-Path $projectRoot ($creds.pwFile -replace '/', [System.IO.Path]::DirectorySeparatorChar)
if (-not (Test-Path $passwordFile)) {
    Write-Host "Passwort-Datei nicht gefunden: $passwordFile" -ForegroundColor Yellow
    $password = "password"  # Fallback
} else {
    $password = Get-Content $passwordFile -Raw | ForEach-Object { $_.Trim() }
}

function Show-Help {
    Write-Host "UGH Database Restore Tool" -ForegroundColor Cyan
    Write-Host "=========================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\database-restore.ps1 -BackupFile backup.sql" -ForegroundColor Gray
    Write-Host "  .\database-restore.ps1 backup.sql -Force" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor White
    Write-Host "  -BackupFile      SQL backup file to restore" -ForegroundColor Gray
    Write-Host "  -Container       Container name (default: ugh-db)" -ForegroundColor Gray
    Write-Host "  -Database        Database name (default: db)" -ForegroundColor Gray
    Write-Host "  -User            Database user (default: root)" -ForegroundColor Gray
    Write-Host "  -Force           Skip confirmation prompts" -ForegroundColor Gray
    Write-Host ""
    Write-Host "WARNING: This will DROP and RECREATE the database!" -ForegroundColor Red
    Write-Host "            All existing data will be lost!" -ForegroundColor Red
}

function Test-Prerequisites {
    # Backup-Datei prüfen
    if (-not (Test-Path $BackupFile)) {
        throw "Backup-Datei nicht gefunden: $BackupFile"
    }
    
    $fileSize = (Get-Item $BackupFile).Length / 1MB
    Write-Host "Backup-Datei gefunden: $BackupFile ($($fileSize.ToString('F2')) MB)" -ForegroundColor Green
    
    # Container-Verfügbarkeit prüfen
    $containerStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String $Container
    if (-not $containerStatus) {
        throw "Container '$Container' nicht gefunden oder nicht aktiv"
    }
    
    Write-Host "Container '$Container' ist aktiv" -ForegroundColor Green
    
    # Debug-Ausgabe: Passwort und Docker-Befehl
    Write-Host "[DEBUG] Teste Verbindung mit: docker exec $Container mysql -u$User -p'$password' -e 'SELECT 1;'" -ForegroundColor Cyan
    Write-Host "[DEBUG] Verwendetes Passwort: $password" -ForegroundColor Yellow
    # Datenbankverbindung testen
    try {
        $cmd = 'docker exec {0} mysql -u{1} -p{2} -e "SELECT 1;"' -f $Container, $User, $password
        Write-Host "[DEBUG] Ausführung: cmd.exe /c $cmd" -ForegroundColor Magenta
        $result = & cmd.exe /c $cmd
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Datenbankverbindung erfolgreich" -ForegroundColor Green
        } else {
            throw "Datenbankverbindung fehlgeschlagen"
        }
    } catch {
        throw "Kann keine Verbindung zur Datenbank herstellen: $($_.Exception.Message)"
    }
}

function Invoke-DatabaseRestore {
    $startTime = Get-Date
    
    try {
        # 1. Backup-Datei in Container kopieren
        Write-Host "1. Kopiere Backup-Datei in Container..." -ForegroundColor Yellow
        Write-Host "[DEBUG] docker cp $BackupFile ${Container}:/backup.sql" -ForegroundColor Cyan
        $cmd = "docker cp `"$BackupFile`" ${Container}:/backup.sql"
        Write-Host "[DEBUG] Ausführung: cmd.exe /c $cmd" -ForegroundColor Magenta
        & cmd.exe /c $cmd
        if ($LASTEXITCODE -ne 0) {
            throw "Fehler beim Kopieren der Backup-Datei"
        }
        Write-Host "   Datei kopiert" -ForegroundColor Green
        
        # 2. Datenbank löschen
        Write-Host "2. Lösche bestehende Datenbank..." -ForegroundColor Yellow
        Write-Host "[DEBUG] docker exec $Container mysql -u$User -p'$password' -e 'DROP DATABASE IF EXISTS $Database;'" -ForegroundColor Cyan
        $cmd = 'docker exec {0} mysql -u{1} -p{2} -e "DROP DATABASE IF EXISTS {3};"' -f $Container, $User, $password, $Database
        Write-Host "[DEBUG] Ausführung: cmd.exe /c $cmd" -ForegroundColor Magenta
        & cmd.exe /c $cmd
        if ($LASTEXITCODE -ne 0) {
            throw "Fehler beim Löschen der Datenbank"
        }
        Write-Host "   Datenbank gelöscht" -ForegroundColor Green
        
        # 3. Neue Datenbank erstellen
        Write-Host "3. Erstelle neue Datenbank..." -ForegroundColor Yellow
        Write-Host "[DEBUG] docker exec $Container mysql -u$User -p'$password' -e 'CREATE DATABASE $Database;'" -ForegroundColor Cyan
        $cmd = 'docker exec {0} mysql -u{1} -p{2} -e "CREATE DATABASE {3};"' -f $Container, $User, $password, $Database
        Write-Host "[DEBUG] Ausführung: cmd.exe /c $cmd" -ForegroundColor Magenta
        & cmd.exe /c $cmd
        if ($LASTEXITCODE -ne 0) {
            throw "Fehler beim Erstellen der Datenbank"
        }
        Write-Host "   Datenbank erstellt" -ForegroundColor Green
        
        # 4. Backup einspielen 
        Write-Host "4. Spiele Backup ein..." -ForegroundColor Yellow
        $cmd = "type `"$BackupFile`" | docker exec -i $Container mysql -u$User -p$password -A $Database"
        Write-Host "[DEBUG] Restore-Befehl: cmd.exe /c $cmd" -ForegroundColor Magenta
        & cmd.exe /c $cmd
        if ($LASTEXITCODE -ne 0) {
            throw "Fehler beim Einspielen des Backups"
        }
        Write-Host "   Backup eingespielt" -ForegroundColor Green
        
        # 5. Aufräumen
        Write-Host "5. Räume temporäre Dateien auf..." -ForegroundColor Yellow
        Write-Host "[DEBUG] docker exec $Container rm -f /backup.sql" -ForegroundColor Cyan
        $cmd = "docker exec $Container rm -f /backup.sql"
        Write-Host "[DEBUG] Ausführung: cmd.exe /c $cmd" -ForegroundColor Magenta
        & cmd.exe /c $cmd
        Write-Host "   Aufräumung abgeschlossen" -ForegroundColor Green
        
        # Erfolgsmeldung
        $endTime = Get-Date
        $duration = $endTime - $startTime
        
        Write-Host ""
        Write-Host "  Datenbank-Restore erfolgreich abgeschlossen!" -ForegroundColor Green
        Write-Host "  Dauer: $($duration.TotalSeconds.ToString('F2')) Sekunden" -ForegroundColor Gray
        Write-Host "  Database: $Database" -ForegroundColor Gray
        Write-Host "  Container: $Container" -ForegroundColor Gray
        
    } catch {
        Write-Host ""
        Write-Host "  Fehler beim Restore-Vorgang: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "  Versuche Aufräumung..." -ForegroundColor Yellow
        
        # Cleanup versuchen
        try {
            docker exec $Container rm -f /backup.sql 2>$null
        } catch {
            Write-Host "  Warnung: Konnte temporäre Datei nicht entfernen" -ForegroundColor Yellow
        }
        
        throw
    }
}

function Confirm-RestoreOperation {
    if ($Force) {
        return $true
    }
    
    Write-Host ""
    Write-Host "WARNUNG: Datenbank-Restore" -ForegroundColor Red
    Write-Host "============================================" -ForegroundColor Red
    Write-Host "Diese Operation wird:" -ForegroundColor White
    Write-Host "  1. Die bestehende Datenbank '$Database' LÖSCHEN" -ForegroundColor Red
    Write-Host "  2. Eine neue leere Datenbank erstellen" -ForegroundColor Yellow
    Write-Host "  3. Das Backup '$BackupFile' einspielen" -ForegroundColor Green
    Write-Host ""
    Write-Host "ALLE AKTUELLEN DATEN GEHEN VERLOREN!" -ForegroundColor Red
    Write-Host ""
    
    $confirm = Read-Host "Möchten Sie fortfahren? Geben Sie 'YES' ein um zu bestätigen"
    
    return ($confirm -eq "YES")
}


# Main Logic
try {
    if ($Help) {
        Show-Help
        exit 0
    }
    # BackupFile ggf. relativen Pfad auflösen
    if (-not [System.IO.Path]::IsPathRooted($BackupFile)) {
        $BackupFile = Join-Path (Get-Location) $BackupFile
    }
    # Prerequisites prüfen
    Test-Prerequisites
    # Bestätigung einholen
    if (-not (Confirm-RestoreOperation)) {
        Write-Host "Restore-Operation abgebrochen" -ForegroundColor Yellow
        exit 0
    }
    # Restore durchführen
    Invoke-DatabaseRestore
} catch {
    Write-Host ("Fehler: $($_.Exception.Message)") -ForegroundColor Red
    exit 1
}

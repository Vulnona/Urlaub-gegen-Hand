
# UGH Database Dump Tool
# PowerShell-Version des dumpdb.sh Scripts

param(
    [Parameter(Mandatory=$false)]
    [string]$OutputFile,
    [Parameter(Mandatory=$false)]
    [string]$Container = "ugh-db",
    [Parameter(Mandatory=$false)]
    [string]$Database = "db",
    [Parameter(Mandatory=$false)]
    [string]$User = "root",
    [Parameter(Mandatory=$false)]
    [switch]$NoTablespaces,
    [Parameter(Mandatory=$false)]
    [switch]$SkipExtendedInsert,
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

function Show-Help {
    Write-Host "UGH Database Dump Tool" -ForegroundColor Cyan
    Write-Host "======================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\database-dump.ps1 -OutputFile backup.sql" -ForegroundColor Gray
    Write-Host "  .\database-dump.ps1 backup                # Fügt .sql automatisch an" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameter:" -ForegroundColor White
    Write-Host "  -OutputFile            Backup-Dateiname (default: auto-generiert)" -ForegroundColor Gray
    Write-Host "  -Container             Container-Name (default: ugh-db)" -ForegroundColor Gray
    Write-Host "  -Database              Datenbankname (default: db)" -ForegroundColor Gray
    Write-Host "  -User                  DB-User (default: root)" -ForegroundColor Gray
    Write-Host "  -NoTablespaces         Tablespace-Infos überspringen" -ForegroundColor Gray
    Write-Host "  -SkipExtendedInsert    Einzelne Inserts verwenden" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Beispiele:" -ForegroundColor White
    Write-Host "  .\database-dump.ps1 my-backup" -ForegroundColor Gray
    Write-Host "  .\database-dump.ps1 -OutputFile full-backup.sql -NoTablespaces" -ForegroundColor Gray
}

function Create-DatabaseDump {
    param([string]$BackupFile)
    $containerStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String $Container
    if (-not $containerStatus) {
        throw "Container '$Container' nicht gefunden oder nicht aktiv"
    } else {
        Write-Host "Container '$Container' ist aktiv" -ForegroundColor Green
    }
    $dumpArgs = @("mysqldump", "--opt")
    if ($NoTablespaces) {
        $dumpArgs += "--no-tablespaces"
    }
    if ($SkipExtendedInsert) {
        $dumpArgs += "--skip-extended-insert"
    }
    $dumpArgs += @("-u$User", "-p`"$password`"", $Database)
    Write-Host "Erstelle Datenbank-Backup..." -ForegroundColor Yellow
    Write-Host "  Container: $Container" -ForegroundColor Gray
    Write-Host "  Database:  $Database" -ForegroundColor Gray
    Write-Host "  Output:    $BackupFile" -ForegroundColor Gray
    Write-Host "  Args:      $($dumpArgs -join ' ')" -ForegroundColor Gray
    $startTime = Get-Date
    $dumpCommand = "docker exec $Container $($dumpArgs -join ' ')"
    Write-Host "Führe Backup aus..." -ForegroundColor Yellow
    Invoke-Expression "$dumpCommand" | Out-File -FilePath $BackupFile -Encoding UTF8
    if ($LASTEXITCODE -eq 0) {
        $endTime = Get-Date
        $duration = $endTime - $startTime
        $fileSize = (Get-Item $BackupFile).Length / 1MB
        Write-Host "Backup erfolgreich erstellt" -ForegroundColor Green
        Write-Host "  Datei:     $BackupFile" -ForegroundColor Gray
        Write-Host "  Größe:     $($fileSize.ToString('F2')) MB" -ForegroundColor Gray
        Write-Host "  Dauer:     $($duration.TotalSeconds.ToString('F2')) Sekunden" -ForegroundColor Gray
    } else {
        throw "mysqldump fehlgeschlagen (Exit Code: $LASTEXITCODE)"
    }
}

try {
    if ($Help) {
        Show-Help
        exit 0
    }
    if (-not $OutputFile) {
        $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
        $OutputFile = "ugh-db-backup_$timestamp.sql"
    } elseif (-not $OutputFile.EndsWith('.sql')) {
        $OutputFile += ".sql"
    }
    if (-not [System.IO.Path]::IsPathRooted($OutputFile)) {
        $OutputFile = Join-Path (Get-Location) $OutputFile
    }
    Create-DatabaseDump -BackupFile $OutputFile
} catch {
    Write-Host ("Fehler: $($_.Exception.Message)") -ForegroundColor Red
    exit 1
}

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

$ErrorActionPreference = "Stop"

# Passwort aus Secret-Datei lesen
$passwordFile = ".docker\db\secrets\.db-root-password.txt"
if (Test-Path $passwordFile) {
    $password = Get-Content $passwordFile -Raw | ForEach-Object { $_.Trim() }
} else {
    Write-Host "⚠ Passwort-Datei nicht gefunden: $passwordFile" -ForegroundColor Yellow
    $password = "password"  # Fallback
}

function Show-Help {
    Write-Host "UGH Database Dump Tool" -ForegroundColor Cyan
    Write-Host "======================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\database-dump.ps1 -OutputFile backup.sql" -ForegroundColor Gray
    Write-Host "  .\database-dump.ps1 backup                # Adds .sql automatically" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor White
    Write-Host "  -OutputFile            Backup file name (default: auto-generated)" -ForegroundColor Gray
    Write-Host "  -Container             Container name (default: ugh-db)" -ForegroundColor Gray
    Write-Host "  -Database              Database name (default: db)" -ForegroundColor Gray
    Write-Host "  -User                  Database user (default: root)" -ForegroundColor Gray
    Write-Host "  -NoTablespaces         Skip tablespace information" -ForegroundColor Gray
    Write-Host "  -SkipExtendedInsert    Use single-row inserts" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor White
    Write-Host "  .\database-dump.ps1 my-backup" -ForegroundColor Gray
    Write-Host "  .\database-dump.ps1 -OutputFile full-backup.sql -NoTablespaces" -ForegroundColor Gray
}

function Create-DatabaseDump {
    param([string]$BackupFile)
    
    # Container-Verfügbarkeit prüfen
    $containerStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String $Container
    if (-not $containerStatus) {
        throw "Container '$Container' nicht gefunden oder nicht aktiv"
    }
    
    Write-Host "✓ Container '$Container' ist aktiv" -ForegroundColor Green
    
    # mysqldump Parameter aufbauen
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
    
    try {
        # Backup erstellen
        $startTime = Get-Date
        $dumpCommand = "docker exec $Container $($dumpArgs -join ' ')"
        
        Write-Host "Führe Backup aus..." -ForegroundColor Yellow
        Invoke-Expression "$dumpCommand" | Out-File -FilePath $BackupFile -Encoding UTF8
        
        if ($LASTEXITCODE -eq 0) {
            $endTime = Get-Date
            $duration = $endTime - $startTime
            $fileSize = (Get-Item $BackupFile).Length / 1MB
            
            Write-Host "✓ Backup erfolgreich erstellt" -ForegroundColor Green
            Write-Host "  Datei:     $BackupFile" -ForegroundColor Gray
            Write-Host "  Größe:     $($fileSize.ToString('F2')) MB" -ForegroundColor Gray
            Write-Host "  Dauer:     $($duration.TotalSeconds.ToString('F2')) Sekunden" -ForegroundColor Gray
        } else {
            throw "mysqldump fehlgeschlagen (Exit Code: $LASTEXITCODE)"
        }
        
    } catch {
        Write-Host "Fehler beim Erstellen des Backups: $($_.Exception.Message)" -ForegroundColor Red
        if (Test-Path $BackupFile) {
            Remove-Item $BackupFile -Force
            Write-Host "Unvollständige Backup-Datei entfernt" -ForegroundColor Yellow
        }
        throw
    }
}

# Main Logic
try {
    if ($Help) {
        Show-Help
        exit 0
    }
    
    # Output-Datei bestimmen
    if (-not $OutputFile) {
        $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
        $OutputFile = "ugh-db-backup_$timestamp.sql"
    } elseif (-not $OutputFile.EndsWith('.sql')) {
        $OutputFile += ".sql"
    }
    
    # Absoluten Pfad erstellen
    if (-not [System.IO.Path]::IsPathRooted($OutputFile)) {
        $OutputFile = Join-Path (Get-Location) $OutputFile
    }
    
    Create-DatabaseDump -BackupFile $OutputFile
    
} catch {
    Write-Host "Fehler: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

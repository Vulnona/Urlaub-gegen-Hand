# PowerShell script to safely reset the ugh-db Docker database
# This script will create a backup before removing the container and volume

$containerName = "ugh-db"
$volumeName = "ugh_db-data"
$workspaceRoot = Resolve-Path (Join-Path $PSScriptRoot '../..')
$backupDir = Join-Path $workspaceRoot "backups\db-$(Get-Date -Format 'yyyyMMdd_HHmmss')"


# --- MySQL Credentials mit Node.js-Skript auslesen ---
$projectRoot = Resolve-Path (Join-Path $PSScriptRoot '../..')
$composePath = Join-Path $projectRoot "compose.yaml"
$nodeScript = Join-Path $PSScriptRoot "get-mysql-creds.js"
$credsJson = node $nodeScript $composePath
if ($LASTEXITCODE -ne 0) {
    Write-Error "Fehler beim Auslesen der MySQL-Credentials aus compose.yaml."
    exit 1
}
$creds = $null
try { $creds = $credsJson | ConvertFrom-Json } catch { Write-Error "Fehler beim Parsen der Credentials: $_"; exit 1 }
$dbUser = $creds.user
$dbName = $creds.db
$pwFile = $creds.pwFile

if ([System.IO.Path]::IsPathRooted($pwFile)) {
    $pwFilePath = $pwFile
} else {
    $pwFilePath = Join-Path $projectRoot $pwFile
}
if (-not (Test-Path $pwFilePath)) {
    Write-Error "MySQL-Passwortdatei nicht gefunden: $pwFilePath"
    exit 1
}
$dbPassword = Get-Content $pwFilePath -Raw | ForEach-Object { $_.Trim() }

# Ensure Docker is available
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Error "Docker is not installed or not in PATH."
    exit 1
}

# Create backup directory
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null


# Backup the database (using credentials from compose.yaml)
Write-Host "Creating database backup..."
docker exec $containerName mysqldump --no-tablespaces -u $dbUser --password=$dbPassword $dbName | Out-File -Encoding utf8 "$backupDir\$dbName.sql"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Backup failed. Aborting reset."
    exit 1
}

# Stop and remove the container
Write-Host "Stopping and removing container $containerName..."
docker stop $containerName
if ($?) { docker rm $containerName }

# Remove the volume
Write-Host "Removing volume $volumeName..."
docker volume rm $volumeName

Write-Host "Database reset complete. Backup saved to $backupDir."

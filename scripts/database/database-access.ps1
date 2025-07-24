# UGH Database Access Tool
# PowerShell-Version des accessdb.sh Scripts

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("u", "m", "o", "users", "memberships", "open")]
    [string]$Action,
    
    [Parameter(Mandatory=$false)]
    [string]$Container = "ugh-db",
    
    [Parameter(Mandatory=$false)]
    [string]$Database = "db",
    
    [Parameter(Mandatory=$false)]
    [string]$User = "root",
    
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

$ErrorActionPreference = "Stop"

# MySQL Credentials mit Node.js-Skript auslesen
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
$User = $creds.user
$Database = $creds.db
$passwordFile = Join-Path $projectRoot ($creds.pwFile -replace '/', [System.IO.Path]::DirectorySeparatorChar)
if (-not (Test-Path $passwordFile)) {
    Write-Host "Passwort-Datei nicht gefunden: $passwordFile" -ForegroundColor Yellow
    $password = "password"  # Fallback
} else {
    $password = Get-Content $passwordFile -Raw | ForEach-Object { $_.Trim() }
}

function Show-Help {
    Write-Host "UGH Database Access Tool" -ForegroundColor Cyan
    Write-Host "========================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\database-access.ps1 -Action u     # List users" -ForegroundColor Gray
    Write-Host "  .\database-access.ps1 -Action m     # List memberships" -ForegroundColor Gray
    Write-Host "  .\database-access.ps1 -Action o     # Open database" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Aliases:" -ForegroundColor White
    Write-Host "  u, users       -> List Users table" -ForegroundColor Gray
    Write-Host "  m, memberships -> List Memberships table" -ForegroundColor Gray
    Write-Host "  o, open        -> Open interactive MySQL shell" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor White
    Write-Host "  -Container     Default: ugh-db" -ForegroundColor Gray
    Write-Host "  -Database      Default: db" -ForegroundColor Gray
    Write-Host "  -User          Default: root" -ForegroundColor Gray
}

function Invoke-DatabaseQuery {
    param([string]$Query)
    
    try {
        $result = docker exec -i $Container mysql -u$User -p"$password" -D $Database -e $Query 2>$null
        if ($LASTEXITCODE -eq 0) {
            return $result
        } else {
            throw "Database query failed"
        }
    } catch {
        Write-Host "Fehler beim Ausführen der Datenbankabfrage: $($_.Exception.Message)" -ForegroundColor Red
        Write-Host "Container: $Container, User: $User, Database: $Database" -ForegroundColor Yellow
        exit 1
    }
}

function Open-DatabaseShell {
    Write-Host "Öffne interaktive MySQL-Shell..." -ForegroundColor Yellow
    Write-Host "Container: $Container, Database: $Database" -ForegroundColor Gray
    Write-Host "Beenden mit: exit" -ForegroundColor Gray
    Write-Host ""
    
    try {
        docker exec -it $Container mysql -u$User -p"$password" -D $Database
    } catch {
        Write-Host "Fehler beim Öffnen der Datenbankverbindung: $($_.Exception.Message)" -ForegroundColor Red
        exit 1
    }
}

function Get-Users {
    Write-Host "Users Tabelle:" -ForegroundColor Cyan
    Write-Host "===============" -ForegroundColor Cyan
    
    $query = "SELECT Id, Username, Email, IsAdmin, TwoFactorEnabled, CreatedAt FROM Users ORDER BY CreatedAt DESC LIMIT 20;"
    $result = Invoke-DatabaseQuery -Query $query
    
    if ($result) {
        $result | ForEach-Object { Write-Host $_ -ForegroundColor White }
    } else {
        Write-Host "Keine Benutzer gefunden oder Tabelle existiert nicht" -ForegroundColor Yellow
    }
}

function Get-Memberships {
    Write-Host "Memberships Tabelle:" -ForegroundColor Cyan
    Write-Host "====================" -ForegroundColor Cyan
    
    $query = "SELECT Id, UserId, MembershipType, Status, StartDate, EndDate FROM Memberships ORDER BY StartDate DESC LIMIT 20;"
    $result = Invoke-DatabaseQuery -Query $query
    
    if ($result) {
        $result | ForEach-Object { Write-Host $_ -ForegroundColor White }
    } else {
        Write-Host "Keine Mitgliedschaften gefunden oder Tabelle existiert nicht" -ForegroundColor Yellow
    }
}

# Main Logic



if ($Help -or -not $Action) {
    Show-Help
    exit 0
}

# Container-Verfügbarkeit prüfen
$containerStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String $Container
if (-not $containerStatus) {
    Write-Host "Container '$Container' nicht gefunden oder nicht aktiv" -ForegroundColor Red
    Write-Host "Aktive Container:" -ForegroundColor Yellow
    docker ps --format "table {{.Names}}\t{{.Status}}"
    exit 1
}

Write-Host "Container '$Container' ist aktiv" -ForegroundColor Green

$actionLower = $Action.ToLower()
if ($actionLower -eq "u" -or $actionLower -eq "users") {
    Get-Users
} elseif ($actionLower -eq "m" -or $actionLower -eq "memberships") {
    Get-Memberships
} elseif ($actionLower -eq "o" -or $actionLower -eq "open") {
    Open-DatabaseShell
} else {
    Write-Host "Unbekannte Aktion: $Action" -ForegroundColor Red
    Show-Help
    exit 1
}

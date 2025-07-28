# UGH Quick Start für Entwickler
# Startet das Projekt mit automatischer Datenbank-Initialisierung

param(
    [Parameter(Mandatory=$false)]
    [switch]$Reset,
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

$ErrorActionPreference = "Stop"

function Show-Help {
    Write-Host "UGH Quick Start für Entwickler" -ForegroundColor Cyan
    Write-Host "==============================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\quick-start.ps1" -ForegroundColor Gray
    Write-Host "  .\quick-start.ps1 -Reset" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor White
    Write-Host "  -Reset           Startet mit frischer Datenbank (löscht alle Daten)" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Dieses Skript:" -ForegroundColor White
    Write-Host "  1. Prüft Voraussetzungen (Docker, etc.)" -ForegroundColor Gray
    Write-Host "  2. Startet alle Services mit docker-compose" -ForegroundColor Gray
    Write-Host "  3. Wartet bis alle Services bereit sind" -ForegroundColor Gray
    Write-Host "  4. Zeigt Zugangsdaten und URLs" -ForegroundColor Gray
}

function Test-Prerequisites {
    Write-Host "Prüfe Voraussetzungen..." -ForegroundColor Yellow
    
    # Docker prüfen
    try {
        $dockerVersion = docker --version
        Write-Host "✓ Docker gefunden: $dockerVersion" -ForegroundColor Green
    } catch {
        throw "Docker nicht gefunden. Bitte installieren Sie Docker Desktop und starten Sie es."
    }
    
    # Docker Compose prüfen
    try {
        $composeVersion = docker-compose --version
        Write-Host "✓ Docker Compose gefunden: $composeVersion" -ForegroundColor Green
    } catch {
        throw "Docker Compose nicht gefunden. Bitte installieren Sie Docker Compose."
    }
    
    # compose.yaml prüfen
    $composeFile = Join-Path (Get-Location) "compose.yaml"
    if (-not (Test-Path $composeFile)) {
        throw "compose.yaml nicht gefunden. Bitte führen Sie dieses Skript im Projektverzeichnis aus."
    }
    
    Write-Host "✓ compose.yaml gefunden" -ForegroundColor Green
}

function Start-Services {
    param([bool]$ResetDatabase)
    
    Write-Host "Starte Services..." -ForegroundColor Yellow
    
    if ($ResetDatabase) {
        Write-Host "⚠ Reset-Modus: Lösche alle Container und Volumes..." -ForegroundColor Yellow
        docker-compose down -v
        if ($LASTEXITCODE -ne 0) {
            Write-Host "⚠ Warnung: Fehler beim Stoppen der Services" -ForegroundColor Yellow
        }
    } else {
        # Stoppe nur Services, behalte Volumes
        docker-compose down
        if ($LASTEXITCODE -ne 0) {
            Write-Host "⚠ Warnung: Fehler beim Stoppen der Services" -ForegroundColor Yellow
        }
    }
    
    # Starte Services im Hintergrund
    Write-Host "Starte Services im Hintergrund..." -ForegroundColor Yellow
    docker-compose up -d
    
    if ($LASTEXITCODE -ne 0) {
        throw "Fehler beim Starten der Services. Prüfen Sie die Docker-Logs."
    }
    
    Write-Host "✓ Services gestartet" -ForegroundColor Green
}

function Wait-ForServices {
    Write-Host "Warte auf Services..." -ForegroundColor Yellow

    $maxAttempts = 30
    $attempt = 0

    while ($attempt -lt $maxAttempts) {
        $attempt++
        Write-Host "  Prüfe Services... (Versuch $attempt/$maxAttempts)" -ForegroundColor Gray

        # Prüfe Datenbank
        $dbStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String "ugh-db"
        if ($dbStatus -and $dbStatus -match "healthy") {
            Write-Host "✓ Datenbank ist bereit" -ForegroundColor Green
            break
        }

        # Prüfe Backend
        $backendStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String "ugh-backend"
        if ($backendStatus -and $backendStatus -match "Up") {
            Write-Host "✓ Backend ist bereit" -ForegroundColor Green
        }

        # Prüfe Frontend
        $frontendStatus = docker ps --format "table {{.Names}}\t{{.Status}}" | Select-String "ugh-frontend"
        if ($frontendStatus -and $frontendStatus -match "Up") {
            Write-Host "✓ Frontend ist bereit" -ForegroundColor Green
        }

        if ($attempt -eq $maxAttempts) {
            Write-Host "⚠ Timeout beim Warten auf Services" -ForegroundColor Yellow
            break
        }

        Start-Sleep -Seconds 2
    }
}

function Show-AccessInfo {
    Write-Host ""
    Write-Host "🎉 UGH Projekt erfolgreich gestartet!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Zugangsdaten:" -ForegroundColor White
    Write-Host "  Frontend:     http://localhost:3002" -ForegroundColor Cyan
    Write-Host "  Backend API:  http://localhost:8081" -ForegroundColor Cyan
    Write-Host "  Nginx:        http://localhost:81" -ForegroundColor Cyan
    Write-Host "  Datenbank:    localhost:3306" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Test-Accounts:" -ForegroundColor White
    Write-Host "  Admin:        adminuser@example.com / password" -ForegroundColor Gray
    Write-Host "  User:         test@example.com / password" -ForegroundColor Gray
    Write-Host "  User 2:       test1@example.com / password" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Nützliche Befehle:" -ForegroundColor White
    Write-Host "  Logs anzeigen:     docker-compose logs -f" -ForegroundColor Gray
    Write-Host "  Services stoppen:  docker-compose down" -ForegroundColor Gray
    Write-Host "  Reset Datenbank:   .\quick-start.ps1 -Reset" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Status der Services:" -ForegroundColor White
    docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
}

function Show-ErrorInfo {
    Write-Host ""
    Write-Host "❌ Fehler beim Starten des Projekts" -ForegroundColor Red
    Write-Host ""
    Write-Host "Häufige Lösungen:" -ForegroundColor White
    Write-Host "  1. Docker Desktop starten" -ForegroundColor Gray
    Write-Host "  2. Ports prüfen (81, 3002, 8081, 3306)" -ForegroundColor Gray
    Write-Host "  3. Docker-Logs prüfen: docker-compose logs" -ForegroundColor Gray
    Write-Host "  4. Mit Reset versuchen: .\quick-start.ps1 -Reset" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Docker-Logs:" -ForegroundColor White
    docker-compose logs --tail=20
}

try {
    if ($Help) {
        Show-Help
        exit 0
    }
    
    Write-Host "UgH Quick Start" -ForegroundColor Cyan
    Write-Host "==================" -ForegroundColor Cyan
    Write-Host ""
    
    Test-Prerequisites
    Start-Services -ResetDatabase $Reset
    Wait-ForServices
    Show-AccessInfo
    
} catch {
    Write-Host ("❌ Fehler: $($_.Exception.Message)") -ForegroundColor Red
    Show-ErrorInfo
    exit 1
} 
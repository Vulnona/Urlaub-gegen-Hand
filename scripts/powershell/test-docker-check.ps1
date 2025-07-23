# Test script to demonstrate Docker checking functionality
param(
    [switch]$SimulateNoDocker = $false
)

function Test-DockerRunning {
    if ($SimulateNoDocker) {
        return $false
    }
    try {
        $null = docker version 2>$null
        return $LASTEXITCODE -eq 0
    } catch {
        return $false
    }
}

function Test-Platform {
    if ($IsLinux -or $IsMacOS) {
        return "Unix"
    } else {
        return "Windows"
    }
}

function Test-DockerAndPlatform {
    $platform = Test-Platform
    
    if (-not (Test-DockerRunning)) {
        Write-Host "❌ Docker ist nicht verfuegbar!" -ForegroundColor Red
        Write-Host ""
        
        if ($platform -eq "Windows") {
            Write-Host "Windows: Bitte Docker Desktop starten:" -ForegroundColor Yellow
            Write-Host "  - Docker Desktop öffnen" -ForegroundColor White
            Write-Host "  - Warten bis Docker läuft (Whale-Icon im System Tray)" -ForegroundColor White
            Write-Host "  - Dann das Skript erneut ausführen" -ForegroundColor White
        } else {
            Write-Host "Linux/macOS: Bitte Docker-Service starten:" -ForegroundColor Yellow
            Write-Host "  sudo systemctl start docker    # Linux" -ForegroundColor White
            Write-Host "  brew services start docker     # macOS mit Homebrew" -ForegroundColor White
            Write-Host "  oder Docker Desktop starten" -ForegroundColor White
        }
        
        Write-Host ""
        Write-Host "Testen Sie Docker mit: docker version" -ForegroundColor Cyan
        return $false
    }
    
    Write-Host "✅ Docker ist verfuegbar!" -ForegroundColor Green
    return $true
}

Write-Host "=== Test der Docker-Check-Funktionalität ===" -ForegroundColor Cyan
Write-Host ""

if ($SimulateNoDocker) {
    Write-Host "Simuliere: Docker nicht verfügbar" -ForegroundColor Yellow
    Write-Host ""
}

if (-not (Test-DockerAndPlatform)) {
    Write-Host ""
    Write-Host "Script would exit here with code 1" -ForegroundColor Red
} else {
    Write-Host "Script would continue normally" -ForegroundColor Green
}

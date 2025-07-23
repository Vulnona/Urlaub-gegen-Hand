# Secure Admin Setup Script
# This script provides a secure way to reset admin password in emergencies

param(
    [Parameter(Mandatory=$false)]
    [string]$NewPassword,
    
    [switch]$Help = $false
)

function Test-DockerRunning {
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
    
    return $true
}

# Show help if requested or no password provided
if ($Help -or [string]::IsNullOrEmpty($NewPassword)) {
    Write-Host "SECURE ADMIN SETUP SCRIPT - Help" -ForegroundColor Green
    Write-Host "=================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "BESCHREIBUNG:" -ForegroundColor Yellow
    Write-Host "  Sicherer Reset des Admin-Passworts fuer Notfaelle"
    Write-Host ""
    Write-Host "USAGE:" -ForegroundColor Yellow
    if (Test-Platform -eq "Windows") {
        Write-Host "  .\secure-admin-setup.ps1 -NewPassword '<password>' [-Help]"
    } else {
        Write-Host "  pwsh ./secure-admin-setup.ps1 -NewPassword '<password>' [-Help]"
    }
    Write-Host ""
    Write-Host "PARAMETER:" -ForegroundColor Yellow
    Write-Host "  -NewPassword : Das neue Admin-Passwort (Pflichtfeld)"
    Write-Host "  -Help        : Zeigt diese Hilfe an"
    Write-Host ""
    Write-Host "BEISPIELE:" -ForegroundColor Yellow
    if (Test-Platform -eq "Windows") {
        Write-Host "  .\secure-admin-setup.ps1 -NewPassword 'MeinNeuesPasswort123!'"
    } else {
        Write-Host "  pwsh ./secure-admin-setup.ps1 -NewPassword 'MeinNeuesPasswort123!'"
    }
    Write-Host ""
    exit 0
}

# Check Docker availability before proceeding
if (-not (Test-DockerAndPlatform)) {
    exit 1
}

# Generate a secure reset token
$ResetToken = [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes([System.Guid]::NewGuid().ToString()))

Write-Host "Secure Admin Password Reset" -ForegroundColor Yellow
Write-Host "================================" -ForegroundColor Yellow
Write-Host ""

# Write token to file inside container (cross-platform compatible)
Write-Host "Setting reset token in container..." -ForegroundColor Green
$TokenResult = docker exec ugh-backend sh -c "echo '$ResetToken' > /tmp/admin_reset_token"

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Failed to set token in container. Is the container running?" -ForegroundColor Red
    Write-Host "Try: docker-compose up -d" -ForegroundColor Yellow
    exit 1
}

# Prepare the request
$RequestBody = @{
    ResetToken = $ResetToken
    NewPassword = $NewPassword
} | ConvertTo-Json

try {
    # Make the reset request to the fixed backend URL
    $ApiUrl = "http://localhost:8080"
    $Response = Invoke-RestMethod -Uri "$ApiUrl/api/admin-setup/emergency-reset" -Method POST -ContentType "application/json" -Body $RequestBody
    
    Write-Host "✅ Admin password reset successful!" -ForegroundColor Green
    Write-Host "Email: admin@gmail.com" -ForegroundColor Cyan
    Write-Host "Password: ******** (masked for security)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "⚠️  The password was set as provided, but is not displayed for security reasons." -ForegroundColor Red
    Write-Host "⚠️  Please change this password immediately after login!" -ForegroundColor Red
    Write-Host "⚠️  Warning: Avoid displaying credentials in logs or console output to prevent unauthorized access." -ForegroundColor Yellow
}
catch {
    Write-Host "❌ Reset failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Make sure the containers are running and try again." -ForegroundColor Yellow
}

# Clean up - remove the token from container
Write-Host ""
Write-Host "Cleaning up reset token..." -ForegroundColor Yellow
docker exec ugh-backend sh -c "rm -f /tmp/admin_reset_token" 2>$null

Write-Host "Setup complete!" -ForegroundColor Green

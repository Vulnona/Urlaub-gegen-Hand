# Secure Admin Setup Script
# This script provides a secure way to reset admin password in emergencies

param(
    [Parameter(Mandatory=$true)]
    [string]$NewPassword,
    
    [Parameter(Mandatory=$false)]
    [string]$ApiUrl = "http://localhost:8080"
)

# Generate a secure reset token
$ResetToken = [System.Convert]::ToBase64String([System.Text.Encoding]::UTF8.GetBytes([System.Guid]::NewGuid().ToString()))

Write-Host "Secure Admin Password Reset" -ForegroundColor Yellow
Write-Host "================================" -ForegroundColor Yellow
Write-Host ""

# Set environment variable for the container
Write-Host "Setting reset token in container..." -ForegroundColor Green
docker exec -e ADMIN_RESET_TOKEN="$ResetToken" ugh-backend-1 echo "Token set"

# Prepare the request
$RequestBody = @{
    ResetToken = $ResetToken
    NewPassword = $NewPassword
} | ConvertTo-Json

try {
    # Make the reset request
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

# Clean up - remove the token from environment
Write-Host ""
Write-Host "Cleaning up reset token..." -ForegroundColor Yellow
docker exec ugh-backend-1 env -u ADMIN_RESET_TOKEN 2>$null

Write-Host "Setup complete!" -ForegroundColor Green

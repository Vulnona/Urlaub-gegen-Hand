# Backend Development Setup Script
# Ensures EF Core Tools are available in the backend container

Write-Host "🔧 Setting up backend development environment..." -ForegroundColor Cyan

# Check if container is running
$backendRunning = docker ps --filter "name=ugh-backend" --filter "status=running" -q
if (-not $backendRunning) {
    Write-Host "❌ Backend container not running. Start with: docker-compose up backend" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Backend container is running" -ForegroundColor Green

# Install EF Core Tools if not available
try {
    docker exec ugh-backend dotnet ef --version 2>$null | Out-Null
    Write-Host "✅ EF Core Tools already available" -ForegroundColor Green
}
catch {
    Write-Host "📦 Installing EF Core Tools..." -ForegroundColor Yellow
    docker exec ugh-backend dotnet tool install --global dotnet-ef
    docker exec ugh-backend sh -c 'echo "export PATH=$PATH:/root/.dotnet/tools" >> ~/.bashrc'
    Write-Host "✅ EF Core Tools installed" -ForegroundColor Green
}

# Verify setup
Write-Host "🔍 Verifying setup..." -ForegroundColor Yellow
docker exec ugh-backend dotnet ef --version
Write-Host "✅ Backend development environment ready!" -ForegroundColor Green

Write-Host ""
Write-Host "You can now use EF commands like:" -ForegroundColor Cyan
Write-Host "  docker exec ugh-backend dotnet ef migrations add <MigrationName>" -ForegroundColor Gray
Write-Host "  docker exec ugh-backend dotnet ef database update" -ForegroundColor Gray
Write-Host "  docker exec ugh-backend dotnet ef migrations list" -ForegroundColor Gray

#!/bin/bash

# Backend Development Setup Script
# Ensures EF Core Tools are available in the backend container

echo "🔧 Setting up backend development environment..."

# Check if container is running
if ! docker ps --filter "name=ugh-backend" --filter "status=running" -q | grep -q .; then
    echo "❌ Backend container not running. Start with: docker-compose up backend"
    exit 1
fi

echo "✅ Backend container is running"

# Install EF Core Tools if not available
if ! docker exec ugh-backend dotnet ef --version >/dev/null 2>&1; then
    echo "📦 Installing EF Core Tools..."
    docker exec ugh-backend dotnet tool install --global dotnet-ef
    docker exec ugh-backend sh -c 'echo "export PATH=\$PATH:/root/.dotnet/tools" >> ~/.bashrc'
    echo "✅ EF Core Tools installed"
else
    echo "✅ EF Core Tools already available"
fi

# Verify setup
echo "🔍 Verifying setup..."
docker exec ugh-backend dotnet ef --version
echo "✅ Backend development environment ready!"

echo ""
echo "You can now use EF commands like:"
echo "  docker exec ugh-backend dotnet ef migrations add <MigrationName>"
echo "  docker exec ugh-backend dotnet ef database update"
echo "  docker exec ugh-backend dotnet ef migrations list"

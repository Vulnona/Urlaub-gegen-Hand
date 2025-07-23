# Development Guide

Entwicklerleitfaden für das UGH-Projekt mit allen wichtigen Informationen für Setup, Entwicklung und Deployment.

## 🚀 Quick Start

### Ersteinrichtung
```powershell
# 1. Repository klonen
git clone https://github.com/Urlaub-gegen-Hand/UGH.git
cd UGH

# 2. Container starten (vollautomatische Migration & 2FA-Setup)
docker-compose up -d

# 3. Backend-Entwicklungsumgebung einrichten (einmalig)
.\scripts\setup-backend-dev.ps1
```

### Tägliche Entwicklung
```powershell
# System starten
docker-compose up -d

# Migration-Status prüfen
.\scripts\migration\enhanced-migration.ps1 status

# Entwicklung starten (Frontend Hot-Reload)
cd Frontend-Vuetify
npm run dev
```

## 📁 Projektstruktur

```
UGH/
├── Frontend-Vuetify/          # Vue.js Frontend (Vuetify UI)
├── Backend/                   # .NET 7.0 Web API
│   ├── Controllers/           # API Controllers
│   ├── Models/               # Entity Models
│   ├── Migrations/           # EF Core Migrations
│   └── Services/             # Business Logic
├── scripts/                  # PowerShell Automation Tools
│   ├── migration/            # Migration Management
│   ├── infrastructure/       # Port & Service Management
│   └── database/            # Database Tools
├── Docs/                     # Projektdokumentation
└── docker-compose.yaml      # Container-Orchestrierung
```

## 🛠️ Entwicklungstools

### Migration Management
```powershell
# Neue Migration erstellen
.\scripts\migration\enhanced-migration.ps1 add -MigrationName "AddNewFeature"

# Migration anwenden
.\scripts\migration\enhanced-migration.ps1 sync

# Migration zurückrollen
.\scripts\migration\enhanced-migration.ps1 remove -Force
```

### Database Tools
```powershell
# Datenbank-Shell öffnen
.\scripts\database\database-access.ps1

# Backup erstellen
.\scripts\database\database-dump.ps1

# Backup wiederherstellen
.\scripts\database\database-restore.ps1 -BackupFile "backup_2025-07-23.sql"
```

### Infrastructure Management
```powershell
# Port-Status prüfen
.\scripts\infrastructure\port-management.ps1 status

# Services neustarten
.\scripts\infrastructure\port-management.ps1 restart
```

## 🐳 Docker Environment

### Services

| Service | Container | Port | URL |
|---------|-----------|------|-----|
| Frontend | `ugh-frontend` | 3002 | http://localhost:3002 |
| Backend | `ugh-backend` | 8081 | http://localhost:8081 |
| Nginx | `ugh-webserver` | 81 | http://localhost:81 |
| MySQL | `ugh-db` | 3306 | localhost:3306 |

### Container-Management
```powershell
# Alle Services starten
docker-compose up -d

# Services anzeigen
docker-compose ps

# Logs anzeigen
docker-compose logs -f [service-name]

# Services stoppen
docker-compose down
```

## 🔧 Konfiguration

### Environment Variables
```yaml
# Backend/.env
DATABASE_URL=Server=ugh-db;Database=db;User=root;Password=${DB_PASSWORD};
JWT_SECRET=${JWT_SECRET}
```

### Port-Konfiguration
Zentrale Port-Verwaltung in `scripts/infrastructure/ports.config`:
```
FRONTEND_PORT=3002
BACKEND_PORT=8081
NGINX_PORT=81
MYSQL_PORT=3306
```

## 🧪 Testing

### Backend Tests
```powershell
# Unit Tests ausführen
cd Backend
dotnet test

# Mit Coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Frontend Tests
```powershell
# Unit Tests
cd Frontend-Vuetify
npm run test:unit

# E2E Tests
npm run test:e2e
```

## 📦 Deployment

### Production Build
```powershell
# Backend Build
cd Backend
dotnet publish -c Release

# Frontend Build
cd Frontend-Vuetify
npm run build
```

### Docker Production
```powershell
# Production Container erstellen
docker-compose -f docker-compose.prod.yml up -d
```

## 🔐 Security

### Admin Setup
```powershell
# Sicheres Admin-Setup
.\secure-admin-setup.ps1

# Admin-Passwort zurücksetzen
.\scripts\database\database-access.ps1
# Dann: SOURCE reset-admin-password.sql
```

### 2FA Configuration
Automatisch konfiguriert beim ersten Container-Start. Details siehe [Admin Security Dokumentation](../ADMIN-SECURITY.md).

## 🐛 Debugging

### Backend Debugging
```powershell
# Development mit Debug-Modus
cd Backend
dotnet watch run --environment Development
```

### Database Debugging
```powershell
# EF Migrations Debug
dotnet ef migrations list --verbose

# SQL Query Logging aktivieren (appsettings.Development.json)
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### Container Debugging
```powershell
# Container-Shell öffnen
docker exec -it ugh-backend bash
docker exec -it ugh-db mysql -u root -p

# Container-Logs
docker logs ugh-backend --follow
```

## 📋 Common Tasks

### Neue API Endpoint hinzufügen
1. Controller in `Backend/Controllers/` erstellen
2. Model in `Backend/Models/` definieren
3. Service in `Backend/Services/` implementieren
4. Migration erstellen falls DB-Änderungen nötig
5. Frontend-Integration in Vue.js

### Neue Database Entity
```powershell
# 1. Model in Backend/Models/ erstellen
# 2. DbContext aktualisieren
# 3. Migration generieren
.\scripts\migration\enhanced-migration.ps1 add -MigrationName "AddNewEntity"

# 4. Migration anwenden
.\scripts\migration\enhanced-migration.ps1 sync
```

### Frontend Component hinzufügen
```bash
# Vue Component erstellen
cd Frontend-Vuetify/src/components
# Component-Datei erstellen
# In Parent-Component importieren und registrieren
```

## 🆘 Troubleshooting

### Häufige Probleme

**Migration-Konflikte**
```powershell
# Orphan-Migrationen bereinigen
.\scripts\migration\enhanced-migration.ps1 orphans -Force

# Migration-Status prüfen
.\scripts\migration\enhanced-migration.ps1 status
```

**Port-Konflikte**
```powershell
# Port-Belegung prüfen
.\scripts\infrastructure\port-management.ps1 check

# Ports in ports.config ändern
# Services neustarten
docker-compose down && docker-compose up -d
```

**Container-Probleme**
```powershell
# Container vollständig zurücksetzen
docker-compose down -v
docker-compose up -d

# Images neu erstellen
docker-compose build --no-cache
```

## 📚 API Dokumentation

Die API-Dokumentation ist über Swagger UI verfügbar:
- **Development**: http://localhost:8081/swagger
- **Production**: http://your-domain/api/swagger

## 🔗 Useful Links

- [.NET 7.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Vue.js 3 Guide](https://vuejs.org/guide/)
- [Vuetify 3 Documentation](https://vuetifyjs.com/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [Docker Compose Reference](https://docs.docker.com/compose/)

---

**Letzte Aktualisierung**: 2025-07-23

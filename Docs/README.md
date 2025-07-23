# Dokumentations-Index

Zentrale Übersicht aller Dokumentationen für das UGH-Projekt.

## 📚 Hauptdokumentation

### 🚀 Getting Started
- **[README](../README.md)** - Projektübersicht und Schnellstart
- **[Development Guide](DEVELOPMENT.md)** - Vollständiger Entwicklerleitfaden
- **[Scripts & Automation](SCRIPTS.md)** - Alle PowerShell-Tools und Automatisierung

### 🔐 Security & Admin
- **[Admin Security](../ADMIN-SECURITY.md)** - Admin-Setup und Sicherheit
- **[Migration System](MIGRATION-SYSTEM.md)** - Erweiterte Migration-Verwaltung

## 🛠️ Technische Dokumentation

### Backend (.NET 7.0)
- **Controllers**: API-Endpunkte und Request-Handling
- **Models**: Entity-Definitionen und Datenmodelle
- **Services**: Business Logic und Service Layer
- **Migrations**: Entity Framework Datenbankmigrationen

### Frontend (Vue.js + Vuetify)
- **Components**: Wiederverwendbare UI-Komponenten
- **Views**: Seiten-Templates und Routing
- **Stores**: Pinia State Management
- **Assets**: Statische Ressourcen und Styling

### Infrastructure
- **Docker**: Container-Orchestrierung und Deployment
- **Nginx**: Reverse Proxy und Load Balancing
- **MySQL**: Datenbank-Schema und Konfiguration

## 🔧 Tools & Scripts

### PowerShell-Tools (`scripts/`)
- **[Migration Tools](../scripts/migration/)** - Enhanced Migration System
- **[Infrastructure Tools](../scripts/infrastructure/)** - Port & Service Management  
- **[Database Tools](../scripts/database/)** - Backup, Restore & Access

### Automation Features
- ✅ **Cross-Platform**: Windows, Linux, macOS
- ✅ **Auto-Documentation**: Selbst-aktualisierende Dokumentation
- ✅ **Docker Integration**: Nahtlose Container-Verwaltung
- ✅ **Error Handling**: Robuste Fehlerbehandlung
- ✅ **Dry-Run Support**: Sichere Vorschau-Modi

## 📋 Quick Reference

### Häufige Kommandos
```powershell
# Projekt starten
docker-compose up -d

# Migration-Status
.\scripts\migration\enhanced-migration.ps1 status

# Neue Migration
.\scripts\migration\enhanced-migration.ps1 add -MigrationName "FeatureName"

# System validieren
.\scripts\migration\enhanced-migration.ps1 validate

# Port-Status
.\scripts\infrastructure\port-management.ps1 status

# Datenbank-Zugriff
.\scripts\database\database-access.ps1
```

### Service URLs (Default)
- **Frontend**: http://localhost:3002
- **Backend API**: http://localhost:8081
- **Nginx Proxy**: http://localhost:81
- **API Docs**: http://localhost:8081/swagger

## 🏗️ Projektstruktur

```
UGH/
├── 📁 Frontend-Vuetify/       # Vue.js Frontend
├── 📁 Backend/               # .NET Web API
├── 📁 scripts/               # PowerShell Tools
│   ├── 📁 migration/         # Migration Management
│   ├── 📁 infrastructure/    # Service Management
│   └── 📁 database/         # Database Tools
├── 📁 Docs/                 # Diese Dokumentation
├── 📁 .docker/              # Docker-Konfiguration
├── 📄 docker-compose.yaml   # Container-Orchestrierung
├── 📄 README.md             # Projekt-Hauptdokumentation
└── 📄 ADMIN-SECURITY.md     # Admin & Sicherheit
```

## 🆘 Support & Troubleshooting

### Bei Problemen
1. **System validieren**: `.\scripts\migration\enhanced-migration.ps1 validate`
2. **Logs prüfen**: `docker-compose logs -f [service-name]`
3. **Orphan-Cleanup**: `.\scripts\migration\enhanced-migration.ps1 orphans`
4. **Container-Reset**: `docker-compose down -v && docker-compose up -d`

### Dokumentation aktualisieren
Die meisten Dokumentationen werden **automatisch aktualisiert** durch die PowerShell-Tools. Bei manuellen Änderungen:

```powershell
# Migration-Dokumentation aktualisieren
.\scripts\migration\enhanced-migration.ps1 sync

# System-Status synchronisieren
.\scripts\migration\enhanced-migration.ps1 status
```

---

**Letzte Aktualisierung**: 2025-07-23  
**Automatisch generiert** durch UGH PowerShell Tools

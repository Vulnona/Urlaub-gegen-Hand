# Urlaub gegen Hand

## Schnellstart

### Erste Einrichtung
```powershell
# 1. Container starten (vollautomatische Migration & 2FA-Setup)
docker-compose up -d

# 2. Backend-Entwicklungsumgebung einrichten (einmalig)
.\scripts\setup-backend-dev.ps1
```

### Tägliche Entwicklung
```powershell
# System starten (alles automatisch)
docker-compose up -d
```


## Dokumentation

### Hauptdokumentation
* **[Dokumentations-Index](docs/INDEX.md)** - Zentrale Übersicht aller Dokumentationen
* **[Development Guide](docs/DEVELOPMENT.md)** - Vollständiger Entwicklerleitfaden
* **[Scripts & Automation](docs/SCRIPTS.md)** - Alle PowerShell-Tools und Automatisierung

### Security & Admin
* **[Admin Security](docs/ADMIN-SECURITY.md)** - Admin-Setup und Sicherheit
* **[Migration System](docs/MIGRATION-SYSTEM.md)** - Erweiterte Migration-Verwaltung und (beinahe) Zero-Maintenance

### Quick Links
* **[Migration Tools](scripts/migration/)** - Migration System mit Auto-Dokumentation
* **[Infrastructure Tools](scripts/infrastructure/)** - Port & Service Management  
* **[Database Tools](scripts/database/)** - Backup, Restore & Access Tools
### Legacy Documentation
* **[Business Specs](docs/)** - Fachliche Spezifikationen und Konzepte

## 🌐 Services

Nach dem Start sind folgende Services verfügbar:
- **Frontend**: http://localhost:3002
- **Backend API**: http://localhost:8080  
- **Nginx Proxy**: http://localhost:81
- **MySQL**: localhost:3306

## ⚙️ Port-Konfiguration

Ports können zentral in port.config geändert werden, siehe hierzu **[Port Management](docs/PORTS.md)**

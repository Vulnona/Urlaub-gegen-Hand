# Urlaub gegen Hand

## Schnellstart

### Erste Einrichtung
```powershell
# 1. Container starten (vollautomatische Migration & 2FA-Setup)
docker-compose up -d

# 2. Backend-Entwicklungsumgebung einrichten (einmalig)
.\scripts\setup-backend-dev.ps1
```

> **✅ Vollautomatisiert**: Migrations und 2FA werden automatisch eingerichtet - keine manuellen Schritte erforderlich!

### Tägliche Entwicklung
```powershell
# System starten (alles automatisch)
docker-compose up -d
```

> **🤖 Zero-Maintenance**: Neue Migrationen werden automatisch erkannt und angewendet!

## Dokumentation

### 📚 Hauptdokumentation
* **[📖 Dokumentations-Index](Docs/INDEX.md)** - Zentrale Übersicht aller Dokumentationen
* **[🛠️ Development Guide](Docs/DEVELOPMENT.md)** - Vollständiger Entwicklerleitfaden
* **[⚙️ Scripts & Automation](Docs/SCRIPTS.md)** - Alle PowerShell-Tools und Automatisierung

### � Security & Admin
* **[🔒 Admin Security](ADMIN-SECURITY.md)** - Admin-Setup und Sicherheit
* **[🔄 Migration System](Docs/MIGRATION-SYSTEM.md)** - Erweiterte Migration-Verwaltung

### 🚀 Quick Links
* **[📋 Migration Tools](scripts/migration/)** - Enhanced Migration System mit Auto-Dokumentation
* **[🌐 Infrastructure Tools](scripts/infrastructure/)** - Port & Service Management  
* **[🗄️ Database Tools](scripts/database/)** - Backup, Restore & Access Tools

### 📖 Legacy Documentation
* **[Business Specs](Docs/)** - Fachliche Spezifikationen und Konzepte

## 🌐 Services

Nach dem Start sind folgende Services verfügbar:
- **Frontend**: http://localhost:3002
- **Backend API**: http://localhost:8080  
- **Nginx Proxy**: http://localhost:81
- **MySQL**: localhost:3306

## ⚙️ Port-Konfiguration

Ports können zentral in `scripts/infrastructure/ports.config` geändert werden:

**Windows:**
```powershell
.\scripts\infrastructure\port-management.ps1
```

**Linux/macOS:**
```bash
pwsh ./scripts/infrastructure/port-management.ps1
```

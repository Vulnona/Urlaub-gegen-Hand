# Scripts & Automation Documentation

Komplette Übersicht aller Automatisierungs-Scripts und Tools für das UGH-Projekt.

## Übersicht

Das UGH-Projekt verwendet ein **einheitliches PowerShell-basiertes Script-System** für alle Automatisierungsaufgaben. Alle Scripts sind **Cross-Platform** und laufen auf Windows, Linux und macOS mit PowerShell Core.

## Script-Kategorien

### 🔄 Migration Management (`scripts/migration/`)

**Enhanced Migration System** - Vollständige Migrations-Verwaltung mit automatischer Dokumentation:

```powershell
# Status aller Migrationen anzeigen
.\scripts\migration\enhanced-migration.ps1 status

# Neue Migration hinzufügen
.\scripts\migration\enhanced-migration.ps1 add -MigrationName "NeueFeature"

# Migrationen ausführen
.\scripts\migration\enhanced-migration.ps1 run

# System validieren
.\scripts\migration\enhanced-migration.ps1 validate

# Migration entfernen (mit Bestätigung)
.\scripts\migration\enhanced-migration.ps1 remove -Force

# Datenbank synchronisieren
.\scripts\migration\enhanced-migration.ps1 sync

# Orphan-Migrationen finden und entfernen
.\scripts\migration\enhanced-migration.ps1 orphans -DryRun
.\scripts\migration\enhanced-migration.ps1 orphans -Force
```

**Verfügbare Aktionen**: `validate`, `cleanup`, `run`, `add`, `remove`, `status`, `sync`, `orphans`

**Features**:
- **Auto-Dokumentation**: Aktualisiert automatisch `MIGRATION-SYSTEM.md`
- **Orphan-Cleanup**: Erkennt und entfernt verwaiste Migration-Dateien
- **Docker-Integration**: Vollständige Container-Validierung
- **Fehlerbehandlung**: Robuste Validierung vor jeder Operation
- **Dry-Run Modus**: Sichere Vorschau aller Änderungen

### 🌐 Infrastructure Management (`scripts/infrastructure/`)

**Port Management System** - Zentrale Port-Verwaltung:

```powershell
# Port-Status anzeigen
.\scripts\infrastructure\port-management.ps1 status

# Port-Konfiguration validieren
.\scripts\infrastructure\port-management.ps1 validate

# Docker-Services neustarten nach Port-Änderungen
.\scripts\infrastructure\port-management.ps1 restart

# Port-Konflikte prüfen
.\scripts\infrastructure\port-management.ps1 check
```

**Features**:
- **Zentrale Konfiguration**: `ports.config` für alle Services
- **Konflikt-Erkennung**: Automatische Port-Kollisions-Prüfung
- **Docker-Integration**: Automatisches compose.yaml Update
- **Service-Management**: Koordinierter Neustart aller Services

### Database Management (`scripts/database/`)

**Database Tools** - Backup, Restore und Zugriff:

```powershell
# Datenbank-Zugriff (interaktive MySQL-Shell)
.\scripts\database\database-access.ps1

# Datenbank-Backup erstellen
.\scripts\database\database-dump.ps1

# Datenbank aus Backup wiederherstellen
.\scripts\database\database-restore.ps1 -BackupFile "backup.sql"
```

**Features**:
- **Sichere Authentifizierung**: File-basierte Passwort-Verwaltung
- **Automatische Backups**: Timestamped backup files
- **Container-Integration**: Direkte Docker-Container-Kommunikation
- **Fehlerbehandlung**: Validierung vor kritischen Operationen

## Installation & Voraussetzungen

### PowerShell Core (Cross-Platform)

#### Windows
```powershell
# Mit Winget (empfohlen)
winget install Microsoft.PowerShell

# Mit Chocolatey
choco install powershell-core
```

#### Linux/macOS
```bash
# Linux (Ubuntu/Debian)
sudo apt update && sudo apt install -y powershell

# macOS
brew install powershell
```

[Vollständige Installationsanleitung](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell)

### Docker & Docker Compose

Alle Scripts setzen eine funktionierende Docker-Umgebung voraus:
- Docker Desktop (Windows/macOS)
- Docker Engine + Docker Compose (Linux)

## 📖 Verwendung

### Windows
```powershell
.\scripts\[kategorie]\[script-name].ps1 [Parameter]
```

### Linux/macOS
```bash
pwsh scripts/[kategorie]/[script-name].ps1 [Parameter]
```

## 🔧 Konfiguration

### Port-Konfiguration (`scripts/infrastructure/ports.config`)
```
# UGH Port Configuration
FRONTEND_PORT=3002
BACKEND_PORT=8081
NGINX_PORT=81
MYSQL_PORT=3306
```

### Datenbank-Passwort (`.docker/db/secrets/.db-root-password.txt`)
```
your-secure-mysql-root-password
```

## Troubleshooting

### Häufige Probleme

**Problem**: Migration-Dateien ohne EF-Registrierung
```powershell
# Lösung: Orphan-Cleanup verwenden
.\scripts\migration\enhanced-migration.ps1 orphans -DryRun
.\scripts\migration\enhanced-migration.ps1 orphans -Force
```

**Problem**: Port-Konflikte
```powershell
# Lösung: Port-Check ausführen
.\scripts\infrastructure\port-management.ps1 check
```

**Problem**: Docker-Container nicht verfügbar
```powershell
# Lösung: System validieren
.\scripts\migration\enhanced-migration.ps1 validate
```

## Weiterführende Dokumentation

- [Migration System Details](MIGRATION-SYSTEM.md)
- [Development Guide](development/README.md)
- [Admin Security](../ADMIN-SECURITY.md)

---

*Diese Dokumentation wird automatisch durch die Script-Tools aktualisiert.*

**Letzte Aktualisierung**: 2025-07-23

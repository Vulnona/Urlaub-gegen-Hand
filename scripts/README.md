# Scripts Directory

Einheitliche PowerShell-basierte Script-Sammlung für das UGH-Projekt.

**Cross-Platform**: Alle Scripts laufen auf Windows, Linux und macOS mit PowerShell Core.

## Struktur

```
scripts/
├── migration/
│   └── enhanced-migration.ps1    # Migration Management System
├── infrastructure/
│   ├── port-management.ps1       # Port & Service Management
│   └── ports.config             # Zentrale Port-Konfiguration
├── database/
│   ├── database-access.ps1       # Database Access Tool
│   ├── database-dump.ps1         # Database Backup Tool
│   └── database-restore.ps1      # Database Restore Tool
└── powershell/
    ├── secure-admin-setup.ps1     # Admin Security Setup
    ├── generate-admin-hash.ps1    # Admin Password Hash Generator
    └── migrate-db.ps1             # Legacy Migration Tool
```

## Installation PowerShell

### Windows
PowerShell ist bereits installiert.

### Linux/macOS
```bash
# Linux (Ubuntu/Debian)
sudo apt update && sudo apt install -y powershell

# macOS
brew install powershell
```

[Vollständige Installationsanleitung](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell)

## Verwendung

### Windows
```powershell
.\scripts\[kategorie]\[script-name].ps1 [Parameter]
```

### Linux/macOS
```bash
pwsh -File ./scripts/[kategorie]/[script-name].ps1 [Parameter]
```

**Hinweis**: Unter Linux benötigen Docker-Interaktionen oft root-Rechte (`sudo`).

## Script-Kategorien

### 🔄 Migration Management (`scripts/migration/`)

**Enhanced Migration System** - Vollständige Migrations-Verwaltung mit automatischer Dokumentation:

```powershell
# Status aller Migrationen
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

**✨ Auto-Dokumentation**: Das System aktualisiert automatisch `Docs/MIGRATION-SYSTEM.md` mit dem aktuellen Migrationsstatus bei jeder Änderung.

**🧹 Orphan-Cleanup**: Erkennt und entfernt Migration-Dateien, die physisch vorhanden sind, aber nicht in Entity Framework registriert wurden.

### 🌐 Infrastructure Management (`scripts/infrastructure/`)

**Port Management System** - Zentrale Port-Verwaltung:

```powershell
# Port-Status anzeigen
.\scripts\infrastructure\port-management.ps1 -Validate -Verbose

# Spezifischen Port ändern
.\scripts\infrastructure\port-management.ps1 -Port BACKEND_PORT -Value 8082

# Alle Ports aktualisieren
.\scripts\infrastructure\port-management.ps1 -RestartServices
```

**Funktionen**: Port-Validation, Compose-Updates, Service-Restart

### 🗄️ Database Management (`scripts/database/`)

**Database Access Tool**:
```powershell
# Benutzer anzeigen
.\scripts\database\database-access.ps1 -Action users

# Mitgliedschaften anzeigen  
.\scripts\database\database-access.ps1 -Action memberships

# Interactive MySQL Shell
.\scripts\database\database-access.ps1 -Action open
```

**Database Backup Tool**:
```powershell
# Automatisches Backup
.\scripts\database\database-dump.ps1

# Benanntes Backup
.\scripts\database\database-dump.ps1 -OutputFile my-backup.sql

# Optimiertes Backup
.\scripts\database\database-dump.ps1 -NoTablespaces -SkipExtendedInsert
```

**Database Restore Tool**:
```powershell
# Backup einspielen
.\scripts\database\database-restore.ps1 -BackupFile backup.sql

# Force-Restore ohne Bestätigung
.\scripts\database\database-restore.ps1 -BackupFile backup.sql -Force
```

### 🔐 Security & Admin (`scripts/powershell/`)

**Admin Setup Tool**:
```powershell
# Sicheres Admin-Setup
.\scripts\powershell\secure-admin-setup.ps1

# Admin-Password Hash generieren
.\scripts\powershell\generate-admin-hash.ps1
```

## Konfiguration

### Port-Konfiguration (`scripts/infrastructure/ports.config`)

Zentrale Konfiguration aller System-Ports:

```config
# Frontend Ports
FRONTEND_PORT=3002
FRONTEND_DEV_PORT=3002

# Backend Ports  
BACKEND_PORT=8081
BACKEND_API_PORT=8081

# Database Ports
DATABASE_PORT=3306

# Web Server Ports
WEBSERVER_PORT=81
```

**Workflow bei Port-Änderungen**:
1. `ports.config` bearbeiten
2. `.\scripts\infrastructure\port-management.ps1` ausführen
3. Optional: Services mit `-RestartServices` neustarten

## Docker Integration

### Container-Script-Zugriff

Scripts können direkt in Docker-Containern verwendet werden:

```bash
# Im Backend-Container
docker exec ugh-backend pwsh -File /app/scripts/migration/enhanced-migration.ps1 -Action status

# Database-Tools
docker exec ugh-db mysql -u root -p
```

### Script-Abhängigkeiten

- **Migration Scripts**: Benötigen aktive Docker-Container (`ugh-db`, `ugh-backend`)
- **Database Scripts**: Benötigen Database-Secrets (`.docker/db/secrets/`)
- **Infrastructure Scripts**: Benötigen `compose.yaml` Zugriff

## Troubleshooting

### Häufige Probleme

1. **"PowerShell nicht gefunden"**
   ```bash
   # Linux/macOS: PowerShell installieren
   sudo apt install powershell
   brew install powershell
   ```

2. **"Container nicht verfügbar"**
   ```powershell
   # Docker-Services starten
   docker-compose up -d
   
   # Container-Status prüfen
   docker ps
   ```

3. **"Database connection failed"**
   ```powershell
   # Passwort-Datei prüfen
   Test-Path ".docker\db\secrets\.db-root-password.txt"
   
   # Container-Logs prüfen
   docker-compose logs db
   ```

4. **"Permission denied" (Linux)**
   ```bash
   # Mit sudo ausführen
   sudo pwsh -File ./scripts/database/database-access.ps1 -Action open
   ```

### Debug-Modi

Alle Scripts unterstützen Verbose- und Debug-Modi:

```powershell
# Verbose Output
.\scripts\[script].ps1 -Verbose

# Dry-Run (keine Änderungen)
.\scripts\infrastructure\port-management.ps1 -DryRun

# Help anzeigen
.\scripts\[script].ps1 -Help
```

## Migration von Bash

Alte `.sh` Scripts wurden zu PowerShell konvertiert:

- `accessdb.sh` → `scripts/database/database-access.ps1`
- `dumpdb.sh` → `scripts/database/database-dump.ps1`
- `restoredb.sh` → `scripts/database/database-restore.ps1`

**Kompatibilität**: Alte Scripts funktionieren weiterhin, neue PowerShell-Versionen bieten aber mehr Features und bessere Fehlerbehandlung.

## Best Practices

1. **Immer Help lesen**: `.\script.ps1 -Help`
2. **Dry-Run nutzen**: Bei kritischen Operationen erst `-DryRun`
3. **Verbose-Mode**: Bei Problemen `-Verbose` aktivieren
4. **Container-Status prüfen**: Vor Database-Operationen `docker ps`
5. **Backups erstellen**: Vor Restore-Operationen immer Backup
6. **Force vermeiden**: Nur bei vollautomatisierten Workflows `-Force`

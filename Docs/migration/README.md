# Migration Management System

Dieses Verzeichnis enthält das komplette Migrations-Management-System für das UGH-Projekt.

## Dateien

- `enhanced-migration.ps1` - Hauptskript für alle Migrations-Operationen
- `README.md` - Diese Dokumentation

## Verwendung

### Basis-Kommandos

```powershell
# Status aller Migrationen anzeigen
.\enhanced-migration.ps1 -Action status

# Alle Migrationen validieren
.\enhanced-migration.ps1 -Action validate

# Datenbank aufräumen (Backup + Reset)
.\enhanced-migration.ps1 -Action cleanup

# Migrationen ausführen
.\enhanced-migration.ps1 -Action run

# Neue Migration hinzufügen
.\enhanced-migration.ps1 -Action add -MigrationName "NeueFeature"

# Letzte Migration entfernen
.\enhanced-migration.ps1 -Action remove

# Datenbank synchronisieren
.\enhanced-migration.ps1 -Action sync
```

### Sicherheitsfeatures

- **Automatische Backups** vor jeder kritischen Operation
- **Dummy-proof Validierung** verhindert versehentliche Datenverluste
- **Colored Output** für bessere Übersichtlichkeit
- **Docker Integration** für Container-basierte Umgebungen

### Abhängigkeiten

- PowerShell 5.1 oder höher
- Docker mit laufenden UGH-Containern
- Entity Framework Tools (dotnet-ef)
- MySQL-Client

## Technische Details

Das Skript verwendet folgende Container:
- `ugh-db` - MySQL Database Container
- `ugh-backend` - .NET Backend Container mit EF Tools

Backup-Speicherort: `/tmp/migration-backups` (im Container)
Maximale Backups: 10 (automatische Bereinigung)

## Troubleshooting

### Häufige Probleme

1. **"Container nicht gefunden"**
   - Prüfen: `docker ps` zeigt alle UGH-Container
   - Lösung: `docker-compose up -d`

2. **"EF Tools nicht verfügbar"**
   - Prüfen: `docker exec ugh-backend dotnet ef --version`
   - Lösung: `docker exec ugh-backend dotnet tool restore`

3. **"Database connection failed"**
   - Prüfen: Container-Gesundheit `docker ps`
   - Prüfen: Passwort-Datei `.docker/db/secrets/.db-root-password.txt`

### Logs und Debugging

```powershell
# Dry-Run Modus (keine Änderungen)
.\enhanced-migration.ps1 -Action validate -DryRun

# Force-Modus (ignoriert Warnungen)
.\enhanced-migration.ps1 -Action cleanup -Force
```

## Integration

Das Migrations-System ist vollständig in die Docker-Umgebung integriert:

1. **Migration Container** (`.docker/migration/`) führt Basis-Setup aus
2. **Enhanced Script** bietet erweiterte Funktionalität
3. **Ports Config** (siehe `../infrastructure/`) steuert Container-Kommunikation

Für Port-Management siehe: `../infrastructure/README.md`

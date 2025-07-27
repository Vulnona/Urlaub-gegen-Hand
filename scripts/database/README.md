# UGH Database Management Scripts

Diese Skripte ermöglichen es, die UGH-Datenbank (Urlaub gegen Hand) einfach zu verwalten und für andere Entwickler bereitzustellen.

## 🚀 Quick Start für Entwickler

### Für neue Entwickler:
```powershell
# 1. Projekt klonen
git clone <repository-url>
cd UGH

# 2. Projekt starten (automatisch mit aktuellen Daten)
.\scripts\database\quick-start.ps1
```

### Für bestehende Entwickler:
```powershell
# Projekt mit aktuellen Daten starten
.\scripts\database\quick-start.ps1

# Oder mit frischer Datenbank starten
.\scripts\database\quick-start.ps1 -Reset
```

## 📦 Backup-System

### Aktuellen Stand als Standard-Backup speichern:
```powershell
# Erstellt Backup der aktuellen Datenbank und macht es zum Standard
.\scripts\database\create-dev-backup.ps1

# Mit benutzerdefiniertem Namen
.\scripts\database\create-dev-backup.ps1 -BackupName "mein-backup"

# Überschreiben ohne Bestätigung
.\scripts\database\create-dev-backup.ps1 -Force
```

### Was passiert beim Backup-Erstellen:
1. **Backup erstellen**: Aktuelle Datenbank wird gesichert
2. **Standard-Backup aktualisieren**: `backups/standard_users.sql` wird überschrieben
3. **Docker-Init aktualisieren**: `data.sql/init.sql` wird für automatische Initialisierung aktualisiert

## 🔧 Manuelle Datenbank-Operationen

### Backup erstellen:
```powershell
.\scripts\database\database-dump.ps1 my-backup
```

### Backup wiederherstellen:
```powershell
.\scripts\database\database-restore.ps1 backups\my-backup.sql
```

### Datenbank zurücksetzen:
```powershell
.\scripts\database\resetdb.ps1
```

## 📁 Dateistruktur

```
scripts/database/
├── create-dev-backup.ps1      # Erstellt Standard-Backup für Entwickler
├── quick-start.ps1            # Startet Projekt für Entwickler
├── database-dump.ps1          # Erstellt manuelles Backup
├── database-restore.ps1       # Stellt Backup wieder her
├── resetdb.ps1               # Setzt Datenbank zurück
├── database-access.ps1       # Datenbank-Zugriff
├── get-mysql-creds.js        # MySQL-Credentials auslesen
└── README.md                 # Diese Datei

backups/
├── standard_users.sql        # Standard-Backup für neue Entwickler
└── [weitere Backups...]

data.sql/
└── init.sql                 # Automatische DB-Initialisierung
```

## 🎯 Workflow für Projekt-Updates

### Wenn Sie Änderungen an der Datenbank haben:

1. **Entwickeln und testen** Sie Ihre Änderungen
2. **Backup erstellen** mit dem aktuellen Stand:
   ```powershell
   .\scripts\database\create-dev-backup.ps1
   ```
3. **Commit und Push** der Änderungen:
   ```bash
   git add .
   git commit -m "Update database with new users/offers"
   git push
   ```

### Für andere Entwickler:
1. **Projekt pullen**:
   ```bash
   git pull
   ```
2. **Projekt neu starten**:
   ```powershell
   .\scripts\database\quick-start.ps1 -Reset
   ```

## 🔍 Troubleshooting

### Häufige Probleme:

**Docker nicht gefunden:**
- Docker Desktop installieren und starten

**Ports bereits belegt:**
- Andere Services stoppen, die Ports 81, 3002, 8081, 3306 verwenden

**Datenbank-Initialisierung fehlgeschlagen:**
- Mit Reset-Modus versuchen: `.\quick-start.ps1 -Reset`

**Backup-Erstellung fehlgeschlagen:**
- Container-Status prüfen: `docker ps`
- Datenbank-Container starten: `docker-compose up -d db`

### Logs anzeigen:
```powershell
# Alle Services
docker-compose logs

# Nur Datenbank
docker-compose logs db

# Live-Logs
docker-compose logs -f
```

## 📋 Test-Accounts

Nach dem Start sind folgende Test-Accounts verfügbar:

- **Admin**: `adminuser@example.com` / `password`
- **User 1**: `test@example.com` / `password`
- **User 2**: `test1@example.com` / `password`

## 🌐 Zugangsdaten

- **Frontend**: http://localhost:3002
- **Backend API**: http://localhost:8081
- **Nginx**: http://localhost:81
- **Datenbank**: localhost:3306

## ⚡ Automatisierung

Das System ist so konfiguriert, dass:

1. **Erste Installation**: Datenbank wird automatisch mit `data.sql/init.sql` initialisiert
2. **Updates**: `create-dev-backup.ps1` aktualisiert automatisch die Initialisierungsdatei
3. **Entwickler-Start**: `quick-start.ps1` startet alles mit einem Befehl

## 🔄 Migration-System

Das System arbeitet mit Entity Framework Core Migrationen:

- **Automatische Migrationen**: Werden beim Backend-Start ausgeführt
- **Manuelle Migrationen**: Über `scripts/migration/migration.ps1`
- **Backup-Kompatibilität**: Backups enthalten alle Daten, Migrationen nur Schema-Änderungen 
# UGH Backup System

## Übersicht

Das UGH Backup System ist eine **codeseitige Lösung** für automatische, sichere und regelmäßige Datenbank-Backups. Es integriert sich nahtlos in das bestehende System und nutzt den AWS S3 für die sichere Speicherung

## Features

### ✅ Automatische Backups
- **Tägliche Backups** (konfigurierbar)
- **Automatische Bereinigung** alter Backups (6 Monate Retention)
- **Fehlerbehandlung** mit Retry-Mechanismus
- **Detailliertes Logging**

### ✅ S3 Integration
- **Sichere Speicherung** in AWS S3
- **Automatischer Upload** nach Backup-Erstellung
- **Lokale Bereinigung** nach erfolgreichem Upload

### ✅ Admin Interface
- **Backup-Liste** anzeigen
- **Backup-Statistiken** einsehen
- **Manuelle Backups** erstellen
- **Backup-Download** für Wiederherstellung
- **Backup-Löschung** (nur Admins)

### ✅ Monitoring
- **PowerShell-Script** für Monitoring
- **Health-Checks** der Backup-API
- **Status-Überwachung** (recent/warning/old)

## Architektur

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────┐
│   Background    │    │   Database       │    │    AWS S3   │
│   Service       │───▶│   Backup         │───▶│   Storage   │
│   (24h Loop)    │    │   Creation       │    │             │
└─────────────────┘    └──────────────────┘    └─────────────┘
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌──────────────────┐    ┌─────────────┐
│   Admin API     │    │   S3 Upload      │    │   Cleanup   │
│   Controller    │    │   Service        │    │   Old Files │
└─────────────────┘    └──────────────────┘    └─────────────┘
```

## Background Services

### 1. Database Backup Service
- **Zweck**: Erstellt automatische Datenbank-Backups
- **Intervall**: Alle 12 Stunden
- **Retention**: 6 Monate
- **Speicherung**: AWS S3

### 2. Offer Expiration Service
- **Zweck**: Schließt abgelaufene Angebote automatisch
- **Intervall**: Täglich
- **Aktion**: `Active` → `Closed` wenn `ToDate < heute`

### 3. Offer Cleanup Service
- **Zweck**: Löscht geschlossene Angebote nach 1 Jahr
- **Intervall**: Täglich
- **Aktion**: Löscht `Closed` Offers die älter als 1 Jahr sind
- **Sicherheit**: Löscht auch zugehörige Bilder (Pictures)

## Konfiguration

### appsettings.json

```json
{
  "BackupSettings": {
    "BackupIntervalHours": 12,
    "RetentionDays": 180,
    "S3BackupPrefix": "backups/database/",
    "EnableAutomaticBackups": true,
    "MaxBackupSizeMB": 100,
    "CompressBackups": false
  }
}
```

### Umgebungsvariablen

```bash
# AWS S3 Konfiguration (bereits vorhanden, falls du dir aber erhoffen solltest die in einem Readme zu finden... nein! )
AWS_ACCESS_KEY_ID=your-access-key
AWS_SECRET_ACCESS_KEY=your-secret-key
AWS_REGION=eu-central-1
AWS_BUCKET_NAME=your-bucket-name
```

## API Endpoints

### Backup Management (Admin Only)

| Endpoint | Method | Beschreibung |
|----------|--------|--------------|
| `/api/backup/list` | GET | Liste aller Backups |
| `/api/backup/stats` | GET | Backup-Statistiken |
| `/api/backup/create` | POST | Manuelles Backup erstellen |
| `/api/backup/download/{fileName}` | GET | Backup herunterladen |
| `/api/backup/delete/{fileName}` | DELETE | Backup löschen |

### Beispiel-Responses

#### Backup Liste
```json
{
  "TotalBackups": 5,
  "Backups": [
    {
      "FileName": "ugh-db-backup_2025-01-27_14-30-00.sql",
      "S3Key": "backups/database/ugh-db-backup_2025-01-27_14-30-00.sql",
      "Size": 5242880,
      "LastModified": "2025-01-27T14:30:00Z",
      "SizeInMB": 5.0
    }
  ]
}
```

#### Backup Statistiken
```json
{
  "TotalBackups": 5,
  "TotalSizeMB": 25.5,
  "TotalSizeGB": 0.025,
  "OldestBackup": "2024-12-28T14:30:00Z",
  "NewestBackup": "2025-01-27T14:30:00Z",
  "LastBackupFileName": "ugh-db-backup_2025-01-27_14-30-00.sql"
}
```

## PowerShell Monitoring

### Installation

Das Monitoring-Script befindet sich in `scripts/backup/backup-monitor.ps1`

### Verwendung

```powershell
# Backup-Status anzeigen
.\backup-monitor.ps1 -ShowStats

# Backup-Liste anzeigen
.\backup-monitor.ps1 -ListBackups

# Manuelles Backup erstellen
.\backup-monitor.ps1 -CreateBackup -AdminToken "your-jwt-token"

# Mit Production API
.\backup-monitor.ps1 -ShowStats -ApiUrl "https://your-production-api.com"
```

### Beispiel-Output

```
Backup Statistics:
==================
Total Backups: 5
Total Size: 25.5 MB (0.025 GB)
Oldest Backup: 2024-12-28 14:30:00
Newest Backup: 2025-01-27 14:30:00
Last Backup: ugh-db-backup_2025-01-27_14-30-00.sql
Status: ✅ Backup is recent (2.5 hours ago)
```

## Deployment

### 1. Code Deployen

```bash
# Backend neu bauen
docker-compose build backend

# Services neu starten
docker-compose up -d
```

### 2. Konfiguration prüfen

```bash
# Backup-Service Status prüfen
docker-compose logs backend | grep "Database Backup Service"

# Offer Services Status prüfen
docker-compose logs backend | grep "Offer.*Service"
```

### 3. Ersten Backup testen

```powershell
# PowerShell Script ausführen
.\scripts\backup\backup-monitor.ps1 -ShowStats
```

## Sicherheit

### Zugriffskontrolle
- **Nur Admins** können Backup-Operationen ausführen
- **JWT-Token** erforderlich für alle Admin-Endpoints
- **S3-Bucket** mit IAM-Rollen konfiguriert

### Datenverschlüsselung
- **Backups** werden in S3 verschlüsselt gespeichert
- **Übertragung** über HTTPS/TLS
- **Lokale Dateien** werden nach Upload gelöscht

### Audit-Logging
- **Alle Backup-Operationen** werden geloggt
- **Fehler** werden detailliert protokolliert
- **Admin-Aktionen** werden nachverfolgt

## Monitoring & Alerting

### Automatische Überwachung

Der Backup-Service überwacht automatisch:
- ✅ **Backup-Erfolg** (alle 12 Stunden)
- ✅ **S3-Upload** Status
- ✅ **Retention-Policy** Einhaltung
- ✅ **Fehler** und Retry-Versuche

### Manuelle Überwachung

```powershell
# Täglicher Check (als Cron-Job)
.\backup-monitor.ps1 -ShowStats -ApiUrl "https://your-production-api.com"

# Bei Problemen
.\backup-monitor.ps1 -CreateBackup -AdminToken "your-jwt-token"
```

### Log-Monitoring

```bash
# Backup-Logs anzeigen
docker-compose logs backend | grep "Database Backup Service"

# Offer Service Logs
docker-compose logs backend | grep "Offer.*Service"

# Fehler-Logs
docker-compose logs backend | grep "ERROR.*Backup"
```

## Troubleshooting

### Häufige Probleme

#### 1. Backup-Service startet nicht
```bash
# Logs prüfen
docker-compose logs backend

# Service-Status
docker-compose ps backend
```

#### 2. S3-Upload fehlgeschlagen
```bash
# AWS Credentials prüfen
docker-compose exec backend env | grep AWS

# S3-Bucket-Zugriff testen
docker-compose exec backend aws s3 ls s3://your-bucket-name
```

#### 3. mysqldump nicht verfügbar
```bash
# MySQL-Client installieren
docker-compose exec backend apt-get update && apt-get install -y mysql-client
```

### Recovery

#### Backup wiederherstellen

1. **Backup herunterladen**
```powershell
.\backup-monitor.ps1 -DownloadBackup "ugh-db-backup_2025-01-27_14-30-00.sql"
```

2. **Datenbank wiederherstellen**
```bash
# Backup in Container kopieren
docker cp ugh-db-backup_2025-01-27_14-30-00.sql ugh-db:/tmp/

# Datenbank wiederherstellen
docker exec ugh-db mysql -u root -ppassword db < /tmp/ugh-db-backup_2025-01-27_14-30-00.sql
```

## Best Practices

### Konfiguration
- **Retention-Policy** an Datenmenge anpassen
- **Backup-Intervall** basierend auf Änderungsrate
- **S3-Lifecycle-Policy** für zusätzliche Sicherheit

### Monitoring
- **Tägliche Checks** der Backup-Status
- **Alerts** bei Backup-Fehlern
- **Regelmäßige Tests** der Wiederherstellung

### Sicherheit
- **Backup-Dateien** nicht öffentlich zugänglich
- **Admin-Tokens** sicher aufbewahren
- **S3-Bucket** mit minimalen Rechten konfigurieren

### Optimierung
- **Komprimierung** für große Backups
- **Lifecycle-Policy** für ältere Backups (S3-IA)
- **Selektive Backups** für große Tabellen


**Kein separater Backup-Server erforderlich!** 
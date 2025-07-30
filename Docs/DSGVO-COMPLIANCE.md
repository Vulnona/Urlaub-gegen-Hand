# DSGVO-Compliance für gelöschte User-Daten

## Übersicht

Dieses Dokument beschreibt die DSGVO-konforme Behandlung von gelöschten User-Daten in der UGH-Anwendung.

## Speicherfristen

### DeletedUserBackups
- **Standard-Speicherfrist**: 30 Tage
- **Konfigurierbar**: Über `appsettings.json` → `DSGVOSettings:DeletedUserRetentionDays`
- **Automatische Löschung**: Täglich um 00:00 Uhr

### Begründung der 30-Tage-Frist

Die 30-Tage-Frist ist DSGVO-konform, weil:

1. **Datenminimierung**: Nur so lange wie absolut nötig
2. **Zweckbindung**: Reviews sollen persistent bleiben, aber ohne persönliche Daten
3. **Recht auf Vergessenwerden**: User können nach 30 Tagen vollständig "vergessen" werden
4. **Technische Notwendigkeit**: Zeit für Support-Anfragen und System-Stabilität

## Implementierung

### Automatischer Cleanup-Service

```csharp
// Backend/Services/BackgroundTasks/DeletedUserBackupCleanupService.cs
public class DeletedUserBackupCleanupService : BackgroundService
{
    private readonly int _retentionDays = 30; // DSGVO-konform
    
    // Läuft alle 24 Stunden
    // Löscht automatisch abgelaufene Backups
}
```

### Fallback-Mechanismus

Wenn DeletedUserBackups gelöscht werden:

1. **Reviews bleiben bestehen** ✅
2. **User-Namen wird "Gelöschter Nutzer"** ✅
3. **Keine persönlichen Daten mehr vorhanden** ✅
4. **DSGVO-konform** ✅

## Konfiguration

### appsettings.json
```json
{
  "DSGVOSettings": {
    "DeletedUserRetentionDays": 30
  }
}
```

### Mögliche Werte:
- **30 Tage**: Standard (empfohlen)
- **14 Tage**: Minimal (nur bei besonderen Anforderungen)
- **90 Tage**: Maximal (nur bei rechtlichen Anforderungen)

## DSGVO-Prinzipien

### ✅ Erfüllte Anforderungen:

1. **Datenminimierung**: Nur notwendige Daten werden gespeichert
2. **Zweckbindung**: Daten nur für Review-System
3. **Speicherbegrenzung**: Automatische Löschung nach 30 Tagen
4. **Recht auf Vergessenwerden**: Vollständige Löschung möglich
5. **Datenintegrität**: Reviews bleiben trotz User-Löschung erhalten

### 📋 Compliance-Checkliste:

- [x] Automatische Löschung implementiert
- [x] Konfigurierbare Speicherfrist
- [x] Fallback-Mechanismus für Reviews
- [x] Logging aller Löschvorgänge
- [x] Dokumentation der Speicherfristen

## Monitoring

### Logs
Der Cleanup-Service loggt alle Aktivitäten:

```
[INFO] Found 5 expired DeletedUserBackups older than 30 days to delete
[INFO] Deleted 5 expired DeletedUserBackups at 2025-07-30 00:00:00
```

### Überwachung
- Tägliche Ausführung um 00:00 Uhr
- Fehlerbehandlung mit Retry-Mechanismus
- Detaillierte Logging aller Operationen

## Rechtliche Grundlagen

### DSGVO-Artikel:
- **Art. 5(1)(e)**: Speicherbegrenzung
- **Art. 17**: Recht auf Löschung
- **Art. 25**: Datenschutz durch Technikgestaltung

### Empfehlungen:
- Regelmäßige Überprüfung der Speicherfristen
- Dokumentation aller Löschvorgänge
- Transparente Kommunikation mit Nutzern

## Support

Bei Fragen zur DSGVO-Compliance:
- Dokumentation prüfen
- Logs analysieren
- Rechtliche Beratung einholen 
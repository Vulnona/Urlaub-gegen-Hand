# UgH Database Migration System

Ein robustes, automatisiertes Database Migration System mit umfassender Datenintegritäts-Validierung und Fehlerbehandlung

## Schnellstart

### ⚡ Einfache Befehle
```powershell
# Sichere Migration mit Validierung
.\scripts\powershell\migrate-db.ps1 run

# Datenprobleme prüfen  
.\scripts\powershell\migrate-db.ps1 validate

# Datenprobleme automatisch beheben
.\scripts\powershell\migrate-db.ps1 cleanup

# Migration-Status anzeigen
.\scripts\powershell\migrate-db.ps1 status
```

### Erstmaliges Setup
```powershell
# 1. Docker-Secrets erstellen (einmalig)
cp .docker/db/secrets/.db-root-password.txt .docker/db/secrets/db-root-password.txt
cp .docker/db/secrets/.db-user-password.txt .docker/db/secrets/db-user-password.txt

# 2. Vollständiger Start mit automatischen Migrationen
docker-compose up -d --build
```

## 🛡️ Robuste Migration Features

### **Datenintegritäts-Validierung**
Das System prüft AUTOMATISCH vor jeder Migration:

- **NULL-Werte**: Verhindert NOT NULL Constraint Fehler
- **Duplikate**: Verhindert UNIQUE Constraint Fehler  
- **Verwaiste Records**: Verhindert FOREIGN KEY Constraint Fehler
- **Datentyp-Konflikte**: Erkennt potentielle Konvertierungsprobleme

### **Automatische Datenbereinigung**
```powershell
# Behebt häufige Datenprobleme automatisch:
.\migrate-db.ps1 cleanup

# Was wird bereinigt:
# ✅ NULL emails → missing_email_[ID]@placeholder.com  
# ✅ Verwaiste Profiles → werden entfernt
# ✅ Verwaiste Offers → werden entfernt
# ✅ Doppelte Emails → erhalten _duplicate_[ID] Suffix
# ✅ NULL Foreign Keys → werden auf gültigen Wert gesetzt
```

### **Backup & Recovery**
- **Automatische Backups** vor jeder Migration
- **Manuelle Backups** jederzeit möglich
- **Einfache Wiederherstellung** bei Problemen

## Wann welches Kommando verwenden?

| Situation | Kommando | Beschreibung |
|-----------|----------|--------------|
| **Neue Migration anwenden** | `.\migrate-db.ps1 run` | Sicherste Option mit automatischer Validierung |
| **Probleme checken** | `.\migrate-db.ps1 validate` | Prüft Datenintegrität ohne Änderungen |
| **Migration fehlgeschlagen** | `.\migrate-db.ps1 cleanup` dann `run` | Behebt Datenprobleme, dann Migration |
| **Status anzeigen** | `.\migrate-db.ps1 status` | Zeigt aktuelle Migration-Historie |
| **Backup erstellen** | `.\migrate-db.ps1 backup` | Manuelles Sicherheitsbackup |
| **Notfall** | `.\migrate-db.ps1 run -SkipValidation` | Nur bei absoluter Notwendigkeit! |

## Kommando-Referenz

### Hauptbefehle
| Kommando | Beschreibung | Beispiel |
|----------|-------------|----------|
| `run` | Sichere Migration mit Validierung | `.\migrate-db.ps1 run` |
| `validate` | Datenintegritäts-Prüfung | `.\migrate-db.ps1 validate` |
| `cleanup` | Automatische Datenbereinigung | `.\migrate-db.ps1 cleanup` |
| `status` | Migration-Status anzeigen | `.\migrate-db.ps1 status` |
| `diagnose` | Probleme diagnostizieren | `.\migrate-db.ps1 diagnose` |

### Entwickler-Befehle  
| Kommando | Beschreibung | Beispiel |
|----------|-------------|----------|
| `create` | Neue Migration erstellen | `.\migrate-db.ps1 create AddUserTable` |
| `backup` | Manuelles Backup erstellen | `.\migrate-db.ps1 backup` |
| `restore` | Backup wiederherstellen | `.\migrate-db.ps1 restore -BackupFile backup.sql` |
| `reset` | Datenbank zurücksetzen | `.\migrate-db.ps1 reset -Force` |

### Optionen
| Option | Beschreibung | Verwendung |
|--------|-------------|------------|
| `-Force` | Überspringt Bestätigungen | `.\migrate-db.ps1 cleanup -Force` |
| `-SkipValidation` | ⚠️ Überspringt Sicherheitsprüfungen | `.\migrate-db.ps1 run -SkipValidation` |

## Migration Workflow

### 1. **Normale Migration (empfohlen)**
```powershell
# Schritt 1: Datenprobleme prüfen
.\migrate-db.ps1 validate

# Schritt 2: Falls Probleme → automatisch beheben  
.\migrate-db.ps1 cleanup

# Schritt 3: Sichere Migration durchführen
.\migrate-db.ps1 run
```

### 2. **Neue Migration entwickeln**
```powershell
# Migration erstellen
.\migrate-db.ps1 create "AddNewFeatureTable"

# Migration testen
.\migrate-db.ps1 validate
.\migrate-db.ps1 run

# Status prüfen
.\migrate-db.ps1 status
```

### 3. **Notfall-Recovery**
```powershell
# Problem diagnostizieren
.\migrate-db.ps1 diagnose

# Backup wiederherstellen (falls nötig)
.\migrate-db.ps1 restore -BackupFile "backup_20250723_101140.sql"

# Daten bereinigen und erneut versuchen
.\migrate-db.ps1 cleanup
.\migrate-db.ps1 run
```

## Technische Architektur

### Container-Setup
```yaml
# compose.yaml - Migration Service
migration:
  build: 
    context: .
    dockerfile: .docker/migration/Dockerfile
  depends_on:
    - db
  environment:
    - MYSQL_ROOT_PASSWORD_FILE=/run/secrets/db-root-password
  secrets:
    - db-root-password
  restart: "no"  # Läuft einmalig bei Start
```

### Datei-Struktur  
```
UGH/
├── migrate-db.ps1                    # 🔧 Haupt-Management-Script
├── .docker/migration/
│   ├── Dockerfile                    # Container-Definition
│   └── migrate.sh                    # Bash-Script im Container
├── Backend/Migrations/               # Entity Framework Migrationen
└── compose.yaml                      # Service-Orchestrierung
```

### Ausführungsreihenfolge
1. **Database Service** startet zuerst
2. **Migration Service** wartet auf Database  
3. **Backend Service** wartet auf Migration
4. **Frontend/Webserver** starten zuletzt

## Erweiterte Funktionen

### **Validierungs-Details**
```sql
-- NULL-Werte Prüfung  
SELECT 'users.email', COUNT(*) FROM users WHERE email IS NULL OR email = '';
SELECT 'userprofiles.user_id', COUNT(*) FROM userprofiles WHERE user_id IS NULL;

-- Duplikat-Prüfung
SELECT email, COUNT(*) FROM users GROUP BY email HAVING COUNT(*) > 1;

-- Verwaiste Records
SELECT COUNT(*) FROM userprofiles up 
LEFT JOIN users u ON up.user_id = u.Id 
WHERE u.Id IS NULL;
```

### 🔧 **Cleanup-Aktionen**
```sql
-- NULL Emails beheben
UPDATE users SET email = CONCAT('missing_email_', Id, '@placeholder.com') 
WHERE email IS NULL OR email = '';

-- Duplikate eindeutig machen  
UPDATE users u1 SET u1.email = CONCAT(u1.email, '_duplicate_', u1.Id)
WHERE u1.Id NOT IN (SELECT MIN(Id) FROM users u2 WHERE u2.email = u1.email);

-- Verwaiste Records entfernen
DELETE up FROM userprofiles up 
LEFT JOIN users u ON up.user_id = u.Id 
WHERE u.Id IS NULL;
```

### **Backup-System**
- **Automatisch**: Vor jeder Migration → `/tmp/db_backup_YYYYMMDD_HHMMSS.sql`
- **Manuell**: `.\migrate-db.ps1 backup` → `backup_YYYYMMDD_HHMMSS.sql`
- **Wiederherstellung**: `.\migrate-db.ps1 restore -BackupFile "backup.sql"`

## Troubleshooting

### **Häufige Probleme**

**Problem**: Migration schlägt mit NOT NULL Constraint fehl
```powershell
# Lösung:
.\migrate-db.ps1 validate  # Problem identifizieren
.\migrate-db.ps1 cleanup   # Automatisch beheben
.\migrate-db.ps1 run       # Migration wiederholen
```

**Problem**: UNIQUE Constraint Violation  
```powershell
# Lösung:
.\migrate-db.ps1 cleanup   # Duplikate automatisch auflösen
.\migrate-db.ps1 run
```

**Problem**: Migration hängt/bricht ab
```powershell
# Diagnose:
.\migrate-db.ps1 diagnose

# Recovery:
.\migrate-db.ps1 restore -BackupFile "letztes_backup.sql"
.\migrate-db.ps1 cleanup
.\migrate-db.ps1 run
```

### **Debug-Kommandos**
```powershell
# Container-Status prüfen
docker-compose ps

# Migration-Logs anzeigen  
docker-compose logs migration

# Direkt in Container schauen
docker-compose exec db mysql -u root -p db

# Manuelle Migration (Notfall)
docker-compose run --rm migration bash
```

### **Container neu bauen**
```powershell
# Bei Script-Änderungen erforderlich:
docker-compose build migration
```

## **Migration-Historie**

### **Erfolgreiche Migrationen**
```
20241016071620_InitialMigration
20241017101135_AddedCreatedAtColumnInOfferTable  
20241114104041_Removed-CreatedAt-And-UpdatedAt-Columns-from-Membership
20241218093137_Removed_DiscountAmount_from_Coupon_Table
20250124094728_Added_ShopItem_and_transaction_table_updated_coupon_table
20250124095631_Changed_coupon_redemption_table_relation
20250321154423_AddedPasswordResetToken
20250502111647_OfferRedesign
20250621114037_AddPaymentIntegration
```

### **System-Status**
- **Aktuelle Migration**: 20250621114037_AddPaymentIntegration
- **Migration-Anzahl**: 9 erfolgreich angewandt
- **Letzter Test**: ✅ 23.07.2025 - Alle Validierungen bestanden
- **Backup-System**: ✅ Aktiv und funktional
- **Container**: ✅ Automatischer Build bei Änderungen

---

## **Best Practices**

### ✅ **DOs**
- Immer `.\migrate-db.ps1 validate` vor produktiven Migrationen
- Bei Datenproblemen erst `cleanup`, dann `run`
- Backups vor wichtigen Änderungen: `.\migrate-db.ps1 backup`
- Status regelmäßig prüfen: `.\migrate-db.ps1 status`

### ❌ **DON'Ts**  
- Niemals `-SkipValidation` in Produktion verwenden
- Keine manuellen SQL-Änderungen ohne Migration
- Nicht `reset` ohne vollständiges Backup
- Migration-Container nicht manuell stoppen während Ausführung

### 💡 **Tipps**
- Bei Problemen immer erst `.\migrate-db.ps1 diagnose`
- Cleanup kann mehrfach ausgeführt werden (idempotent)
- Backups werden automatisch bei jedem `cleanup` erstellt
- Container wird automatisch neu gebaut bei Script-Änderungen

## Entwickler-Hinweise

### **Nach Skript-Änderungen:**
```powershell
# Container neu bauen (wichtig!)
docker-compose build migration
```

### **Typischer Entwicklungs-Workflow:**
```powershell
# 1. Neue Migration erstellen
dotnet ef migrations add NeueFeature --context Ugh_Context

# 2. Container bauen (bei Skript-Änderungen)
docker-compose build migration

# 3. Sicher anwenden
.\migrate-db.ps1 run
```

### **Fehlerbehebung-Workflow:**
```powershell
# 1. Was ist das Problem?
.\migrate-db.ps1 validate

# 2. Automatisch beheben
.\migrate-db.ps1 cleanup

# 3. Migration erneut versuchen  
.\migrate-db.ps1 run
```

### **Bei hartnäckigen Problemen:**
```powershell
# Logs detailliert anschauen
docker-compose logs migration --tail 50

# System-Status prüfen
docker-compose ps

# Container-Zustand prüfen
docker-compose exec migration bash
```


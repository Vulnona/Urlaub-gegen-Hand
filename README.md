# Urlaub gegen Hand

## Schnellstart

### Erste Einrichtung
```powershell
# 1. Container starten (automatische Migration)
docker-compose up -d

# 2. Bei Migrationsproblemen (Windows):
.\scripts\powershell\migrate-db.ps1 validate  # Probleme prüfen
.\scripts\powershell\migrate-db.ps1 cleanup   # Probleme beheben
.\scripts\powershell\migrate-db.ps1 run       # Migration ausführen

# 2. Bei Migrationsproblemen (Linux/macOS):
pwsh ./scripts/powershell/migrate-db.ps1 validate  # Probleme prüfen
pwsh ./scripts/powershell/migrate-db.ps1 cleanup   # Probleme beheben
pwsh ./scripts/powershell/migrate-db.ps1 run       # Migration ausführen
```

### Tägliche Entwicklung
```powershell
# System starten
docker-compose up -d

# Bei neuen Migrationen (Windows):
.\scripts\powershell\migrate-db.ps1 run

# Bei neuen Migrationen (Linux/macOS):
pwsh ./scripts/powershell/migrate-db.ps1 run
```

## Dokumentation

### 📚 Kern-Dokumentation
* [Migrations-System Schnellreferenz](MIGRATION-QUICK-REFERENCE.md) - **Wichtig für Entwickler!**
* [Vollständige Migrations-Dokumentation](MIGRATION-SYSTEM.md)
* [Admin-Sicherheit & Passwort-Management](ADMIN-SECURITY.md)

### 🛠️ Entwicklung & Tools
* [Entwicklerdokumentation](Docs/development/README.md)
* [PowerShell-Skripte](scripts/powershell/README.md) - Automatisierungstools

### ⚙️ Scripts & Tools
* [PowerShell Scripts Übersicht](scripts/powershell/README.md)

## 🌐 Services

Nach dem Start sind folgende Services verfügbar:
- **Frontend**: http://localhost:3002
- **Backend API**: http://localhost:8080  
- **Nginx Proxy**: http://localhost:81
- **MySQL**: localhost:3306

## ⚙️ Port-Konfiguration

Ports können zentral in `ports.config` geändert werden:

**Windows:**
```powershell
.\scripts\powershell\update-ports.ps1
```

**Linux/macOS:**
```bash
pwsh ./scripts/powershell/update-ports.ps1
```

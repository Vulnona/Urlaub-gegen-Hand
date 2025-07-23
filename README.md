# Urlaub gegen Hand

## Schnellstart

### Erste Einrichtung
```powershell
# 1. Container starten (automatische Migration)
docker-compose up -d

# 2. Bei Migrationsproblemen:
.\migrate-db.ps1 validate  # Probleme prüfen
.\migrate-db.ps1 cleanup   # Probleme beheben
.\migrate-db.ps1 run       # Migration ausführen
```

### Tägliche Entwicklung
```powershell
# System starten
docker-compose up -d

# Bei neuen Migrationen
.\migrate-db.ps1 run
```

## Dokumentation

* [Migrations-System Schnellreferenz](MIGRATION-QUICK-REFERENCE.md) - **Wichtig für Entwickler!**
* [Vollständige Migrations-Dokumentation](MIGRATION-SYSTEM.md)
* [Entwicklerdokumentation](Docs/development/README.md)

## 🌐 Services

Nach dem Start sind folgende Services verfügbar:
- **Frontend**: http://localhost:3001
- **Backend API**: http://localhost:8080  
- **Nginx Proxy**: http://localhost:81
- **MySQL**: localhost:3306

## ⚙️ Port-Konfiguration

Ports können zentral in `ports.config` geändert werden:
```powershell
.\update-ports.ps1
```

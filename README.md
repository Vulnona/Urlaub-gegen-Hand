# UGH - Urlaub gegen Hand

Eine moderne Webplattform für Menschen, die Urlaub gegen Handarbeit tauschen möchten.

## Quick Start für Entwickler

### Voraussetzungen
- Docker Desktop
- Git

### Einfacher Start (Empfohlen)
```powershell
# 1. Projekt klonen
git clone <repository-url>
cd UGH

# 2. Projekt starten (automatisch mit aktuellen Daten)
.\scripts\database\quick-start.ps1
```

Das war's! Das Projekt startet automatisch mit allen aktuellen Usern und Angeboten.

### Alternative: Manueller Start
```powershell
# Services starten
docker-compose up -d

# Mit frischer Datenbank starten
.\scripts\database\quick-start.ps1 -Reset
```

## 📦 Backup-System

### Aktuellen Stand als Standard-Backup speichern:
```powershell
# Erstellt Backup der aktuellen Datenbank und macht es zum Standard
.\scripts\database\create-dev-backup.ps1
```

### Was passiert beim Backup-Erstellen:
1. **Backup erstellen**: Aktuelle Datenbank wird gesichert
2. **Standard-Backup aktualisieren**: `backups/standard_users.sql` wird überschrieben
3. **Docker-Init aktualisieren**: `data.sql/init.sql` wird für automatische Initialisierung aktualisiert

## 🌐 Zugangsdaten

Nach dem Start sind folgende Services verfügbar:

- **Frontend**: http://localhost:3002
- **Backend API**: http://localhost:8081
- **Nginx**: http://localhost:81
- **Datenbank**: localhost:3306

### Test-Accounts
- **Admin**: `adminuser@example.com` / `password`
- **User 1**: `test@example.com` / `password`
- **User 2**: `test1@example.com` / `password`

## 🔧 Entwicklung

### Projekt-Struktur
```
UGH/
├── Backend/                 # ASP.NET Core API
├── Frontend-Vuetify/        # Vue.js Frontend
├── scripts/                 # PowerShell-Skripte
│   ├── database/           # Datenbank-Management
│   ├── migration/          # EF Core Migrationen
│   └── setup/              # Setup-Skripte
├── backups/                # Datenbank-Backups
├── data.sql/              # Automatische DB-Initialisierung
└── compose.yaml           # Docker Compose Konfiguration
```

### Nützliche Befehle
```powershell
# Logs anzeigen
docker-compose logs -f

# Services stoppen
docker-compose down

# Reset Datenbank
.\scripts\database\quick-start.ps1 -Reset

# Backup erstellen
.\scripts\database\create-dev-backup.ps1

# Migrationen ausführen
.\scripts\migration\migration.ps1
```

## 📚 Dokumentation

- [Database Management](scripts/database/README.md) - Detaillierte Anleitung für Datenbank-Management
- [Development Guide](Docs/DEVELOPMENT.md) - Entwicklungsrichtlinien
- [Migration System](Docs/MIGRATION-SYSTEM.md) - EF Core Migrationen
- [Admin Security](Docs/ADMIN-SECURITY.md) - Admin-Sicherheit

## 🔄 Workflow für Updates

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

## 🛠️ Technologie-Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core
- **Database**: MySQL 8.0
- **Authentication**: JWT Bearer Tokens
- **Architecture**: Clean Architecture mit CQRS

### Frontend
- **Framework**: Vue.js 3
- **UI Library**: Vuetify 3
- **Build Tool**: Vite
- **Language**: TypeScript

### Infrastructure
- **Containerization**: Docker & Docker Compose
- **Web Server**: Nginx
- **Database**: MySQL 8.0
- **Scripts**: PowerShell

## 🔍 Troubleshooting

### Häufige Probleme:

**Docker nicht gefunden:**
- Docker Desktop installieren und starten

**Ports bereits belegt:**
- Andere Services stoppen, die Ports 81, 3002, 8081, 3306 verwenden

**Datenbank-Initialisierung fehlgeschlagen:**
- Mit Reset-Modus versuchen: `.\quick-start.ps1 -Reset`

### Logs anzeigen:
```powershell
# Alle Services
docker-compose logs

# Nur Datenbank
docker-compose logs db

# Live-Logs
docker-compose logs -f
```


## 🤝 Beitragen

1. Fork des Repositories
2. Feature-Branch erstellen (`git checkout -b feature/AmazingFeature`)
3. Änderungen committen (`git commit -m 'Add some AmazingFeature'`)
4. Branch pushen (`git push origin feature/AmazingFeature`)
5. Pull Request erstellen

## Support

Bei Fragen oder Problemen:
1. Dokumentation prüfen
2. Issues im Repository erstellen


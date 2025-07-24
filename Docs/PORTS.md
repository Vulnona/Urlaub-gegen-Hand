# Infrastructure Management

Dieses Verzeichnis enthält alle Skripte und Konfigurationen für die UGH-Infrastruktur.

## Dateien

- `port-management.ps1` - Zentralisierte Port-Verwaltung
- `ports.config` - Zentrale Port-Konfiguration
- `README.md` - Diese Dokumentation

## Port-Management

### Zentrale Konfiguration

Alle Ports werden in `ports.config` definiert:

```config
# Frontend Ports
FRONTEND_PORT=3002
FRONTEND_DEV_PORT=3002

# Backend Ports  
BACKEND_PORT=8081
BACKEND_API_PORT=8081

# Database Ports
DATABASE_PORT=3306
DATABASE_HOST=ugh-db

# Web Server Ports
WEBSERVER_PORT=81
WEBSERVER_INTERNAL_PORT=80

# Development Ports
DEV_MAIL_PORT=1025
DEV_MAIL_WEB_PORT=1080
```

### Port-Management Skript

```powershell
# Alle Ports aktualisieren
.\port-management.ps1

# Spezifischen Port ändern
.\port-management.ps1 -Port BACKEND_PORT -Value 8082

# Validation-Modus
.\port-management.ps1 -Validate

# Docker-Services neustarten
.\port-management.ps1 -RestartServices
```

### Funktionen

1. **Zentrale Verwaltung** - Alle Ports in einer Datei
2. **Automatische Updates** - docker-compose.yaml wird automatisch aktualisiert
3. **Validation** - Prüft Port-Konflikte und Verfügbarkeit
4. **Service Restart** - Startet betroffene Container automatisch neu

### Integration

Das Port-Management System integriert sich mit:

- **Docker Compose** - Automatische Aktualisierung der Service-Ports
- **Backend Container** - Dynamisches Lesen via start.sh
- **Nginx Configuration** - Proxy-Weiterleitungen
- **Frontend Development** - Vite/Vue Development Server

### Workflow bei Port-Änderungen

1. **Konfiguration ändern** in `ports.config`
2. **Skript ausführen** `.\port-management.ps1`
3. **Automatische Updates**:
   - docker-compose.yaml Ports
   - Backend start.sh Umgebungsvariablen
   - Nginx Proxy-Konfiguration
4. **Services neustarten** (optional mit `-RestartServices`)

## Docker Integration

### Container-Start Scripts

Jeder Container liest dynamisch die Port-Konfiguration:

```bash
# Backend start.sh
source /app/read-ports.sh
export ASPNETCORE_URLS="http://+:${BACKEND_PORT}"
```

### Compose-Datei Updates

Das Port-Management aktualisiert automatisch:

```yaml
services:
  backend:
    ports:
      - "${BACKEND_PORT}:${BACKEND_PORT}"
```

## Troubleshooting

### Häufige Probleme

1. **Port bereits belegt**
   ```powershell
   # Prüfen welcher Prozess den Port nutzt
   netstat -ano | findstr :8081
   ```

2. **Container startet nicht**
   ```bash
   # Logs prüfen
   docker-compose logs backend
   ```

3. **Port-Updates nicht übernommen**
   ```powershell
   # Services force-restart
   .\port-management.ps1 -RestartServices -Force
   ```

### Logs und Debugging

```powershell
# Dry-Run Modus
.\port-management.ps1 -DryRun

# Verbose Output
.\port-management.ps1 -Verbose

# Nur Validation
.\port-management.ps1 -Validate -Verbose
```

## Best Practices

1. **Immer via Skript ändern** - Niemals docker-compose.yaml direkt bearbeiten
2. **Testing-Ports verwenden** - Separate Ports für Development/Testing
3. **Dokumentation aktuell halten** - Bei neuen Services auch ports.config erweitern
4. **Backup vor Änderungen** - Automatisch durch Skript

## Integration mit Migration System

Das Port-Management arbeitet eng mit dem Migration-System zusammen:

- **Database Container** - Migration nutzt Dynamic Port Resolution
- **Backend Container** - EF Tools über konfigurierte Ports
- **Health Checks** - Verwenden zentral konfigurierte Ports

Für Migration-Management siehe: `../migration/README.md`

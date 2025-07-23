# PowerShell Scripts

Dieser Ordner enthält alle PowerShell-Skripte für das UGH-Projekt.

**Cross-Platform:** Alle Skripte laufen auf Windows, Linux und macOS mit PowerShell Core.

## Installation PowerShell (Linux/macOS)
```bash
# Ubuntu/Debian
sudo apt-get install -y powershell

# CentOS/RHEL  
sudo yum install -y powershell

# macOS
brew install powershell
```

## Aufruf der Skripte

**Windows:**
```powershell
.\script-name.ps1 [Parameter]
```

**Linux/macOS:**
```bash
pwsh ./script-name.ps1 [Parameter]
```

## Verfügbare Skripte

### 📊 update-ports.ps1
**Zweck:** Automatisierte Port-Konfiguration für alle Projektdateien

```powershell
.\update-ports.ps1 [-DryRun] [-Verbose] [-Help]
```

**Parameter:**
- `-DryRun`: Zeigt nur an, welche Änderungen gemacht würden
- `-Verbose`: Ausführliche Ausgabe
- `-Help`: Zeigt Hilfe an

**Beispiel:**
```powershell
.\update-ports.ps1 -DryRun -Verbose
```

---

### 🗄️ migrate-db.ps1
**Zweck:** Datenbank-Migrations-Management

```powershell
.\migrate-db.ps1 [Action] [-Force] [-SkipValidation]
```

**Aktionen:**
- `help`: Zeigt Hilfe an (Standard)
- `migrate`: Führt Migrationen aus
- `reset`: Setzt Datenbank zurück

**Beispiel:**
```powershell
.\migrate-db.ps1 migrate
```

---

### 🔐 secure-admin-setup.ps1
**Zweck:** Sicherer Admin-Passwort Reset für Notfälle

```powershell
.\secure-admin-setup.ps1 -NewPassword '<password>' [-Help]
```

**Parameter:**
- `-NewPassword`: Das neue Admin-Passwort (Pflichtfeld)
- `-Help`: Zeigt Hilfe an

**Beispiel:**
```powershell
.\secure-admin-setup.ps1 -NewPassword "MeinNeuesPasswort123!"
```

---

### 🔐 generate-admin-hash.ps1
**Zweck:** Generiert Passwort-Hash für Admin-Accounts

```powershell
.\generate-admin-hash.ps1
```

Interaktives Skript ohne Parameter. Folgt den Anweisungen auf dem Bildschirm.

---

## Hilfe aufrufen

Für Skripte mit Parametern:
- Verwende den `-Help` Parameter
- Oder rufe das Skript ohne Parameter auf

Beispiele:
```powershell
.\update-ports.ps1 -Help
.\secure-admin-setup.ps1 -Help
.\migrate-db.ps1 help
```

## Ausführungsrichtlinien

Falls Skripte nicht ausgeführt werden können:

```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

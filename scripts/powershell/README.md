# PowerShell Scripts

Dieser Ordner enthält alle PowerShell-Skripte für das UGH-Projekt.

**Cross-Platform:** Alle Skripte laufen auf Windows, Linux und macOS mit PowerShell Core.

## ⚠ Migration zu Docs/

**Wichtig**: Viele Skripte wurden nach `Docs/` verschoben für bessere Organisation:

- **Migration Management**: Siehe `Docs/migration/README.md`
- **Port Management**: Siehe `Docs/infrastructure/README.md`

## Installation PowerShell (Linux/macOS)
[Link](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux?view=powershell-7.5)

## Aufruf der Skripte

**Windows:**
```powershell
.\script-name.ps1 [Parameter]
```

**Linux/macOS:**
```bash
pwsh -File ./script-name.ps1 [Parameter]
```
Wichtig: Unter Linux brauchen Interaktionen mit Dockercontainern root-Rechte. D.h. einige Scripte erfordern ein sudo.

## Verfügbare Skripte

### 📊 Admin & Security Tools
**Hinweis:** Port-Management wurde nach `Docs/infrastructure/` verschoben

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

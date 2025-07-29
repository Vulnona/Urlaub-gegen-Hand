# 🔐 Admin Security & Two-Factor Authentication (2FA) Guide

> **Wichtig**: Diese Dokumentation beschreibt die vollständige Sicherheitsimplementierung für UGH, 
> einschließlich Admin-Sicherheit und Zwei-Faktor-Authentifizierung.

## 📋 Inhaltsverzeichnis

1. [Admin Security](#admin-security)
2. [Two-Factor Authentication (2FA)](#two-factor-authentication-2fa)
3. [Sicherheitsrichtlinien](#sicherheitsrichtlinien)
4. [Notfallverfahren](#notfallverfahren)
5. [Technische Implementierung](#technische-implementierung)

---

## 🔑 Admin Security

### Übersicht
Die UGH-Plattform implementiert umfassende Sicherheitsmaßnahmen für Admin-Accounts, einschließlich erzwungener 2FA und sicherer Passwort-Verwaltung.

### ✅ Implementierte Sicherheitsfeatures

#### 1. **Erzwungene 2FA für Admins**
- **Status**: ✅ Vollständig implementiert
- **Feature**: Admin-Accounts müssen 2FA aktiviert haben
- **Backend-Validierung**: Middleware prüft 2FA-Status bei jedem Request
- **Recovery-Flow**: Backup-Code-Login mit optionaler 2FA-Reset

#### 2. **Sichere Passwort-Verwaltung**
- **Status**: ✅ Implementiert
- **Algorithmus**: PBKDF2 mit HMAC-SHA256, 10.000 Iterationen
- **Salt**: 32-Byte kryptographisch sicherer Salt
- **Speicherung**: Gehashed in Datenbank

#### 3. **Emergency Password Reset**
- **Status**: ✅ Implementiert
- **Feature**: PowerShell-Skript für Notfall-Passwort-Reset
- **Sicherheit**: Token-basierte Authentifizierung
- **Skript**: `scripts/powershell/secure-admin-setup.ps1`

### Für Produktionsumgebungen

#### 1. **Starke Passwörter verwenden**
```bash
# Beispiel für sicheres Passwort (mindestens 16 Zeichen)
$SecurePassword = "cRTtT~ZFfF2!nFyQ"
```

#### 2. **Umgebungsvariablen für Secrets**
```yaml
# docker-compose.yml
environment:
  - ADMIN_INITIAL_PASSWORD_FILE=/run/secrets/admin_password

secrets:
  admin_password:
    file: ./secrets/admin-password.txt
```

#### 3. **Sichere Passwort-Zurücksetzung**
```powershell
# Nur im Notfall verwenden:
.\scripts\powershell\secure-admin-setup.ps1 -NewPassword "NewSecurePassword123!"
```

### Für Entwicklungsumgebungen

#### Option 1: Automatischer Reset via API (empfohlen)
```powershell
# Verwendet die Backend-API für sicheren Passwort-Reset
.\scripts\powershell\secure-admin-setup.ps1 -NewPassword "DevPassword123"
```

#### Option 2: Manueller Hash für Docker MySQL
```powershell
# 1. Hash generieren
.\scripts\powershell\generate-admin-hash.ps1

# 2. SQL in Docker Container ausführen
docker exec -i ugh-db mysql -u root -p ugh_db
# Dann das UPDATE SQL-Statement eingeben
```

---

## 🔐 Two-Factor Authentication (2FA)

### Übersicht
Die UGH-Plattform implementiert eine vollständige Zwei-Faktor-Authentifizierung (2FA) basierend auf dem Time-based One-Time Password (TOTP) Standard.

### ✅ Implementierte Features

#### 1. **TOTP-basierte 2FA**
- Kompatibel mit Google Authenticator, Authy, Microsoft Authenticator
- 30-Sekunden-Zeitfenster mit ±30 Sekunden Toleranz
- Base32-kodierte Secrets für maximale Kompatibilität

#### 2. **QR-Code-Generation**
- Automatische QR-Code-Erstellung für einfache App-Integration
- Manuelle Secret-Eingabe als Fallback-Option
- Sichere QR-Code-URLs mit Issuer-Information

#### 3. **Backup-Codes**
- 10 eindeutige Backup-Codes pro Benutzer
- Einmalige Verwendung pro Code
- Sichere Speicherung als JSON in der Datenbank
- Download- und Kopierfunktionen

#### 4. **Admin-spezifische Features**
- **Erzwungene 2FA**: Admin-Accounts müssen 2FA aktiviert haben
- **Recovery-Flow**: Backup-Code-Login mit optionaler 2FA-Reset
- **Setup-Enforcement**: Automatische Weiterleitung zum 2FA-Setup

### Benutzer-Workflow

#### 2FA Aktivierung
1. **Benutzer** navigiert zu Profileinstellungen
2. **System** zeigt 2FA-Management-Komponente
3. **Benutzer** klickt "2FA einrichten"
4. **System** generiert Secret und QR-Code
5. **Benutzer** scannt QR-Code mit Authenticator App
6. **Benutzer** gibt Verifizierungscode ein
7. **System** aktiviert 2FA und zeigt Backup-Codes
8. **Benutzer** speichert Backup-Codes sicher

#### Login mit 2FA
1. **Benutzer** gibt Email/Passwort ein
2. **System** erkennt 2FA-Aktivierung
3. **System** zeigt 2FA-Login-Komponente
4. **Benutzer** gibt TOTP-Code oder Backup-Code ein
5. **System** verifiziert Code und gewährt Zugang

#### Backup-Code Recovery Flow
1. **Admin** verwendet Backup-Code für Login
2. **System** zeigt Warnung und Optionen
3. **Admin** wählt "Einfach fortfahren" oder "Passwort ändern"
4. **Bei "Einfach fortfahren"**: 2FA bleibt aktiv
5. **Bei "Passwort ändern"**: 2FA wird zurückgesetzt, Setup erforderlich

### Technische Implementierung

#### Backend-Komponenten

##### 1. TwoFactorAuthService (`Backend/Services/TwoFactorAuthService.cs`)
```csharp
// Hauptservice für 2FA-Funktionalität
- GenerateSecret(): Erstellt kryptographisch sichere Secrets
- GenerateQrCode(): Erstellt QR-Codes für Authenticator-Apps
- ValidateCode(): Verifiziert TOTP-Codes mit Zeittoleranz
- GenerateBackupCodes(): Erstellt sichere Backup-Codes
- ValidateBackupCodeWithMessage(): Prüft und gibt spezifische Fehlermeldungen
```

##### 2. AuthController 2FA-Endpoints
```
POST /api/authenticate/2fa/setup
POST /api/authenticate/2fa/verify-setup
POST /api/authenticate/login-2fa
GET  /api/authenticate/2fa/status
POST /api/authenticate/2fa/disable
```

##### 3. Datenbank-Schema (User-Tabelle)
```sql
ALTER TABLE users ADD COLUMN IsTwoFactorEnabled BOOLEAN DEFAULT FALSE;
ALTER TABLE users ADD COLUMN TwoFactorSecret VARCHAR(255) NULL;
ALTER TABLE users ADD COLUMN BackupCodes TEXT NULL;
```

#### Frontend-Komponenten

##### 1. TwoFactorSetup.vue
- **Zweck**: Vollständiger Setup-Prozess für 2FA
- **Features**: QR-Code-Anzeige, manuelle Secret-Eingabe, Backup-Codes
- **Sicherheit**: Code-Verifizierung vor Aktivierung
- **Admin-Support**: Recovery-Modus für Admin-Accounts

##### 2. AccountLogin.vue
- **Zweck**: 2FA-Verifizierung während Login-Prozess
- **Features**: TOTP-Code-Eingabe, Backup-Code-Option
- **UX**: Intuitive Eingabefelder mit Validierung
- **Admin-Flow**: Automatische Weiterleitung zum 2FA-Setup

##### 3. ChangePassword.vue
- **Zweck**: Passwort-Änderung mit 2FA-Reset-Option
- **Features**: Backup-Code-Initiated Password Change
- **2FA-Reset**: Automatischer Reset bei Backup-Code-Password-Change

### Frontend-Dependencies

#### package.json Ergänzungen
```json
{
  "dependencies": {
    "qrcode": "^1.5.3",           // QR-Code-Generierung
    "qrcode-vue3": "^1.6.8"       // Vue 3 QR-Code-Komponente
  },
  "devDependencies": {
    "@types/qrcode": "^1.5.5"     // TypeScript-Unterstützung
  }
}
```

---

## 🛡️ Sicherheitsrichtlinien


### Verschlüsselung und Speicherung

- **Secrets**: Base32-kodiert, kryptographisch sicher generiert
- **Backup-Codes**: Als JSON gehashed und in Datenbank gespeichert
- **Zeittoleranz**: ±30 Sekunden für Netzwerk-/Uhren-Abweichungen

### Angriffsvektoren und Schutz

- **Replay-Angriffe**: Verhindert durch Zeitfenster-Validierung
- **Brute-Force**: Rate-Limiting auf 2FA-Endpoints
- **Backup-Code-Missbrauch**: Einmalige Verwendung, sichere Speicherung

### Compliance und Standards

- **RFC 6238**: TOTP-Standard-Konformität
- **RFC 4648**: Base32-Encoding nach Standard
- **OWASP**: Folgt 2FA-Sicherheitsrichtlinien

---

## 🚨 Notfallverfahren

### Admin-Zugang verloren

#### 1. **Check Setup Status:**
```bash
curl http://localhost:8081/api/admin-setup/status
```

#### 2. **Emergency Reset:**
```powershell
.\scripts\powershell\secure-admin-setup.ps1 -NewPassword "EmergencyPassword123!"
```

#### 3. **Verify Login:**
```bash
curl -X POST http://localhost:8081/api/authenticate/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com","password":"EmergencyPassword123!"}'
```

### 2FA-Probleme

#### Häufige Probleme und Lösungen

1. **"Code ungültig"**: Zeitabweichung zwischen Geräten
   - **Lösung**: Zeitsynchronisation prüfen

2. **"QR-Code nicht lesbar"**: Manuelle Secret-Eingabe verwenden
   - **Lösung**: Secret manuell in Authenticator-App eingeben

3. **"Backup-Codes funktionieren nicht"**: Bereits verwendete Codes
   - **Lösung**: Neue Backup-Codes generieren

4. **Admin kann sich nicht einloggen**: 2FA nicht aktiviert
   - **Lösung**: Backup-Code verwenden oder Emergency Reset

### Backup-Code Recovery

#### Aktuelle Backup-Codes prüfen:
```bash
docker exec -it ugh-db mysql -u root -ppassword -e "USE db; SELECT Email_Address, BackupCodes FROM users WHERE Email_Address = 'admin@example.com';"
```

#### Backup-Codes verwenden:
1. Login mit Email/Passwort
2. Bei 2FA-Prompt: "Backup-Code verwenden" wählen
3. Einen der gültigen Backup-Codes eingeben
4. "Einfach fortfahren" oder "Passwort ändern" wählen

---

## 🔧 Technische Implementierung

### Installation und Konfiguration

#### Backend-Setup
```bash
# NuGet-Packages installieren
dotnet add package Otp.NET --version 1.3.0
dotnet add package QRCoder --version 1.4.3

# Migration anwenden
dotnet ef database update
```

#### Frontend-Setup
```bash
# NPM-Packages installieren
npm install qrcode@^1.5.3 qrcode-vue3@^1.6.8
npm install --save-dev @types/qrcode@^1.5.5
```

#### Service-Registrierung
```csharp
// Program.cs
builder.Services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();
```

### Testing und Validierung

#### Empfohlene Test-Apps
- **Google Authenticator** (iOS/Android)
- **Microsoft Authenticator** (iOS/Android)
- **Authy** (iOS/Android/Desktop)

#### Test-Szenarien
1. **Setup-Prozess**: QR-Code-Scan und manuelle Eingabe
2. **Login-Flow**: Normale und Backup-Code-Anmeldung
3. **Backup-Management**: Regenerierung und Verwendung
4. **Deaktivierung**: Passwort-Bestätigung und Cleanup
5. **Admin-Recovery**: Backup-Code-Login mit 2FA-Reset

### Wartung und Monitoring

#### Metriken
- 2FA-Aktivierungsrate
- Backup-Code-Verwendung
- Fehlgeschlagene 2FA-Versuche

#### Logs
- 2FA-Setup-Ereignisse
- Login-Versuche mit 2FA
- Backup-Code-Verwendung

---

## Roadmap und Erweiterungen

### Geplante Features
- **Hardware-Token**: FIDO2/WebAuthn-Unterstützung
- **Session-Management**: 2FA-remember für vertrauenswürdige Geräte

### Verbesserungen
- **Geolocation**: Standort-basierte 2FA-Anforderungen
- **Rate-Limiting**: Erweiterte Schutzmaßnahmen
- **Audit Logging**: Detaillierte Protokollierung aller Sicherheitsereignisse

---

## Support und Troubleshooting

### Häufige Probleme

1. **Admin kann sich nicht einloggen**
   - Prüfen Sie 2FA-Status in der Datenbank
   - Verwenden Sie Backup-Codes für Recovery
   - Emergency Reset als letzte Option

2. **Backup-Codes funktionieren nicht**
   - Prüfen Sie aktuelle Codes in der Datenbank
   - Verwenden Sie nur unbenutzte Codes
   - Generieren Sie neue Codes bei Bedarf

3. **2FA-Setup schlägt fehl**
   - Prüfen Sie Zeit-Synchronisation
   - Testen Sie alternative Authenticator-Apps
   - Verwenden Sie manuelle Secret-Eingabe

### Lösungsansätze
- Zeitsynchronisation prüfen
- Alternative Authenticator-Apps testen
- Neue Backup-Codes generieren
- Emergency Reset als Notfall-Option

---

**Wichtig:** Diese Sicherheitsimplementierung ist für Produktionsumgebungen konzipiert und sollte regelmäßig überprüft und aktualisiert werden. 
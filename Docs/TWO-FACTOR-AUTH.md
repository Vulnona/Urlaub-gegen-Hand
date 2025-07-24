# Zwei-Faktor-Authentifizierung (2FA) - Implementierungshandbuch

## Übersicht

Die UGH-Plattform implementiert eine vollständige Zwei-Faktor-Authentifizierung (2FA) basierend auf dem Time-based One-Time Password (TOTP) Standard. Diese Implementierung bietet eine zusätzliche Sicherheitsschicht für alle Benutzerkonten.

## Sicherheitsfeatures

### ✅ Implementierte Features

1. **TOTP-basierte 2FA**
   - Kompatibel mit Google Authenticator, Authy, Microsoft Authenticator
   - 30-Sekunden-Zeitfenster mit ±30 Sekunden Toleranz
   - Base32-kodierte Secrets für maximale Kompatibilität

2. **QR-Code-Generation**
   - Automatische QR-Code-Erstellung für einfache App-Integration
   - Manuelle Secret-Eingabe als Fallback-Option
   - Sichere QR-Code-URLs mit Issuer-Information

3. **Backup-Codes**
   - 10 eindeutige Backup-Codes pro Benutzer
   - Einmalige Verwendung pro Code
   - Sichere Speicherung als JSON in der Datenbank
   - Download- und Kopierfunktionen

4. **Optionale Aktivierung**
   - 2FA ist für alle Benutzer (einschließlich Admins) optional
   - Benutzer können 2FA jederzeit aktivieren oder deaktivieren
   - Passwort-Bestätigung erforderlich zum Deaktivieren

5. **Admin-Password-Reset**
   - Spezieller Reset-Button im Admin-Panel
   - Verwendet bestehende Infrastruktur für E-Mail-Versand
   - Sichere Token-basierte Passwort-Zurücksetzung

## Technische Implementierung

### Backend-Komponenten

#### 1. TwoFactorAuthService (`Backend/Services/TwoFactorAuthService.cs`)
```csharp
// Hauptservice für 2FA-Funktionalität
- GenerateSecret(): Erstellt kryptographisch sichere Secrets
- GenerateQrCode(): Erstellt QR-Codes für Authenticator-Apps
- ValidateCode(): Verifiziert TOTP-Codes mit Zeittoleranz
- GenerateBackupCodes(): Erstellt sichere Backup-Codes
- ValidateBackupCode(): Prüft und entfernt verwendete Backup-Codes
```

#### 2. AuthController 2FA-Endpoints
```
POST /api/authenticate/setup-2fa
POST /api/authenticate/verify-2fa-setup
POST /api/authenticate/login-with-2fa
GET  /api/authenticate/2fa-status
GET  /api/authenticate/backup-codes
POST /api/authenticate/regenerate-backup-codes
POST /api/authenticate/disable-2fa
```

#### 3. Datenbank-Schema (User-Tabelle)
```sql
ALTER TABLE users ADD COLUMN IsTwoFactorEnabled BOOLEAN DEFAULT FALSE;
ALTER TABLE users ADD COLUMN TwoFactorSecret VARCHAR(255) NULL;
ALTER TABLE users ADD COLUMN BackupCodes TEXT NULL;
```

### Frontend-Komponenten

#### 1. TwoFactorSetup.vue
- **Zweck**: Vollständiger 3-Schritt-Setup-Prozess
- **Features**: QR-Code-Anzeige, manuelle Secret-Eingabe, Backup-Codes
- **Sicherheit**: Code-Verifizierung vor Aktivierung

#### 2. TwoFactorLogin.vue
- **Zweck**: 2FA-Verifizierung während Login-Prozess
- **Features**: TOTP-Code-Eingabe, Backup-Code-Option
- **UX**: Intuitive Eingabefelder mit Validierung

#### 3. TwoFactorManagement.vue
- **Zweck**: Verwaltung der 2FA-Einstellungen
- **Features**: Status-Anzeige, Backup-Code-Management, Deaktivierung
- **Admin**: Vollständige Kontrolle über 2FA-Features

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

## Benutzer-Workflow

### 2FA Aktivierung
1. **Benutzer** navigiert zu Profileinstellungen
2. **System** zeigt 2FA-Management-Komponente
3. **Benutzer** klickt "2FA einrichten"
4. **System** generiert Secret und QR-Code
5. **Benutzer** scannt QR-Code mit Authenticator App
6. **Benutzer** gibt Verifizierungscode ein
7. **System** aktiviert 2FA und zeigt Backup-Codes
8. **Benutzer** speichert Backup-Codes sicher

### Login mit 2FA
1. **Benutzer** gibt Email/Passwort ein
2. **System** erkennt 2FA-Aktivierung
3. **System** zeigt 2FA-Login-Komponente
4. **Benutzer** gibt TOTP-Code oder Backup-Code ein
5. **System** verifiziert Code und gewährt Zugang

### Admin-Password-Reset
1. **Admin** klickt "Reset Admin Password" im Admin-Panel
2. **System** zeigt Email-Eingabe-Dialog
3. **Admin** gibt Email-Adresse ein
4. **System** sendet Reset-Link per Email
5. **Admin** folgt Link und setzt neues Passwort

## Sicherheitsüberlegungen

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

## Installation und Konfiguration

### Backend-Setup
```bash
# NuGet-Packages installieren
dotnet add package Otp.NET --version 1.3.0
dotnet add package QRCoder --version 1.4.3

# Migration anwenden
dotnet ef database update
```

### Frontend-Setup
```bash
# NPM-Packages installieren
npm install qrcode@^1.5.3 qrcode-vue3@^1.6.8
npm install --save-dev @types/qrcode@^1.5.5
```

### Service-Registrierung
```csharp
// Program.cs
builder.Services.AddScoped<ITwoFactorAuthService, TwoFactorAuthService>();
```

## Testing und Validierung

### Empfohlene Test-Apps
- **Google Authenticator** (iOS/Android)
- **Microsoft Authenticator** (iOS/Android)
- **Authy** (iOS/Android/Desktop)

### Test-Szenarien
1. **Setup-Prozess**: QR-Code-Scan und manuelle Eingabe
2. **Login-Flow**: Normale und Backup-Code-Anmeldung
3. **Backup-Management**: Regenerierung und Verwendung
4. **Deaktivierung**: Passwort-Bestätigung und Cleanup

## Wartung und Monitoring

### Metriken
- 2FA-Aktivierungsrate
- Backup-Code-Verwendung
- Fehlgeschlagene 2FA-Versuche

### Logs
- 2FA-Setup-Ereignisse
- Login-Versuche mit 2FA
- Backup-Code-Verwendung

## Roadmap und Erweiterungen

### Geplante Features
- **SMS-2FA**: Alternative für Benutzer ohne Smartphone
- **Push-Notifications**: App-basierte Bestätigung
- **Hardware-Token**: FIDO2/WebAuthn-Unterstützung
- **Admin-Enforcement**: Erzwungene 2FA für Admin-Accounts

### Verbesserungen
- **Session-Management**: 2FA-remember für vertrauenswürdige Geräte
- **Geolocation**: Standort-basierte 2FA-Anforderungen
- **Rate-Limiting**: Erweiterte Schutzmaßnahmen

## Support und Troubleshooting

### Häufige Probleme
1. **"Code ungültig"**: Zeitabweichung zwischen Geräten
2. **"QR-Code nicht lesbar"**: Manuelle Secret-Eingabe verwenden
3. **"Backup-Codes funktionieren nicht"**: Bereits verwendete Codes

### Lösungsansätze
- Zeitsynchronisation prüfen
- Alternative Authenticator-Apps testen
- Neue Backup-Codes generieren

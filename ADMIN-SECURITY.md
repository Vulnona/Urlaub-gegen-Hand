# 🔐 Admin Password Security Guide


## Für Produktionsumgebungen

### 1. **Verwenden Sie starke Passwörter**
```bash
# Beispiel für sicheres Passwort (mindestens 16 Zeichen)
$SecurePassword = "cRTtT~ZFfF2!nFyQ"
```

### 2. **Umgebungsvariablen für Secrets**
```yaml
# docker-compose.yml
environment:
  - ADMIN_INITIAL_PASSWORD_FILE=/run/secrets/admin_password

secrets:
  admin_password:
    file: ./secrets/admin-password.txt
```

### 3. **Sichere Passwort-Zurücksetzung**
```powershell
# Nur im Notfall verwenden:
.\secure-admin-setup.ps1 -NewPassword "NewSecurePassword123!"
```

## Für Entwicklungsumgebungen

### Option 1: Manuelles Setup (empfohlen)
```sql
-- 1. Generieren Sie Hash mit dem PowerShell-Tool
.\generate-admin-hash.ps1

-- 2. Führen Sie das SQL-Update aus
UPDATE users SET 
  Password = 'GENERATED_HASH', 
  SaltKey = 'GENERATED_SALT' 
WHERE Email_Address = 'admin@gmail.com';
```

### Option 2: Sicherer Reset-Endpoint
```powershell
# Mit temporärem Token
.\secure-admin-setup.ps1 -NewPassword "DevPassword123"
```

## ⚠️ Sicherheitsregeln

1. **NIE hardcoded Passwörter im Code**
2. **Verwenden von Umgebungsvariablen oder Secrets**
3. **Implementieren von Passwort-Rotation**
4. **Loggen aller Admin-Zugriffe**
5. **2FA in Produktion**

## Next Steps für Produktion

1. **OAuth/OIDC Integration** (Google, Microsoft, etc.)
2. **Role-based Access Control (RBAC)**
3. **Session Management** mit Redis
4. **Audit Logging** für alle Admin-Aktionen
5. **Password Policies** (Complexity, Expiration)

## Emergency Procedures

Wenn Sie den Admin-Zugang verlieren:

1. **Check Setup Status:**
   ```bash
   curl http://localhost:8080/api/admin-setup/status
   ```

2. **Emergency Reset:**
   ```powershell
   .\secure-admin-setup.ps1 -NewPassword "EmergencyPassword123!"
   ```

3. **Verify Login:**
   ```bash
   curl -X POST http://localhost:8080/api/authenticate/login \
     -H "Content-Type: application/json" \
     -d '{"email":"admin@gmail.com","password":"EmergencyPassword123!"}'
   ```

---

**Wichtig:** Diese Lösung ist nur für Development!

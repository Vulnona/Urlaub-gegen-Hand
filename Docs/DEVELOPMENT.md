# Development Guide

Entwicklerleitfaden für das UGH-Projekt mit allen wichtigen Informationen für Setup, Entwicklung und Deployment.

## Quick Start

### Ersteinrichtung
```powershell
# 1. Repository klonen
git clone https://github.com/Urlaub-gegen-Hand/UGH.git
cd UGH

# 2. Container starten (vollautomatische Migration & 2FA-Setup)
docker-compose up -d

# 3. Backend-Entwicklungsumgebung einrichten (einmalig)
.\scripts\setup-backend-dev.ps1
```

### Tägliche Entwicklung
```powershell
# System starten
docker-compose up -d

# Migration-Status prüfen
.\scripts\migration\enhanced-migration.ps1 status

# Entwicklung starten (Frontend Hot-Reload)
cd Frontend-Vuetify
npm run dev
```

## 📁 Projektstruktur

```
UGH/
├── Frontend-Vuetify/          # Vue.js Frontend (Vuetify UI)
├── Backend/                   # .NET 7.0 Web API
│   ├── Controllers/           # API Controllers
│   ├── Models/               # Entity Models
│   ├── Migrations/           # EF Core Migrations
│   └── Services/             # Business Logic
├── scripts/                  # PowerShell Automation Tools
│   ├── migration/            # Migration Management
│   ├── infrastructure/       # Port & Service Management
│   └── database/            # Database Tools
├── docs/                     # Projektdokumentation
└── docker-compose.yaml      # Container-Orchestrierung
```

## 🛠️ Entwicklungstools

### Migration Management
# sichere Migrationen durchführen
.\scripts\migration\migration.ps1


### Database Tools

Die MySQL-Zugangsdaten werden automatisch und plattformübergreifend aus der `compose.yaml` und den Secret-Dateien gelesen – dank des Node.js-Hilfsskripts `scripts/get-mysql-creds.js`.

```powershell
# Datenbank-Shell öffnen
.\scripts\database\database-access.ps1

# Backup erstellen
.\scripts\database\database-dump.ps1

# Backup wiederherstellen
.\scripts\database\database-restore.ps1 -BackupFile "backup_2025-07-23.sql"
```
#  Datenbank-Reset

Das Skript `scripts/resetdb.ps1` setzt die Datenbank sicher zurück und erstellt vorher ein Backup

**Verwendung:**
```powershell
cd scripts
./resetdb.ps1
```

### Infrastructure Management
```powershell
# Port-Status prüfen
.\scripts\infrastructure\port-management.ps1 status

# Services neustarten
.\scripts\infrastructure\port-management.ps1 restart
```

## Docker Environment

### Services

| Service | Container | Port | URL |
|---------|-----------|------|-----|
| Frontend | `ugh-frontend` | 3002 | http://localhost:3002 |
| Backend | `ugh-backend` | 8081 | http://localhost:8081 |
| Nginx | `ugh-webserver` | 81 | http://localhost:81 |
| MySQL | `ugh-db` | 3306 | localhost:3306 |

### Container-Management
```powershell
# Alle Services starten
docker-compose up -d

# Services anzeigen
docker-compose ps

# Logs anzeigen
docker-compose logs -f [service-name]

# Services stoppen
docker-compose down
```

## Konfiguration

### Environment Variables
```yaml
# Backend/.env
DATABASE_URL=Server=ugh-db;Database=db;User=root;Password=${DB_PASSWORD};
JWT_SECRET=${JWT_SECRET}
```

### Port-Konfiguration
Zentrale Port-Verwaltung in `scripts/infrastructure/ports.config`:
```
FRONTEND_PORT=3002
BACKEND_PORT=8081
NGINX_PORT=81
MYSQL_PORT=3306
```

## Testing

### Backend Tests
```powershell
# Unit Tests ausführen
cd Backend
dotnet test

# Mit Coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Frontend Tests
```powershell
# Unit Tests
cd Frontend-Vuetify
npm run test:unit

# E2E Tests
npm run test:e2e
```

### API Testing

### Login Process
1. **POST** `/api/login` - User login
   - Returns JWT token with user information
   - Contains `MembershipStatus` claim

2. **POST** `/api/login-2fa` - Two-factor authentication login
   - Required for Admin users
   - Optional for regular users

### User Verification via API

#### Endpoint: POST /api/login
```json
{
  "email": "test@example.com",
  "password": "TestPassword123!"
}
```

#### Response:
```json
{
  "token": "eyJ...",
  "user": {
    "email": "test@example.com",
    "membershipStatus": "Active",
    "roles": ["User"]
  }
}
```

## Coupon System

### Admin Endpoints (Admin Role Required)

#### Create Coupon
- **POST** `/api/coupon/add-coupon`
- Body: `{ "membershipId": 1 }`
- Creates a new coupon for active membership

#### Send Coupon
- **POST** `/api/coupon/send-coupon`
- Sends coupon via email

#### Get All Coupons
- **GET** `/api/coupon/get-all-coupon`
- Returns paginated list of all coupons

#### Delete Coupon
- **DELETE** `/api/coupon/delete-coupon/{couponId}`

### User Endpoints

#### Redeem Coupon
- **POST** `/api/coupon/redeem`
- Body: `{ "couponCode": "XXXXX" }`
- Activates membership for user

## Testing New Account + Coupon Flow

### Step 1: Create Test User
```bash
# Register new user via Frontend or API
POST /api/register
{
  "email": "newuser@example.com",
  "password": "TestPassword123!",
  "firstName": "Test",
  "lastName": "User"
}
```

### Step 2: Verify User (Admin Action)
```bash
# Admin creates and sends coupon
POST /api/coupon/add-coupon
Authorization: Bearer {admin_token}
Content-Type: application/json
{
  "membershipId": 1
}
```

### Step 3: User Redeems Coupon
```bash
# User redeems coupon to activate membership
POST /api/coupon/redeem
Authorization: Bearer {user_token}
Content-Type: application/json
{
  "couponCode": "GENERATED_CODE"
}
```

### Step 4: Verify Active Membership
```bash
# User logs in again to get updated token
POST /api/login
{
  "email": "newuser@example.com",
  "password": "TestPassword123!"
}

# Response should show "membershipStatus": "Active"
```

## Important Notes

- **Timezone**: System uses German local time (`DateTime.Now`)
- **Admin 2FA**: Mandatory for Admin users
- **User 2FA**: Optional for regular users  
- **Coupon Duration**: Based on membership type (1-3 years or lifetime)
- **Debug Logs**: Enabled in UserRepository for troubleshooting

## Current Test User
- **Email**: `test@example.com`
- **Password**: `TestPassword123!`
- **Status**: ✅ Active Membership
- **JWT**: Contains correct `MembershipStatus: "Active"`


## 📦 Deployment

### Production Build
```powershell
# Backend Build
cd Backend
dotnet publish -c Release

# Frontend Build
cd Frontend-Vuetify
npm run build
```

### Docker Production
```powershell
# Production Container erstellen
docker-compose -f docker-compose.prod.yml up -d
```

## 🔐 Security

### Admin Setup
```powershell
# Sicheres Admin-Setup
.\secure-admin-setup.ps1

# Admin-Passwort zurücksetzen
.\scripts\database\database-access.ps1
# Dann: SOURCE reset-admin-password.sql
```

### 2FA Configuration
Automatisch konfiguriert beim ersten Container-Start. Details siehe [Admin Security Dokumentation](../ADMIN-SECURITY.md).

## 🐛 Debugging

### Backend Debugging
```powershell
# Development mit Debug-Modus
cd Backend
dotnet watch run --environment Development
```

### Database Debugging
```powershell
# EF Migrations Debug
dotnet ef migrations list --verbose

# SQL Query Logging aktivieren (appsettings.Development.json)
{
  "Logging": {
    "LogLevel": {
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  }
}
```

### Container Debugging
```powershell
# Container-Shell öffnen
docker exec -it ugh-backend bash
docker exec -it ugh-db mysql -u root -p

# Container-Logs
docker logs ugh-backend --follow
```

## 📋 Common Tasks

### Neue API Endpoint hinzufügen
1. Controller in `Backend/Controllers/` erstellen
2. Model in `Backend/Models/` definieren
3. Service in `Backend/Services/` implementieren
4. Migration erstellen falls DB-Änderungen nötig
5. Frontend-Integration in Vue.js

### Neue Database Entity
```powershell
# 1. Model in Backend/Models/ erstellen
# 2. DbContext aktualisieren
# 3. Migration generieren
.\scripts\migration\enhanced-migration.ps1 add -MigrationName "AddNewEntity"

# 4. Migration anwenden
.\scripts\migration\enhanced-migration.ps1 sync
```

### Frontend Component hinzufügen
```bash
# Vue Component erstellen
cd Frontend-Vuetify/src/components
# Component-Datei erstellen
# In Parent-Component importieren und registrieren
```

## 🆘 Troubleshooting

### Häufige Probleme

**Migration-Konflikte**
```powershell
# Orphan-Migrationen bereinigen
.\scripts\migration\enhanced-migration.ps1 orphans -Force

# Migration-Status prüfen
.\scripts\migration\enhanced-migration.ps1 status
```

**Port-Konflikte**
```powershell
# Port-Belegung prüfen
.\scripts\infrastructure\port-management.ps1 check

# Ports in ports.config ändern
# Services neustarten
docker-compose down && docker-compose up -d
```

**Container-Probleme**
```powershell
# Container vollständig zurücksetzen
docker-compose down -v
docker-compose up -d

# Images neu erstellen
docker-compose build --no-cache
```

## 📚 API Dokumentation

Die vollständige API-Dokumentation ist über Swagger UI verfügbar:
- **Development**: http://localhost:8081/swagger
- **Production**: http://your-domain/api/swagger

### Wichtige API-Endpunkte

#### Authentication & User Management

**User Registration**
```http
POST /api/authenticate/register
Content-Type: application/json

{
  "FirstName": "Test",
  "LastName": "User",
  "Email_Address": "test@example.com",
  "Password": "TestPassword123!",
  "DateOfBirth": "1990-01-01",
  "Gender": "M",
  "Latitude": 52.5200,
  "Longitude": 13.4050,
  "DisplayName": "Berlin, Deutschland",
  "City": "Berlin",
  "Country": "Deutschland",
  "CountryCode": "DE"
}
```

**User Login (Standard)**
```http
POST /api/authenticate/login
Content-Type: application/json

{
  "Email": "test@example.com",
  "Password": "TestPassword123!"
}

Response: {
  "success": true,
  "value": {
    "token": "jwt-token-here",
    "expires": "2025-07-24T15:00:00Z",
    "membershipStatus": "Active",
    "requires2FA": false
  }
}
```

**User Login (with 2FA)**
```http
POST /api/authenticate/login-2fa
Content-Type: application/json

{
  "Email": "admin@example.com",
  "Password": "AdminPassword123!",
  "TwoFactorCode": "123456"
}
```

#### 2FA Management

**Setup 2FA**
```http
POST /api/authenticate/setup-2fa
Authorization: Bearer {jwt-token}

Response: {
  "qrCodeDataUrl": "data:image/png;base64,...",
  "manualEntryKey": "ABCD1234EFGH5678",
  "backupCodes": ["12345678", "87654321", ...]
}
```

**Verify 2FA Setup**
```http
POST /api/authenticate/verify-2fa-setup
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
  "verificationCode": "123456"
}
```

**Disable 2FA**
```http
POST /api/authenticate/disable-2fa
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
  "password": "UserPassword123!",
  "verificationCode": "123456"
}
```

#### Coupon System

**Get Available Coupons**
```http
GET /api/coupons
Authorization: Bearer {jwt-token}

Response: {
  "success": true,
  "value": [
    {
      "id": 1,
      "code": "WELCOME2025",
      "name": "Welcome Bonus",
      "description": "30 days premium membership",
      "isActive": true,
      "membershipId": 1
    }
  ]
}
```

**Redeem Coupon**
```http
POST /api/coupons/redeem
Authorization: Bearer {jwt-token}
Content-Type: application/json

{
  "couponCode": "WELCOME2025"
}

Response: {
  "success": true,
  "value": {
    "message": "Coupon successfully redeemed",
    "newMembershipExpiry": "2025-08-23T12:00:00"
  }
}
```

**Create Admin Coupon**
```http
POST /api/coupons/create
Authorization: Bearer {admin-jwt-token}
Content-Type: application/json

{
  "code": "ADMIN-SPECIAL",
  "name": "Admin Special Offer",
  "description": "60 days premium access",
  "membershipId": 2,
  "duration": "TwoMonths"
}
```

#### Membership Management

**Get User Memberships**
```http
GET /api/memberships/user/{userId}
Authorization: Bearer {jwt-token}

Response: {
  "success": true,
  "value": [
    {
      "id": 1,
      "membershipId": 1,
      "startDate": "2025-07-24T00:00:00",
      "expiration": "2025-08-23T23:59:59",
      "status": "Active",
      "membership": {
        "name": "Basic Membership",
        "durationDays": 30
      }
    }
  ]
}
```

**Admin: Grant Membership**
```http
POST /api/admin/grant-membership
Authorization: Bearer {admin-jwt-token}
Content-Type: application/json

{
  "userId": "user-guid-here",
  "membershipId": 1,
  "durationDays": 30
}
```

#### User Profile

**Get User Profile**
```http
GET /api/profile/{userId}
Authorization: Bearer {jwt-token}

Response: {
  "success": true,
  "value": {
    "userId": "guid",
    "firstName": "Test",
    "lastName": "User",
    "email": "test@example.com",
    "membershipStatus": "Active",
    "has2FAEnabled": false,
    "location": {
      "city": "Berlin",
      "country": "Deutschland"
    }
  }
}
```

---

**Letzte Aktualisierung**: 2025-07-24

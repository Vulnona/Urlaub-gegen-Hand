# Migration System Documentation

## Overview
The UGH application uses a robust migration management system that handles Entity Framework migrations with automatic consistency checks and DSGVO-compliant data cleanup.

## Features

### 🔧 Migration Management
- **Status Check**: Verify migration consistency across database, code, and filesystem
- **Inconsistency Fixing**: Automatically resolve migration conflicts
- **Orphaned File Cleanup**: Remove unused migration files
- **Schema Validation**: Check for model vs database mismatches
- **Force Rebuild**: Complete system reset for extreme cases

### 🛡️ DSGVO Compliance
- **DeletedUserBackupCleanupService**: Automatic cleanup of expired user backups
- **Configurable Retention**: 30 days by default (configurable in appsettings.json)
- **Background Processing**: Runs every 24 hours
- **Proper Logging**: All cleanup activities are logged

## Usage

### Modern Cross-Platform Commands

**Universal Command (Works on all platforms):**
```bash
# Check migration status
./scripts/migration/migrate status

# Fix inconsistencies
./scripts/migration/migrate fix-inconsistencies

# Add new migration
./scripts/migration/migrate add-migration YourMigrationName

# Schema check
./scripts/migration/migrate schema-check

# Clean orphaned files
./scripts/migration/migrate clean-orphans

# Force rebuild (nuclear option)
./scripts/migration/migrate force-rebuild --force
```

**Legacy PowerShell (Windows only):**
```powershell
# Check migration status
.\scripts\migration\migration.ps1 -Action status

# Fix inconsistencies
.\scripts\migration\migration.ps1 -Action fix-inconsistencies

# Add new migration
.\scripts\migration\migration.ps1 -Action add-migration -MigrationName "YourMigrationName"
```

### Dry Run Mode
Add `-DryRun` to preview changes without applying them:
```powershell
.\scripts\migration\migration.ps1 -Action fix-inconsistencies -DryRun
```

## Container Requirements

The migration system requires:
- `ugh-db` container (MySQL database)
- `ugh-backend` container (ASP.NET Core application)

Start containers with:
```bash
docker compose up -d
```

## Cross-Platform Support

The migration system works on:
- **Windows**: PowerShell 5.1+ or PowerShell Core
- **Linux**: PowerShell Core (pwsh) - automatically detected and installed
- **macOS**: PowerShell Core (pwsh) - automatically detected and installed

**Modern Features:**
- **Universal Script**: `./scripts/migration/migrate` works on all platforms
- **Auto-Detection**: Automatically detects PowerShell availability
- **Smart Installation**: Provides installation instructions if PowerShell is missing
- **Container Validation**: Checks if required Docker containers are running
- **State-of-the-Art**: Uses modern PowerShell platform detection instead of legacy environment variables

**Prerequisites:**
- Docker and Docker Compose
- PowerShell Core (automatically detected and guided installation provided)

## DSGVO Settings

Configure retention periods in `Backend/appsettings.json`:

```json
{
  "DSGVOSettings": {
    "DeletedUserRetentionDays": 30
  }
}
```

## Migration History

### Current Migrations
- `20250725223915_InitialCreate` - Initial database schema
- `20250725231705_RemoveCountryFields` - Removed country fields
- `20250726154812_AddDeletedUserBackup` - Added DSGVO-compliant backup table
- `20250730110050_MakeOfferIdOptionalInReviews` - Made OfferId optional in reviews
- `20250730110521_MakeOfferIdOptionalInReviewsFinal` - Final review schema fix

### DeletedUserBackup Table
The `DeletedUserBackups` table stores user data for DSGVO compliance:
- `Id` - Primary key
- `UserId` - Original user ID
- `FirstName`, `LastName` - User names
- `Email` - User email
- `Skills`, `Hobbies` - User preferences
- `Address`, `Latitude`, `Longitude` - Location data
- `ProfilePicture` - Profile image URL
- `DeletedAt` - Deletion timestamp

## Troubleshooting

### Common Issues

1. **EF Tools Not Available**
   ```
   [ERROR] Run "dotnet tool restore" to make the "dotnet-ef" command available.
   ```
   **Solution**: The script now automatically installs EF tools in the container.

2. **Containers Not Running**
   ```
   [ERROR] Missing containers: ugh-db, ugh-backend
   ```
   **Solution**: Start containers with `docker compose up -d`

3. **Migration Inconsistencies**
   ```
   [INCONSISTENCIES FOUND] X issues detected
   ```
   **Solution**: Run `-Action fix-inconsistencies`

### Recovery Procedures

1. **Schema Mismatch**: Use `-Action schema-check` to detect and fix
2. **Broken Migrations**: Use `-Action force-rebuild` (nuclear option)
3. **Orphaned Files**: Use `-Action clean-orphans`

## Best Practices

1. **Always check status** before making changes
2. **Use dry-run mode** for previewing changes
3. **Backup database** before major operations
4. **Test migrations** in development first
5. **Monitor cleanup logs** for DSGVO compliance

## Evolution

- **V1**: Basic migration management
- **V2**: Added inconsistency detection
- **V3**: Added DSGVO compliance features
- **V4**: migration.ps1 (current - proven, reliable, battle-tested)

**Total Migrations**: 5
**Applied Migrations**: 5
**Last updated**: 2025-01-27 by Battle-Tested Migration Management System








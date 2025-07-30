# Migration System Documentation

## Overview

The UGH Migration System provides a comprehensive, cross-platform solution for managing Entity Framework Core migrations in Docker environments. It supports both Windows and Linux systems through a unified PowerShell-based approach.

## Features

- **Cross-Platform**: Works on Windows, Linux, and macOS
- **Docker Integration**: Fully integrated with Docker containers
- **Backup & Restore**: Automatic backup creation and restoration capabilities
- **Error Handling**: Robust error handling with MySQL warning suppression
- **Status Monitoring**: Real-time migration status and consistency checking

## Prerequisites

### Windows
- PowerShell 5.1 or higher
- Docker Desktop

### Linux/macOS
- PowerShell Core (pwsh)
- Docker

### Installation on Linux

```bash
# Ubuntu/Debian
sudo apt-get update
sudo apt-get install -y wget apt-transport-https software-properties-common
wget -q "https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb"
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y powershell

# CentOS/RHEL
sudo yum install -y powershell

# Or download from GitHub
# https://github.com/PowerShell/PowerShell/releases
```

## Usage

### Windows (PowerShell)

```powershell
# Show migration status
.\scripts\migration\migration.ps1 -Action status

# Add new migration
.\scripts\migration\migration.ps1 -Action add -MigrationName "AddNewFeature"

# Check for inconsistencies
.\scripts\migration\migration.ps1 -Action fix

# Clean orphaned files
.\scripts\migration\migration.ps1 -Action clean

# Schema consistency check
.\scripts\migration\migration.ps1 -Action schema-check

# Force rebuild (destructive!)
.\scripts\migration\migration.ps1 -Action force-rebuild

# Restore from backup
.\scripts\migration\migration.ps1 -Action restore-all
```

### Linux/macOS (Bash)

```bash
# Show migration status
./.docker/migration/migrate.sh status

# Add new migration
./.docker/migration/migrate.sh add AddNewFeature

# Check for inconsistencies
./.docker/migration/migrate.sh fix

# Clean orphaned files
./.docker/migration/migrate.sh clean

# Schema consistency check
./.docker/migration/migrate.sh schema-check

# Force rebuild (destructive!)
./.docker/migration/migrate.sh force-rebuild

# Restore from backup
./.docker/migration/migrate.sh restore-all

# Run full migration process
./.docker/migration/migrate.sh full
```

## Available Actions

| Action | Description | Parameters |
|--------|-------------|------------|
| `status` | Show current migration status | None |
| `add` | Create new migration | `-MigrationName "Name"` |
| `fix` | Check for migration inconsistencies | None |
| `clean` | Remove orphaned migration files | None |
| `force-rebuild` | Nuclear rebuild (destructive!) | None |
| `schema-check` | Check schema consistency | None |
| `restore-db` | Restore database from backup | None |
| `restore-migrations` | Restore migration files | None |
| `restore-all` | Restore everything from backup | None |

## Backup System

### Automatic Backups
- Database backups: `backup-before-force-rebuild-YYYYMMDD-HHMMSS.sql`
- Migration backups: `migration-backup-YYYYMMDD-HHMMSS/`

### Manual Backup Creation
```bash
# Database backup
docker exec ugh-db mysqldump -uuser -ppassword db --no-tablespaces > backup-manual-$(date +%Y%m%d-%H%M%S).sql
```

## Error Handling

### MySQL Warnings
The system automatically suppresses MySQL password warnings and handles them gracefully.

### Container Checks
- Verifies `ugh-db` and `ugh-backend` containers are running
- Checks EF Tools availability in containers
- Provides clear error messages for missing dependencies

### PowerShell Core Detection
On Linux/macOS, the system checks for PowerShell Core installation and provides installation instructions if missing.

## Security Considerations

### Force Rebuild
- **DESTRUCTIVE OPERATION**: Removes all migrations and data
- Requires explicit confirmation by typing "NUCLEAR"
- Always create backups before use
- Only use on test systems

### Backup Management
- Backups are stored locally in the project directory
- Consider moving backups to secure storage for production
- Regular backup rotation recommended

## Troubleshooting

### Common Issues

1. **PowerShell Core not found on Linux**
   ```bash
   # Install PowerShell Core
   sudo apt-get install powershell  # Ubuntu/Debian
   sudo yum install powershell      # CentOS/RHEL
   ```

2. **Containers not running**
   ```bash
   # Start the application
   docker compose up -d
   ```

3. **EF Tools not available**
   ```bash
   # The system automatically installs EF Tools
   # If manual installation needed:
   docker exec ugh-backend dotnet tool install --global dotnet-ef
   ```

4. **Permission denied on migrate.sh**
   ```bash
   # Make executable
   chmod +x .docker/migration/migrate.sh
   ```

### Debug Mode
For troubleshooting, you can run PowerShell commands directly:
```bash
pwsh -Command "& './scripts/migration/migration.ps1' -Action status"
```

## Best Practices

1. **Always check status before operations**
2. **Create backups before destructive operations**
3. **Test migrations on development environment first**
4. **Use descriptive migration names**
5. **Regular consistency checks with `fix` action**
6. **Monitor schema changes with `schema-check`**

## File Structure

```
scripts/migration/
├── migration.ps1          # Main PowerShell migration script
└── README.md             # This documentation

.docker/migration/
└── migrate.sh            # Linux/macOS wrapper script

backup-*.sql              # Database backups
migration-backup-*/       # Migration file backups
```

## Version History

- **v2.0**: Cross-platform support, backup/restore functionality
- **v1.0**: Initial Windows-only implementation








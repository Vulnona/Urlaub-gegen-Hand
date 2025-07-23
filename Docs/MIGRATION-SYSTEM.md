# Migration Management System - Übersicht

Für die vollständige Migrati#### PowerShell (Windows/Linux/macOS)
```powershell
# Check system health
.\scripts\migration\enhanced-migration.ps1 validate

# Run full migration pipeline
.\scripts\migration\enhanced-migration.ps1 run

# Create new migration
.\scripts\migration\enhanced-migration.ps1 add -MigrationName AddUserProfile

# Remove last migration
.\scripts\migration\enhanced-migration.ps1 remove -Force

# Dry run (preview changes)
.\scripts\migration\enhanced-migration.ps1 sync -DryRun
```he: **[Migration Tools](../scripts/migration/)**

## Schnellstart

```powershell
# Status aller Migrationen anzeigen
.\scripts\migration\enhanced-migration.ps1 status

# Neue Migration hinzufügen
.\scripts\migration\enhanced-migration.ps1 add -MigrationName "NeueFeature"

# Migrationen ausführen
.\scripts\migration\enhanced-migration.ps1 run
```

## Migration System Architektur

Das UGH-Projekt verwendet ein **zweistufiges Migration-System**:

### 1. Docker Migration Container (`/.docker/migration/`)
- **Zweck**: Basis-Setup und Datenbank-Initialisierung
- **Ausführung**: Automatisch beim Container-Start
- **Funktion**: Database Health Checks, Backup-Erstellung

### 2. Enhanced Migration Tools (`/scripts/migration/`)
- **Zweck**: Entwickler-Tools für erweiterte Migration-Verwaltung
- **Ausführung**: Manuell via PowerShell-Skript
- **Funktionen**: validate, cleanup, run, add, remove, status, sync

## Integration mit Infrastructure

Das Migration-System arbeitet eng mit dem Port-Management zusammen:
- **Port-Konfiguration**: Via `Docs/infrastructure/ports.config`
- **Container-Kommunikation**: Dynamische Port-Resolution
- **Service-Koordination**: Automatische Container-Abhängigkeiten

Für Details siehe: **[Infrastructure Management](./infrastructure/README.md)**

---

> **⚠ Legacy Dokumentation unten** - Verwende die neuen Tools in `/Docs/migration/`

# UGH Database Migration System

*Auto-generated on 2025-07-23 22:30:09*

## Overview

This document is automatically generated and tracks all database migrations in the UGH system.

## Migration Status

**Total Migrations:** 5  
**Applied Migrations:** 5  
**System Status:** ✅ Synchronized

## Available Commands

### Core Operations
- **validate**: Check system integrity and migration status
- **cleanup**: Clean old backups and temporary files  
- **run**: Execute full migration pipeline with validation
- **status**: Show detailed system overview

### Migration Management
- **add [name]**: Create new migration with Entity Framework
- **remove [name]**: Remove migration (use -Force for last migration)
- **sync**: Synchronize migration history between files and database

### Command Examples

#### PowerShell (Windows/Linux/macOS)
```powershell
# Check system health
.\scripts\migration\enhanced-migration.ps1 validate

# Run full migration pipeline
.\scripts\migration\enhanced-migration.ps1 run

# Create new migration
.\scripts\migration\enhanced-migration.ps1 add -MigrationName AddUserProfile

# Remove last migration
.\scripts\migration\enhanced-migration.ps1 remove -Force

# Dry run (preview changes)
.\scripts\migration\enhanced-migration.ps1 sync -DryRun
```

**Hinweis**: PowerShell Core ist plattformübergreifend verfügbar und funktioniert auf Windows, Linux und macOS.

## Migration History

| Migration ID | Description | Status | Version |
|--------------|-------------|---------|---------|
| `20250723171500_InitialCreate` | Initial database creation | ✅ Applied | 7.0.0 |
| `20250723172000_AddBasicTables` | Basic user and system tables | ✅ Applied | 7.0.0 |
| `20250723173000_AddShopTables` | Shop items and transactions | ✅ Applied | 7.0.0 |
| `20250723173500_AddProfileTables` | User profiles and skills | ✅ Applied | 7.0.0 |
| `20250723174700_Add2FASupport` | Two-Factor Authentication support | ✅ Applied | 7.0.0 |


### Migration System Features ✅
- ✅ **Automated Migration Tracking**: No manual intervention required
- ✅ **Full Command Suite**: validate, cleanup, run, add, remove, status, sync
- ✅ **Dual Platform Support**: PowerShell (Windows) + Bash (Linux/macOS)
- ✅ **Dry Run Support**: Preview changes before execution
- ✅ **Comprehensive Validation**: System integrity checking
- ✅ **Auto-Documentation**: This file updates automatically
- ✅ **Backup Management**: Automatic database backups before changes
- ✅ **Error Handling**: Robust error recovery and reporting

## Command Structure Overview

The enhanced migration system provides comprehensive control while maintaining automation:

### Primary Commands
1. **validate** - Checks database connectivity, migration history integrity, and identifies inconsistencies
2. **cleanup** - Removes old backups, temporary files, and validates system after cleanup
3. **run** - Executes full migration pipeline with pre-validation, backup, sync, and documentation update
4. **status** - Displays detailed system overview including health status and recent migrations
5. **add** - Creates new Entity Framework migrations and auto-syncs to database history
6. **remove** - Removes migrations from both files and database with safety checks
7. **sync** - Synchronizes migration history between EF files and database records

### Safety Features
- **Pre-execution validation** for all destructive operations
- **Automatic backups** before running migrations
- **Dry run mode** for testing changes without execution
- **Force flags** for overriding safety checks when needed
- **Container health checks** before executing EF commands

## Docker Integration

The system seamlessly integrates with the existing Docker infrastructure:

- **Database Container**: `ugh-db` (MySQL 8.0)
- **Backend Container**: `ugh-backend` (.NET 7.0)
- **Password Management**: Secure file-based credentials
- **Automatic Container Detection**: Validates containers before operations

---
*This documentation is automatically maintained. Manual edits will be overwritten.*

Last updated: 2025-07-23 18:26:00 by Enhanced Migration System

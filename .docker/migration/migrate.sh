#!/bin/bash
set -e

echo "Starting UgH Database Migration Process..."
echo "  UGH Database Migration - $(date)"
echo "======================================="

# Check if PowerShell Core is available
if ! command -v pwsh &> /dev/null; then
    echo "ERROR: PowerShell Core (pwsh) is not installed!"
    echo "Please install PowerShell Core for Linux:"
    echo "  Ubuntu/Debian: sudo apt-get install powershell"
    echo "  CentOS/RHEL: sudo yum install powershell"
    echo "  Or download from: https://github.com/PowerShell/PowerShell/releases"
    exit 1
fi

# Check if Docker is available
if ! command -v docker &> /dev/null; then
    echo "ERROR: Docker is not installed or not in PATH!"
    exit 1
fi

# Check if containers are running
echo "Checking container availability..."
if ! docker ps --filter "name=ugh-db" --format "{{.Status}}" | grep -q "Up"; then
    echo "ERROR: ugh-db container is not running!"
    echo "Please start the application with: docker compose up -d"
    exit 1
fi

if ! docker ps --filter "name=ugh-backend" --format "{{.Status}}" | grep -q "Up"; then
    echo "ERROR: ugh-backend container is not running!"
    echo "Please start the application with: docker compose up -d"
    exit 1
fi

echo "Containers are running ✓"

# Function to run migration command
run_migration() {
    local action="$1"
    local migration_name="$2"
    
    echo "Running migration: $action"
    
    if [ -n "$migration_name" ]; then
        pwsh -Command "& './scripts/migration/migration.ps1' -Action '$action' -MigrationName '$migration_name'"
    else
        pwsh -Command "& './scripts/migration/migration.ps1' -Action '$action'"
    fi
    
    local exit_code=$?
    if [ $exit_code -ne 0 ]; then
        echo "ERROR: Migration command '$action' failed with exit code $exit_code"
        exit $exit_code
    fi
}

# Main execution flow
main() {
    echo "Starting migration process..."
    
    # Show current status
    run_migration "status"
    
    # Check for schema changes
    echo ""
    echo "Checking for schema changes..."
    run_migration "schema-check"
    
    echo ""
    echo "Migration process completed successfully!"
    echo "======================================="
}

# Parse command line arguments
case "${1:-status}" in
    "status")
        run_migration "status"
        ;;
    "add")
        if [ -z "$2" ]; then
            echo "ERROR: Migration name required for 'add' action"
            echo "Usage: $0 add <migration_name>"
            exit 1
        fi
        run_migration "add" "$2"
        ;;
    "fix")
        run_migration "fix"
        ;;
    "clean")
        run_migration "clean"
        ;;
    "force-rebuild")
        echo "WARNING: This is a destructive operation!"
        read -p "Type 'NUCLEAR' to continue: " confirm
        if [ "$confirm" != "NUCLEAR" ]; then
            echo "Operation cancelled"
            exit 0
        fi
        run_migration "force-rebuild"
        ;;
    "schema-check")
        run_migration "schema-check"
        ;;
    "restore-db")
        run_migration "restore-db"
        ;;
    "restore-migrations")
        run_migration "restore-migrations"
        ;;
    "restore-all")
        run_migration "restore-all"
        ;;
    "full")
        main
        ;;
    *)
        echo "Usage: $0 <action> [migration_name]"
        echo ""
        echo "Available actions:"
        echo "  status              - Show migration status"
        echo "  add <name>          - Add new migration"
        echo "  fix                 - Check for inconsistencies"
        echo "  clean               - Clean orphaned files"
        echo "  force-rebuild       - Nuclear rebuild (destructive!)"
        echo "  schema-check        - Check schema consistency"
        echo "  restore-db          - Restore database from backup"
        echo "  restore-migrations  - Restore migration files"
        echo "  restore-all         - Restore everything"
        echo "  full                - Run full migration process"
        echo ""
        echo "Examples:"
        echo "  $0 status"
        echo "  $0 add AddNewFeature"
        echo "  $0 full"
        exit 1
        ;;
esac

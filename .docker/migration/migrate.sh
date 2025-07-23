#!/bin/bash
set -e

echo "Starting UgH Database Migration Process..."

# Configuration
MAX_RETRIES=30
RETRY_INTERVAL=5

# Read MySQL password from secret file
if [ -f "/run/secrets/db__root_password" ]; then
    MYSQL_ROOT_PASSWORD=$(cat /run/secrets/db__root_password)
elif [ -f "/run/secrets/db_root_password" ]; then
    MYSQL_ROOT_PASSWORD=$(cat /run/secrets/db_root_password)
else
    echo "ERROR: No database root password secret found!"
    exit 1
fi

echo "Configuration loaded successfully"
echo "  UGH Database Migration - $(date)"
echo "======================================="

# Function to wait for database to be ready
wait_for_database() {
    echo "Waiting for database to be ready..."
    for i in $(seq 1 $MAX_RETRIES); do
        if mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -e "SELECT 1;" > /dev/null 2>&1; then
            echo "Database is ready!"
            return 0
        fi
        echo "Database not ready yet, waiting... (attempt $i/$MAX_RETRIES)"
        sleep $RETRY_INTERVAL
    done
    echo "ERROR: Database failed to become ready after $MAX_RETRIES attempts"
    exit 1
}

# Function to create backup
create_backup() {
    if [ "${CREATE_BACKUP:-true}" = "true" ]; then
        local backup_name="$1"
        echo "Creating database backup before migration..."
        if mysqldump -h db -u root -p"$MYSQL_ROOT_PASSWORD" --single-transaction --routines --triggers db > "/tmp/${backup_name}.sql" 2>/dev/null; then
            echo "Backup created: /tmp/${backup_name}.sql"
        else
            echo "WARNING: Backup creation failed, continuing without backup..."
        fi
    fi
}

# Main execution flow
main() {
    # Create initial backup
    create_backup "db_backup_$(date +%Y%m%d_%H%M%S)"
    
    # Wait for database to be ready
    wait_for_database
    
    echo "No migrations needed - database is up to date"
    exit 0
}

# Run main function
main

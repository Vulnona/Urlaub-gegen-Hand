#!/bin/bash
# Database Migration Script 
# This script handles automated database migrations in Docker environment

set -e  # Exit on any error

echo "Starting UgH Database Migration Process..."

# Configuration
MAX_RETRIES=30
RETRY_INTERVAL=5
CONNECTION_STRING=${ConnectionStrings__DefaultConnection}

# Read MySQL password from secret file
if [ -f "/run/secrets/db__root_password" ]; then
    MYSQL_ROOT_PASSWORD=$(cat /run/secrets/db__root_password)
elif [ -f "/run/secrets/db_root_password" ]; then
    MYSQL_ROOT_PASSWORD=$(cat /run/secrets/db_root_password)
else
    echo "❌ No database root password secret found!"
    exit 1
fi

echo "Configuration loaded successfully"

# Function to wait for database
wait_for_database() {
    echo "⏳ Waiting for database to be ready..."
    
    for i in $(seq 1 $MAX_RETRIES); do
        if mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -e "SELECT 1;" > /dev/null 2>&1; then
            echo "✅ Database is ready!"
            return 0
        fi
        
        echo "   Attempt $i/$MAX_RETRIES: Database not ready, waiting ${RETRY_INTERVAL}s..."
        sleep $RETRY_INTERVAL
    done
    
    echo "❌ Database failed to become ready after $MAX_RETRIES attempts"
    exit 1
}

# Function to check if migrations are needed
check_migrations_needed() {
    echo "Checking migration status..."
    
    # Check if __EFMigrationsHistory table exists
    if mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -e "SELECT 1 FROM __EFMigrationsHistory LIMIT 1;" > /dev/null 2>&1; then
        echo "Migration history table exists, checking for pending migrations..."
        
        # Check if database needs updates - proper way to detect pending migrations
        UPDATE_NEEDED=$(dotnet ef database update --dry-run --no-build --project /app/Backend --startup-project /app/Backend --context Ugh_Context 2>&1 | grep -c "Applying migration" || echo "0")
        
        if [ "$UPDATE_NEEDED" -eq 0 ]; then
            echo "✅ Database is up to date - no migrations needed"
            return 1
        else
            echo "Pending migrations found - $UPDATE_NEEDED migration(s) to apply"
            # Show which migrations are pending
            dotnet ef database update --dry-run --no-build --project /app/Backend --startup-project /app/Backend --context Ugh_Context 2>&1 | grep "Applying migration" || true
            return 0
        fi
    else
        echo "No migration history found - initializing migration baseline..."
        
        # Check if database has tables (indicating manual setup)
        TABLE_COUNT=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -e "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='db' AND TABLE_TYPE='BASE TABLE';" -s -N 2>/dev/null || echo "0")
        
        if [ "$TABLE_COUNT" -gt 0 ]; then
            echo "Database has $TABLE_COUNT tables but no migration history"
            echo "Creating migration baseline to mark existing schema as applied..."
            
            # Add all migrations to history without applying them
            add_migration_baseline
            return 1
        else
            echo "Empty database - applying all migrations..."
            return 0
        fi
    fi
}

# Function to add migration baseline (mark existing schema as migrated)
add_migration_baseline() {
    echo "Adding migration baseline..."
    
    # Create __EFMigrationsHistory table if it doesn't exist
    mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -e "
        CREATE TABLE IF NOT EXISTS \`__EFMigrationsHistory\` (
            \`MigrationId\` varchar(150) NOT NULL,
            \`ProductVersion\` varchar(32) NOT NULL,
            PRIMARY KEY (\`MigrationId\`)
        );
    " 2>/dev/null || {
        echo "❌ Failed to create migration history table"
        return 1
    }
    
    # Get all migration files and mark them as applied
    MIGRATIONS=$(dotnet ef migrations list --no-build --project /app/Backend --startup-project /app/Backend --context Ugh_Context | grep -E "^\s*[0-9]" | awk '{print $1}' || true)
    
    if [ -n "$MIGRATIONS" ]; then
        echo "📝 Marking existing migrations as applied:"
        for migration in $MIGRATIONS; do
            echo "   - $migration"
            mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -e "
                INSERT IGNORE INTO \`__EFMigrationsHistory\` (\`MigrationId\`, \`ProductVersion\`) 
                VALUES ('$migration', '7.0.20');
            " 2>/dev/null || {
                echo "⚠️  Warning: Failed to insert migration $migration"
            }
        done
        echo "✅ Migration baseline created successfully!"
    else
        echo "⚠️  No migrations found to baseline"
    fi
}

# Function to apply migrations with robust error handling
apply_migrations() {
    echo "🔄 Applying database migrations with robust error handling..."
    
    # Check if any migrations need to be applied using dry-run
    UPDATE_CHECK=$(dotnet ef database update --dry-run --no-build --project /app/Backend --startup-project /app/Backend --context Ugh_Context 2>&1 || true)
    PENDING_COUNT=$(echo "$UPDATE_CHECK" | grep -c "Applying migration" || echo "0")
    
    if [ "$PENDING_COUNT" -eq 0 ]; then
        echo "✅ No pending migrations to apply"
        return 0
    fi
    
    echo "📋 Found $PENDING_COUNT migration(s) to apply:"
    echo "$UPDATE_CHECK" | grep "Applying migration" || true
    echo ""
    
    # Apply all pending migrations at once (more reliable than one-by-one)
    echo "🔧 Applying all pending migrations..."
    
    # Create backup before migrations
    create_migration_backup "batch_$(date +%Y%m%d_%H%M%S)"
    
    # Apply all migrations with detailed error handling
    if dotnet ef database update --no-build --project /app/Backend --startup-project /app/Backend --context Ugh_Context; then
        echo "✅ Successfully applied all pending migrations!"
        verify_migration_result
        return 0
    else
        echo "❌ Failed to apply migrations"
        handle_migration_failure "batch_migration"
        return 1
    fi
}

# Function to verify migration success
verify_migration() {
    echo "🔍 Verifying migration status..."
    
    # Check if __EFMigrationsHistory table exists and has entries
    MIGRATION_COUNT=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SELECT COUNT(*) FROM __EFMigrationsHistory;" 2>/dev/null || echo "0")
    
    if [ "$MIGRATION_COUNT" -gt 0 ]; then
        echo "✅ Migration verification successful - $MIGRATION_COUNT migrations in history"
        
        # Show last applied migration
        LAST_MIGRATION=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId DESC LIMIT 1;" 2>/dev/null || echo "None")
        echo "Last applied migration: $LAST_MIGRATION"
    else
        echo "⚠️  No migration history found - this might be the first run"
    fi
}

# Function to create backup before each migration

# Function to create backup before each migration
create_migration_backup() {
    local migration_name="$1"
    local backup_file="/tmp/backup_before_${migration_name}_$(date +%Y%m%d_%H%M%S).sql"
    
    echo "💾 Creating backup before migration $migration_name..."
    
    mysqldump -h db -u root -p"$MYSQL_ROOT_PASSWORD" \
        --single-transaction \
        --routines \
        --triggers \
        --lock-tables=false \
        --add-drop-table \
        db > "$backup_file" 2>/dev/null || {
        echo "⚠️  Backup creation failed - continuing anyway"
        return 0
    }
    
    echo "✅ Backup created: $backup_file"
}

# Function to validate migration safety
validate_migration_safety() {
    local migration_name="$1"
    
    echo "🔍 Validating migration safety: $migration_name"
    
    # Get migration SQL to analyze potential issues
    local migration_sql=$(dotnet ef migrations script "$migration_name" "$migration_name" \
        --no-build \
        --project /app/Backend \
        --startup-project /app/Backend \
        --context Ugh_Context 2>/dev/null || echo "")
    
    if [ -n "$migration_sql" ]; then
        # Check for risky operations
        check_risky_operations "$migration_sql" "$migration_name"
    else
        echo "⚠️  Could not retrieve migration SQL - proceeding with caution"
    fi
    
    return 0
}

# Function to check for risky migration operations
check_risky_operations() {
    local sql="$1"
    local migration_name="$2"
    local warnings=0
    
    echo "🔍 Analyzing migration for potential issues..."
    
    # Check for NOT NULL additions to existing tables
    if echo "$sql" | grep -i "ADD.*NOT NULL" > /dev/null; then
        echo "⚠️  WARNING: Adding NOT NULL column - may fail if existing data has NULLs"
        warnings=$((warnings + 1))
        suggest_not_null_fix "$migration_name"
    fi
    
    # Check for column drops
    if echo "$sql" | grep -i "DROP COLUMN" > /dev/null; then
        echo "⚠️  WARNING: Dropping columns - data will be lost permanently"
        warnings=$((warnings + 1))
    fi
    
    # Check for table drops
    if echo "$sql" | grep -i "DROP TABLE" > /dev/null; then
        echo "⚠️  WARNING: Dropping tables - all data will be lost permanently"
        warnings=$((warnings + 1))
    fi
    
    # Check for unique constraint additions
    if echo "$sql" | grep -i "ADD.*UNIQUE" > /dev/null; then
        echo "⚠️  WARNING: Adding unique constraints - may fail with duplicate data"
        warnings=$((warnings + 1))
        suggest_unique_constraint_fix "$migration_name"
    fi
    
    # Check for foreign key additions
    if echo "$sql" | grep -i "ADD.*FOREIGN KEY" > /dev/null; then
        echo "⚠️  WARNING: Adding foreign keys - may fail with orphaned records"
        warnings=$((warnings + 1))
        suggest_foreign_key_fix "$migration_name"
    fi
    
    if [ $warnings -gt 0 ]; then
        echo "Found $warnings potential issues in migration $migration_name"
        echo "Consider running data cleanup before this migration"
    else
        echo "✅ No obvious risks detected in migration"
    fi
}

# Function to suggest fixes for NOT NULL issues
suggest_not_null_fix() {
    local migration_name="$1"
    
    echo "💡 SUGGESTION for NOT NULL issues:"
    echo "   1. Check for NULL values: SELECT * FROM table WHERE column IS NULL;"
    echo "   2. Update NULL values: UPDATE table SET column = 'default_value' WHERE column IS NULL;"
    echo "   3. Or add DEFAULT constraint first"
}

# Function to suggest fixes for unique constraint issues
suggest_unique_constraint_fix() {
    local migration_name="$1"
    
    echo "💡 SUGGESTION for UNIQUE constraint issues:"
    echo "   1. Find duplicates: SELECT column, COUNT(*) FROM table GROUP BY column HAVING COUNT(*) > 1;"
    echo "   2. Clean up duplicates before applying migration"
    echo "   3. Consider adding composite unique constraints instead"
}

# Function to suggest fixes for foreign key issues
suggest_foreign_key_fix() {
    local migration_name="$1"
    
    echo "💡 SUGGESTION for FOREIGN KEY issues:"
    echo "   1. Find orphaned records: SELECT * FROM child WHERE parent_id NOT IN (SELECT id FROM parent);"
    echo "   2. Clean up orphaned records or add missing parent records"
    echo "   3. Consider using CASCADE options carefully"
}

# Function to apply single migration with error handling
apply_single_migration() {
    local migration_name="$1"
    local max_retries=3
    local retry_count=0
    
    echo "🔄 Applying migration: $migration_name"
    
    while [ $retry_count -lt $max_retries ]; do
        # Apply migration to specific target
        if dotnet ef database update "$migration_name" \
            --no-build \
            --project /app/Backend \
            --startup-project /app/Backend \
            --context Ugh_Context \
            --verbose 2>&1; then
            
            echo "✅ Migration $migration_name applied successfully"
            return 0
        else
            retry_count=$((retry_count + 1))
            echo "❌ Migration failed (attempt $retry_count/$max_retries)"
            
            if [ $retry_count -lt $max_retries ]; then
                echo "⏳ Waiting 5 seconds before retry..."
                sleep 5
                
                # Try to diagnose the issue
                diagnose_migration_failure "$migration_name"
            fi
        fi
    done
    
    echo "❌ Migration $migration_name failed after $max_retries attempts"
    return 1
}

# Function to diagnose migration failures
diagnose_migration_failure() {
    local migration_name="$1"
    
    echo "🔍 Diagnosing migration failure..."
    
    # Check database connectivity
    if ! mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -e "SELECT 1;" > /dev/null 2>&1; then
        echo "❌ Database connection lost"
        return 1
    fi
    
    # Check for lock issues
    local locks=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SHOW PROCESSLIST;" 2>/dev/null | grep -i "lock" || echo "")
    if [ -n "$locks" ]; then
        echo "⚠️  Detected database locks - waiting for release..."
        sleep 10
    fi
    
    # Check disk space
    local disk_usage=$(df / | tail -1 | awk '{print $5}' | sed 's/%//')
    if [ "$disk_usage" -gt 90 ]; then
        echo "⚠️  Warning: Disk usage is at ${disk_usage}%"
    fi
    
    # Check for specific error patterns
    echo "   Run manual diagnosis with:"
    echo "   docker-compose exec db mysql -u root -p\"password\" -D db"
    echo "   Check for: data integrity, constraints, disk space, locks"
}

# Function to handle migration failure with recovery options
handle_migration_failure() {
    local migration_name="$1"
    
    echo "    Migration failure detected: $migration_name"
    echo "  Available recovery options:"
    echo "   1. Check backup files in /tmp/"
    echo "   2. Restore from backup if needed"
    echo "   3. Fix data issues manually"
    echo "   4. Re-run migration"
    
    # Create failure report
    local failure_report="/tmp/migration_failure_${migration_name}_$(date +%Y%m%d_%H%M%S).log"
    echo "Creating failure report: $failure_report"
    
    {
        echo "Migration Failure Report"
        echo "======================"
        echo "Migration: $migration_name"
        echo "Timestamp: $(date)"
        echo "Database Status:"
        mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -e "SHOW PROCESSLIST;" 2>/dev/null || echo "Could not get process list"
        echo ""
        echo "Last Applied Migration:"
        mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId DESC LIMIT 1;" 2>/dev/null || echo "Could not get migration history"
        echo ""
        echo "Available Backups:"
        ls -la /tmp/backup_* 2>/dev/null || echo "No backups found"
    } > "$failure_report"
    
    echo "📋 Failure report saved to: $failure_report"
    
    # Set environment variable for debugging
    export MIGRATION_FAILURE_REPORT="$failure_report"
    export FAILED_MIGRATION="$migration_name"
}

# Function to verify migration result
verify_migration_result() {
    local migration_name="$1"
    
    echo "✅ Verifying migration result: $migration_name"
    
    # Check if migration was recorded in history
    local recorded=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SELECT COUNT(*) FROM __EFMigrationsHistory WHERE MigrationId='$migration_name';" 2>/dev/null || echo "0")
    
    if [ "$recorded" -eq 1 ]; then
        echo "✅ Migration $migration_name recorded in history"
    else
        echo "❌ Migration $migration_name NOT found in history"
        return 1
    fi
    
    # Run basic database integrity check
    local integrity_check=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "CHECK TABLE users;" 2>/dev/null | grep -c "OK" || echo "0")
    if [ "$integrity_check" -gt 0 ]; then
        echo "✅ Basic integrity check passed"
    else
        echo "⚠️  Database integrity check failed or table not found"
    fi
}

# Function to create database backup before migration
create_backup() {
    echo "💾 Creating database backup before migration..."
    
    BACKUP_FILE="/tmp/db_backup_$(date +%Y%m%d_%H%M%S).sql"
    
    mysqldump -h db -u root -p"$MYSQL_ROOT_PASSWORD" --single-transaction --routines --triggers db > "$BACKUP_FILE" 2>/dev/null || {
        echo "⚠️  Backup creation failed or database is empty - continuing anyway"
        return 0
    }
    
    echo "✅ Backup created: $BACKUP_FILE"
}

# Main execution flow
main() {
    echo "  UGH Database Migration - $(date)"
    echo "======================================="
    
    # Step 1: Wait for database
    wait_for_database
    
    # Step 2: Create backup (optional, for safety)
    if [ "${CREATE_BACKUP:-true}" = "true" ]; then
        create_backup
    fi
    
    # Step 3: Check if migrations are needed
    if check_migrations_needed; then
        # Step 4: Apply migrations
        apply_migrations
        
        # Step 5: Verify success  
        echo "🔍 Verifying migration status..."
        
        # Check if __EFMigrationsHistory table exists and has entries
        MIGRATION_COUNT=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SELECT COUNT(*) FROM __EFMigrationsHistory;" 2>/dev/null || echo "0")
        
        if [ "$MIGRATION_COUNT" -gt 0 ]; then
            echo "✅ Migration verification successful - $MIGRATION_COUNT migrations in history"
            
            # Show last applied migration
            LAST_MIGRATION=$(mysql -h db -u root -p"$MYSQL_ROOT_PASSWORD" -D db -se "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY MigrationId DESC LIMIT 1;" 2>/dev/null || echo "None")
            echo "📅 Last applied migration: $LAST_MIGRATION"
        else
            echo "⚠️  No migration history found - this might be the first run"
        fi
        
        echo ""
        echo " Migration process completed successfully!"
    else
        echo ""
        echo "✨ No migrations needed - database is current!"
    fi
    
    echo "======================================="
    echo "📊 Migration Summary:"
    echo "   Database Host: db"
    echo "   Timestamp: $(date)"
    echo "   Status: SUCCESS"
    echo "======================================="
}

# Execute main function
main "$@"

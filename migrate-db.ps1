# Database Migration Management Script for UGH Project
param(
    [Parameter(Position=0)]
    [string]$Action = "help",
    [switch]$Force,
    [switch]$SkipValidation
)

$DatabaseService = "db"

function Write-ColorOutput {
    param([string]$Message, [string]$Color = "White")
    Write-Host $Message -ForegroundColor $Color
}

function Test-DockerRunning {
    try {
        $null = docker version 2>$null
        return $LASTEXITCODE -eq 0
    } catch {
        Write-ColorOutput "Docker is not running. Please start Docker Desktop." "Red"
        return $false
    }
}

function Start-DatabaseIfNeeded {
    $dbStatus = docker-compose ps $DatabaseService --format json | ConvertFrom-Json | Select-Object -ExpandProperty State -ErrorAction SilentlyContinue
    
    if ($dbStatus -ne "running") {
        Write-ColorOutput "Starting database service..." "Yellow"
        docker-compose up -d $DatabaseService
        Start-Sleep 10
    }
}

function Test-MigrationSafety {
    Write-ColorOutput "Validating migration safety..." "Cyan"
    Start-DatabaseIfNeeded
    
    # Check for NULL values in critical columns
    Write-ColorOutput "Checking for potential data conflicts..." "Yellow"
    
    # Check for NULL values in critical columns
    $nullIssues = docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        SELECT 'users.email' as table_column, COUNT(*) as null_count FROM users WHERE email IS NULL OR email = '' 
        UNION ALL
        SELECT 'userprofiles.user_id', COUNT(*) FROM userprofiles WHERE user_id IS NULL
        UNION ALL  
        SELECT 'offers.user_id', COUNT(*) FROM offers WHERE user_id IS NULL;
    " 2>$null
    
    if ($nullIssues -match "[1-9]") {
        Write-ColorOutput "WARNING: Found NULL values in critical columns!" "Red"
        Write-ColorOutput "Run cleanup before migration: .\migrate-db.ps1 cleanup" "Yellow"
        return $false
    } else {
        Write-ColorOutput "NULL value check passed" "Green"
    }
    
    # Check for duplicate values that might conflict with unique constraints
    $duplicates = docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        SELECT 'users.email' as table_column, email, COUNT(*) as count 
        FROM users 
        GROUP BY email 
        HAVING COUNT(*) > 1 
        LIMIT 5;
    " 2>$null
    
    if ($duplicates -match "count") {
        Write-ColorOutput "WARNING: Found duplicate email addresses!" "Red"
        Write-ColorOutput "These will conflict with unique constraints" "Yellow"
        return $false
    } else {
        Write-ColorOutput "Duplicate check passed" "Green"
    }
    
    # Check for orphaned foreign key references
    $orphaned = docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        SELECT 'userprofiles orphaned' as issue, COUNT(*) as count
        FROM userprofiles up 
        LEFT JOIN users u ON up.user_id = u.Id 
        WHERE u.Id IS NULL
        UNION ALL
        SELECT 'offers orphaned', COUNT(*)
        FROM offers o
        LEFT JOIN users u ON o.user_id = u.Id
        WHERE u.Id IS NULL;
    " 2>$null
    
    if ($orphaned -match "[1-9]") {
        Write-ColorOutput "WARNING: Found orphaned records!" "Red"
        Write-ColorOutput "These will conflict with foreign key constraints" "Yellow"
        return $false
    } else {
        Write-ColorOutput "Foreign key integrity check passed" "Green"
    }
    
    return $true
}

function Invoke-DataCleanup {
    Write-ColorOutput "Running comprehensive data cleanup..." "Cyan"
    Start-DatabaseIfNeeded
    
    if (-not $Force) {
        Write-ColorOutput "WARNING: This will modify your data!" "Red"
        $confirmation = Read-Host "Continue with data cleanup? (yes/no)"
        if ($confirmation -ne "yes") {
            Write-ColorOutput "Cleanup cancelled" "Yellow"
            return
        }
    }
    
    Write-ColorOutput "1. Cleaning up NULL email addresses..." "Yellow"
    docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        UPDATE users 
        SET email = CONCAT('missing_email_', Id, '@placeholder.com') 
        WHERE email IS NULL OR email = '';
    " 2>$null
    
    Write-ColorOutput "2. Removing orphaned profiles..." "Yellow"
    docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        DELETE up FROM userprofiles up 
        LEFT JOIN users u ON up.user_id = u.Id 
        WHERE u.Id IS NULL;
    " 2>$null
    
    Write-ColorOutput "3. Removing orphaned offers..." "Yellow"
    docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        DELETE o FROM offers o
        LEFT JOIN users u ON o.user_id = u.Id
        WHERE u.Id IS NULL;
    " 2>$null
    
    Write-ColorOutput "4. Handling duplicate emails..." "Yellow"
    docker-compose exec -T $DatabaseService mysql -u root --password="password" -D db -e "
        UPDATE users u1
        JOIN (
            SELECT email, MIN(Id) as keep_id
            FROM users 
            GROUP BY email 
            HAVING COUNT(*) > 1
        ) u2 ON u1.email = u2.email AND u1.Id != u2.keep_id
        SET u1.email = CONCAT(u1.email, '_duplicate_', u1.Id);
    " 2>$null
    
    Write-ColorOutput "Cleanup completed!" "Green"
    Write-ColorOutput "Run validation again: .\migrate-db-clean.ps1 validate" "Cyan"
}

function Invoke-Migration {
    Write-ColorOutput "Running database migrations..." "Cyan"
    
    if (-not $SkipValidation) {
        Write-ColorOutput "Running safety validation first..." "Yellow"
        if (-not (Test-MigrationSafety)) {
            Write-ColorOutput "Migration aborted due to validation failures!" "Red"
            Write-ColorOutput "Use -SkipValidation to override (dangerous!)" "Yellow"
            return
        }
    }
    
    Write-ColorOutput "Starting migration process..." "Green"
    docker-compose up migration
}

# Main execution
if (-not (Test-DockerRunning)) {
    exit 1
}

switch ($Action.ToLower()) {
    "run" { Invoke-Migration }
    "validate" { 
        Write-ColorOutput "Running migration validation..." "Cyan"
        if (Test-MigrationSafety) {
            Write-ColorOutput "All validation checks passed!" "Green"
        } else {
            Write-ColorOutput "Validation failed - see issues above" "Red"
        }
    }
    "cleanup" { Invoke-DataCleanup }
    default { 
        Write-ColorOutput "Available actions: run, validate, cleanup" "Yellow"
        Write-ColorOutput "Usage: .\migrate-db-clean.ps1 <action>" "White"
    }
}

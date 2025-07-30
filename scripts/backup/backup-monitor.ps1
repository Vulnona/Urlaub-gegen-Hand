# UGH Backup Monitor Script
# PowerShell script to monitor backup status and trigger manual backups

param(
    [Parameter(Mandatory=$false)]
    [string]$ApiUrl = "http://localhost:8081",
    [Parameter(Mandatory=$false)]
    [string]$AdminToken,
    [Parameter(Mandatory=$false)]
    [switch]$CreateBackup,
    [Parameter(Mandatory=$false)]
    [switch]$ListBackups,
    [Parameter(Mandatory=$false)]
    [switch]$ShowStats,
    [Parameter(Mandatory=$false)]
    [switch]$Help
)

function Show-Help {
    Write-Host "UGH Backup Monitor Script" -ForegroundColor Cyan
    Write-Host "=========================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  .\backup-monitor.ps1 -ListBackups" -ForegroundColor Gray
    Write-Host "  .\backup-monitor.ps1 -ShowStats" -ForegroundColor Gray
    Write-Host "  .\backup-monitor.ps1 -CreateBackup" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Parameters:" -ForegroundColor White
    Write-Host "  -ApiUrl              API URL (default: http://localhost:8081)" -ForegroundColor Gray
    Write-Host "  -AdminToken          Admin JWT token for authentication" -ForegroundColor Gray
    Write-Host "  -CreateBackup        Trigger a manual backup" -ForegroundColor Gray
    Write-Host "  -ListBackups         List all available backups" -ForegroundColor Gray
    Write-Host "  -ShowStats           Show backup statistics" -ForegroundColor Gray
    Write-Host ""
    Write-Host "Examples:" -ForegroundColor White
    Write-Host "  .\backup-monitor.ps1 -ListBackups" -ForegroundColor Gray
    Write-Host "  .\backup-monitor.ps1 -CreateBackup -AdminToken 'your-jwt-token'" -ForegroundColor Gray
    Write-Host "  .\backup-monitor.ps1 -ShowStats -ApiUrl 'https://your-production-api.com'" -ForegroundColor Gray
}

function Invoke-ApiRequest {
    param(
        [string]$Endpoint,
        [string]$Method = "GET",
        [string]$Token = $null
    )
    
    $headers = @{
        "Content-Type" = "application/json"
    }
    
    if ($Token) {
        $headers["Authorization"] = "Bearer $Token"
    }
    
    try {
        $response = Invoke-RestMethod -Uri "$ApiUrl/api/backup/$Endpoint" -Method $Method -Headers $headers
        return $response
    }
    catch {
        Write-Host "API request failed: $($_.Exception.Message)" -ForegroundColor Red
        return $null
    }
}

function Show-BackupList {
    Write-Host "Fetching backup list..." -ForegroundColor Yellow
    
    $backups = Invoke-ApiRequest -Endpoint "list" -Token $AdminToken
    
    if ($backups) {
        Write-Host "`nBackup List:" -ForegroundColor Green
        Write-Host "============" -ForegroundColor Green
        
        if ($backups.TotalBackups -eq 0) {
            Write-Host "No backups found." -ForegroundColor Yellow
            return
        }
        
        foreach ($backup in $backups.Backups) {
            $date = [DateTime]::Parse($backup.LastModified).ToString('yyyy-MM-dd HH:mm:ss')
            Write-Host "📁 $($backup.FileName)" -ForegroundColor White
            Write-Host "   Size: $($backup.SizeInMB) MB" -ForegroundColor Gray
            Write-Host "   Date: $date" -ForegroundColor Gray
            Write-Host ""
        }
        
        Write-Host "Total Backups: $($backups.TotalBackups)" -ForegroundColor Cyan
    }
}

function Show-BackupStats {
    Write-Host "Fetching backup statistics..." -ForegroundColor Yellow
    
    $stats = Invoke-ApiRequest -Endpoint "stats" -Token $AdminToken
    
    if ($stats) {
        Write-Host "`nBackup Statistics:" -ForegroundColor Green
        Write-Host "==================" -ForegroundColor Green
        Write-Host "Total Backups: $($stats.TotalBackups)" -ForegroundColor White
        Write-Host ("Total Size: " + $stats.TotalSizeMB + " MB (" + $stats.TotalSizeGB + " GB)") -ForegroundColor White
        
        if ($stats.OldestBackup) {
            $oldest = [DateTime]::Parse($stats.OldestBackup).ToString('yyyy-MM-dd HH:mm:ss')
            Write-Host "Oldest Backup: $oldest" -ForegroundColor White
        }
        
        if ($stats.NewestBackup) {
            $newest = [DateTime]::Parse($stats.NewestBackup).ToString('yyyy-MM-dd HH:mm:ss')
            Write-Host "Newest Backup: $newest" -ForegroundColor White
        }
        
        if ($stats.LastBackupFileName) {
            Write-Host "Last Backup: $($stats.LastBackupFileName)" -ForegroundColor White
        }
        
        # Calculate time since last backup
        if ($stats.NewestBackup) {
            $lastBackup = [DateTime]::Parse($stats.NewestBackup)
            $timeSince = (Get-Date) - $lastBackup
            
            if ($timeSince.TotalHours -lt 24) {
                $hours = [math]::Round($timeSince.TotalHours, 1)
                Write-Host "Status: ✅ Backup is recent ($hours hours ago)" -ForegroundColor Green
            }
            elseif ($timeSince.TotalHours -lt 48) {
                $hours = [math]::Round($timeSince.TotalHours, 1)
                Write-Host "Status: ⚠️  Backup is getting old ($hours hours ago)" -ForegroundColor Yellow
            }
            else {
                $days = [math]::Round($timeSince.TotalDays, 1)
                Write-Host "Status: ❌ Backup is too old ($days days ago)" -ForegroundColor Red
            }
        }
    }
}

function Create-ManualBackup {
    Write-Host "Triggering manual backup..." -ForegroundColor Yellow
    
    if (-not $AdminToken) {
        Write-Host "Error: Admin token is required for creating backups" -ForegroundColor Red
        return
    }
    
    $result = Invoke-ApiRequest -Endpoint "create" -Method "POST" -Token $AdminToken
    
    if ($result) {
        Write-Host "✅ $($result.message)" -ForegroundColor Green
        Write-Host "Backup process started. Check the logs for progress." -ForegroundColor Cyan
    }
}

function Test-ApiConnection {
    Write-Host "Testing API connection..." -ForegroundColor Yellow
    
    try {
        $health = Invoke-RestMethod -Uri "$ApiUrl/api/health" -Method "GET"
        Write-Host "✅ API is healthy: $($health.status)" -ForegroundColor Green
        Write-Host "   Environment: $($health.environment)" -ForegroundColor Gray
        Write-Host "   Version: $($health.version)" -ForegroundColor Gray
        return $true
    }
    catch {
        Write-Host "❌ API connection failed: $($_.Exception.Message)" -ForegroundColor Red
        return $false
    }
}

# Main execution
try {
    if ($Help) {
        Show-Help
        exit 0
    }
    
    # Test API connection first
    if (-not (Test-ApiConnection)) {
        exit 1
    }
    
    # Execute requested operations
    if ($ListBackups) {
        Show-BackupList
    }
    
    if ($ShowStats) {
        Show-BackupStats
    }
    
    if ($CreateBackup) {
        Create-ManualBackup
    }
    
    # If no specific operation requested, show stats
    if (-not $ListBackups -and -not $ShowStats -and -not $CreateBackup) {
        Show-BackupStats
    }
}
catch {
    Write-Host "Script execution failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} 
# UGH Port-Management System
Write-Host "=== UGH Port-Management System ===" -ForegroundColor Magenta
Write-Host "===================================" -ForegroundColor Magenta

try {
    # Read ports configuration
    $portsConfigPath = Join-Path $PSScriptRoot "scripts\infrastructure\ports.config"
    
    if (!(Test-Path $portsConfigPath)) {
        # Try alternative location
        $portsConfigPath = Join-Path $PSScriptRoot "ports.config"
        if (!(Test-Path $portsConfigPath)) {
            Write-Host "Keine ports.config gefunden - erstelle Standard-Konfiguration..." -ForegroundColor Yellow
            # Skip port configuration but continue with EF fixes
            $ports = @{}
        } else {
            $ports = @{}
            $content = Get-Content $portsConfigPath -ErrorAction Stop
            
            foreach ($line in $content) {
                if ($line -match '^([A-Z_]+)=(\d+)$') {
                    $ports[$matches[1]] = $matches[2]
                    Write-Host "Port gelesen: $($matches[1]) = $($matches[2])" -ForegroundColor Cyan
                }
            }
        }
    } else {
        $ports = @{}
        $content = Get-Content $portsConfigPath -ErrorAction Stop
        
        foreach ($line in $content) {
            if ($line -match '^([A-Z_]+)=(\d+)$') {
                $ports[$matches[1]] = $matches[2]
                Write-Host "Port gelesen: $($matches[1]) = $($matches[2])" -ForegroundColor Cyan
            }
        }
    }
    
    Write-Host "ERFOLG: Ports erfolgreich gelesen aus: $portsConfigPath" -ForegroundColor Green
    
    # Update compose.yaml
    $composeFile = Join-Path $PSScriptRoot "compose.yaml"
    
    if (Test-Path $composeFile) {
        Write-Host "Aktualisiere compose.yaml..." -ForegroundColor Cyan
        
        $composeContent = Get-Content $composeFile -Raw
        $updated = $false
        
        # Backend Port Mapping (im backend service)
        if ($ports.ContainsKey('BACKEND_PORT')) {
            # Port Mapping aktualisieren
            $oldPortPattern = "(\s+- )'8080:8080'"
            $newPortMapping = "`$1'$($ports.BACKEND_PORT):$($ports.BACKEND_PORT)'"
            if ($composeContent -match $oldPortPattern) {
                $composeContent = $composeContent -replace $oldPortPattern, $newPortMapping
                $updated = $true
                Write-Host "Backend Port Mapping aktualisiert: $($ports.BACKEND_PORT):$($ports.BACKEND_PORT)" -ForegroundColor Green
            }
            
            # Environment Variable aktualisieren
            $oldEnvPattern = "ASPNETCORE_URLS=http://\+:8080"
            $newEnvVar = "ASPNETCORE_URLS=http://+:$($ports.BACKEND_PORT)"
            if ($composeContent -match $oldEnvPattern) {
                $composeContent = $composeContent -replace $oldEnvPattern, $newEnvVar
                $updated = $true
                Write-Host "Backend Environment Variable aktualisiert: $newEnvVar" -ForegroundColor Green
            }
        }
        
        if ($updated) {
            Set-Content -Path $composeFile -Value $composeContent -NoNewline
            Write-Host "✅ compose.yaml aktualisiert" -ForegroundColor Green
        } else {
            Write-Host "Keine Änderungen in compose.yaml notwendig" -ForegroundColor Yellow
        }
    }
    
    # Update Entity Framework Design-Time Configuration
    Write-Host ""
    Write-Host "=== EF Design-Time Configuration Update ===" -ForegroundColor Magenta
    
    # Update UghContextFactory for design-time
    $contextFactoryFile = Join-Path $PSScriptRoot "Backend\DATA\UghContextFactory.cs"
    if (Test-Path $contextFactoryFile) {
        Write-Host "Aktualisiere UghContextFactory.cs für Design-Time..." -ForegroundColor Cyan
        
        $factoryContent = Get-Content $contextFactoryFile -Raw
        
        # Check if design-time fix is already present
        if ($factoryContent -notmatch "localhost:3306") {
            # Add design-time connection string fix
            $designTimePattern = '(\s+var connectionString = configuration\.GetConnectionString\("DefaultConnection"\);)'
            $designTimeFix = @"
`$1
            
            // Design-Time Fix: Replace docker container name with localhost for EF tools
            if (connectionString.Contains("Server=db;")) {
                connectionString = connectionString.Replace("Server=db;", "Server=localhost;");
                connectionString = connectionString.Replace("root", "root").Replace("password", "password");
                Console.WriteLine(`$"Design-Time Connection String: {connectionString}");
            }
"@
            
            if ($factoryContent -match $designTimePattern) {
                $factoryContent = $factoryContent -replace $designTimePattern, $designTimeFix
                Set-Content -Path $contextFactoryFile -Value $factoryContent -NoNewline
                Write-Host "✅ UghContextFactory.cs für Design-Time aktualisiert" -ForegroundColor Green
            }
        } else {
            Write-Host "UghContextFactory.cs bereits für Design-Time konfiguriert" -ForegroundColor Yellow
        }
    }
    
    # Update Ugh_Context for design-time
    $contextFile = Join-Path $PSScriptRoot "Backend\DATA\Ugh_Context.cs"
    if (Test-Path $contextFile) {
        Write-Host "Aktualisiere Ugh_Context.cs für Design-Time..." -ForegroundColor Cyan
        
        $contextContent = Get-Content $contextFile -Raw
        
        # Check if design-time fix is already present
        if ($contextContent -notmatch "localhost:3306") {
            # Replace ServerVersion.AutoDetect with fixed version for design-time
            $autoDetectPattern = 'ServerVersion\.AutoDetect\(connectionString\)'
            $fixedVersion = 'GetDesignTimeServerVersion(connectionString)'
            
            if ($contextContent -match $autoDetectPattern) {
                $contextContent = $contextContent -replace $autoDetectPattern, $fixedVersion
                
                # Add the helper method if not present
                if ($contextContent -notmatch "GetDesignTimeServerVersion") {
                    $helperMethod = @"

    private static ServerVersion GetDesignTimeServerVersion(string connectionString)
    {
        try 
        {
            // Fix connection string for design-time (localhost instead of docker container)
            if (connectionString.Contains("Server=db;")) {
                connectionString = connectionString.Replace("Server=db;", "Server=localhost;");
            }
            return ServerVersion.AutoDetect(connectionString);
        }
        catch 
        {
            // Fallback for design-time when database is not available
            return ServerVersion.Create(Version.Parse("8.0.0"), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
        }
    }
"@
                    
                    # Insert before the last closing brace
                    $contextContent = $contextContent -replace '(\s*})(\s*)$', "$helperMethod`n`$1`$2"
                }
                
                Set-Content -Path $contextFile -Value $contextContent -NoNewline
                Write-Host "✅ Ugh_Context.cs für Design-Time aktualisiert" -ForegroundColor Green
            }
        } else {
            Write-Host "Ugh_Context.cs bereits für Design-Time konfiguriert" -ForegroundColor Yellow
        }
    }
    
    # Update Program.cs for design-time
    $programFile = Join-Path $PSScriptRoot "Backend\Program.cs"
    if (Test-Path $programFile) {
        Write-Host "Aktualisiere Program.cs für Design-Time..." -ForegroundColor Cyan
        
        $programContent = Get-Content $programFile -Raw
        
        # Check if design-time fix is already present
        if ($programContent -notmatch "GetDesignTimeServerVersion") {
            # Replace ServerVersion.AutoDetect in Program.cs
            $programAutoDetectPattern = 'ServerVersion\.AutoDetect\(connectionString\)'
            $programFixedVersion = 'GetDesignTimeServerVersion(connectionString)'
            
            if ($programContent -match $programAutoDetectPattern) {
                $programContent = $programContent -replace $programAutoDetectPattern, $programFixedVersion
                
                # Add helper method after the connection string declaration
                $connectionStringPattern = '(var connectionString = .*?;)'
                $programHelperMethod = @"
`$1

static ServerVersion GetDesignTimeServerVersion(string connectionString)
{
    try 
    {
        // Fix connection string for design-time (localhost instead of docker container)
        if (connectionString.Contains("Server=db;")) {
            connectionString = connectionString.Replace("Server=db;", "Server=localhost;");
        }
        return ServerVersion.AutoDetect(connectionString);
    }
    catch 
    {
        // Fallback for design-time when database is not available
        return ServerVersion.Create(Version.Parse("8.0.0"), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
    }
}
"@
                
                $programContent = $programContent -replace $connectionStringPattern, $programHelperMethod
                Set-Content -Path $programFile -Value $programContent -NoNewline
                Write-Host "✅ Program.cs für Design-Time aktualisiert" -ForegroundColor Green
            }
        } else {
            Write-Host "Program.cs bereits für Design-Time konfiguriert" -ForegroundColor Yellow
        }
    }
    
    Write-Host ""
    Write-Host "✅ EF Design-Time Konfiguration abgeschlossen!" -ForegroundColor Green
    
    Write-Host ""
    Write-Host "✨ Port-Update erfolgreich abgeschlossen!" -ForegroundColor Green
    
} catch {
    Write-Host "❌ Fehler beim Port-Update: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

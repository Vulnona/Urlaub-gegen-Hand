# UGH Port-Management System
Write-Host "=== UGH Port-Management System ===" -ForegroundColor Magenta
Write-Host "===================================" -ForegroundColor Magenta

try {
    # Read ports configuration
    $portsConfigPath = Join-Path $PSScriptRoot "ports.config"
    
    if (!(Test-Path $portsConfigPath)) {
        throw "ports.config nicht gefunden: $portsConfigPath"
    }
    
    $ports = @{}
    $content = Get-Content $portsConfigPath -ErrorAction Stop
    
    foreach ($line in $content) {
        if ($line -match '^([A-Z_]+)=(\d+)$') {
            $ports[$matches[1]] = $matches[2]
            Write-Host "Port gelesen: $($matches[1]) = $($matches[2])" -ForegroundColor Cyan
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
    
    Write-Host ""
    Write-Host "✨ Port-Update erfolgreich abgeschlossen!" -ForegroundColor Green
    
} catch {
    Write-Host "❌ Fehler beim Port-Update: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

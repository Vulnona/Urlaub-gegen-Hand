param(
    [switch]$DryRun = $false,
    [switch]$Verbose = $false,
    [switch]$Help = $false
)

function Test-Platform {
    if ($IsLinux -or $IsMacOS) {
        return "Unix"
    } else {
        return "Windows"
    }
}

# Show help if requested
if ($Help) {
    Write-Host "PORT UPDATE SCRIPT - Help" -ForegroundColor Green
    Write-Host "=========================" -ForegroundColor Green
    Write-Host ""
    Write-Host "BESCHREIBUNG:" -ForegroundColor Yellow
    Write-Host "  Automatisiert die Aktualisierung von Port-Konfigurationen in allen Projektdateien"
    Write-Host "  basierend auf der zentralen ports.config Datei."
    Write-Host ""
    Write-Host "USAGE:" -ForegroundColor Yellow
    if ((Test-Platform) -eq "Windows") {
        Write-Host "  .\update-ports.ps1 [-DryRun] [-Verbose] [-Help]"
    } else {
        Write-Host "  pwsh ./update-ports.ps1 [-DryRun] [-Verbose] [-Help]"
    }
    Write-Host ""
    Write-Host "PARAMETER:" -ForegroundColor Yellow
    Write-Host "  -DryRun   : Zeigt nur an, welche Aenderungen gemacht wuerden (ohne Ausfuehrung)"
    Write-Host "  -Verbose  : Aktiviert ausfuehrliche Ausgabe"
    Write-Host "  -Help     : Zeigt diese Hilfe an"
    Write-Host ""
    Write-Host "BEISPIELE:" -ForegroundColor Yellow
    if ((Test-Platform) -eq "Windows") {
        Write-Host "  .\update-ports.ps1                # Normale Ausfuehrung"
        Write-Host "  .\update-ports.ps1 -DryRun        # Vorschau ohne Aenderungen"
        Write-Host "  .\update-ports.ps1 -Verbose       # Mit ausfuehrlicher Ausgabe"
        Write-Host "  .\update-ports.ps1 -DryRun -Verbose  # Beides kombiniert"
    } else {
        Write-Host "  pwsh ./update-ports.ps1                # Normale Ausfuehrung"
        Write-Host "  pwsh ./update-ports.ps1 -DryRun        # Vorschau ohne Aenderungen"
        Write-Host "  pwsh ./update-ports.ps1 -Verbose       # Mit ausfuehrlicher Ausgabe"
        Write-Host "  pwsh ./update-ports.ps1 -DryRun -Verbose  # Beides kombiniert"
    }
    Write-Host ""
    exit 0
}

Write-Host "PORT UPDATE SCRIPT" -ForegroundColor Green
Write-Host "==================" -ForegroundColor Green

# Navigate to project root (two levels up from scripts/powershell)
$ScriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent (Split-Path -Parent $ScriptPath)
Set-Location $ProjectRoot

$PortsConfigPath = "ports.config"
if (-not (Test-Path $PortsConfigPath)) {
    Write-Host "Fehler: ports.config nicht gefunden" -ForegroundColor Red
    exit 1
}

Write-Host "Lade Port-Konfiguration..." -ForegroundColor Yellow

$PortConfig = @{}
Get-Content $PortsConfigPath | ForEach-Object {
    if ($_ -match '^([A-Z_]+)=(.+)$' -and -not $_.StartsWith('#')) {
        $key = $matches[1]
        $value = $matches[2]
        $PortConfig[$key] = $value
        if ($Verbose) {
            Write-Host "  -> Geladen: $key = $value" -ForegroundColor Cyan
        }
    }
}

$RequiredPorts = @('FRONTEND_PORT', 'BACKEND_PORT', 'DATABASE_PORT', 'WEBSERVER_PORT')
foreach ($port in $RequiredPorts) {
    if (-not $PortConfig.ContainsKey($port)) {
        Write-Host "Fehler: $port nicht in ports.config definiert!" -ForegroundColor Red
        exit 1
    }
}

Write-Host "Port-Konfiguration erfolgreich geladen" -ForegroundColor Green

function Update-DockerCompose {
    $FilePath = "compose.yaml"
    if (-not (Test-Path $FilePath)) {
        Write-Host "Warnung: $FilePath nicht gefunden - ueberspringe" -ForegroundColor Yellow
        return
    }

    Write-Host "Aktualisiere Docker Compose..." -ForegroundColor Yellow
    
    $Content = Get-Content $FilePath -Raw
    $OriginalContent = $Content
    
    # Frontend Port - einfache String-Ersetzung
    $Content = $Content -replace "3001:3001", "$($PortConfig['FRONTEND_PORT']):$($PortConfig['FRONTEND_PORT'])"
    
    # Backend Port - einfache String-Ersetzung  
    $Content = $Content -replace "8080:8080", "$($PortConfig['BACKEND_PORT']):$($PortConfig['BACKEND_PORT'])"
    
    # Webserver Port - einfache String-Ersetzung
    $Content = $Content -replace "81:81", "$($PortConfig['WEBSERVER_PORT']):$($PortConfig['WEBSERVER_INTERNAL_PORT'])"
    
    # ASPNETCORE_URLS
    $Content = $Content -replace "ASPNETCORE_URLS=http://\+:8080", "ASPNETCORE_URLS=http://+:$($PortConfig['BACKEND_PORT'])"
    
    if ($Content -ne $OriginalContent) {
        if (-not $DryRun) {
            $Content | Set-Content $FilePath -NoNewline
            Write-Host "   Docker Compose aktualisiert" -ForegroundColor Green
        } else {
            Write-Host "   Wuerde Docker Compose aktualisieren" -ForegroundColor Cyan
        }
    } else {
        Write-Host "   Docker Compose bereits aktuell" -ForegroundColor Cyan
    }
}

function Update-ViteConfig {
    $FilePath = "Frontend-Vuetify/vite.config.ts"
    if (-not (Test-Path $FilePath)) {
        Write-Host "Warnung: $FilePath nicht gefunden - ueberspringe..." -ForegroundColor Yellow
        return
    }

    Write-Host "Aktualisiere Vite Config..." -ForegroundColor Yellow
    
    $Content = Get-Content $FilePath -Raw
    $OriginalContent = $Content
    
    # Port im server Objekt - einfache String-Ersetzung
    $Content = $Content -replace "port: 3001,", "port: $($PortConfig['FRONTEND_PORT']),"
    
    if ($Content -ne $OriginalContent) {
        if (-not $DryRun) {
            $Content | Set-Content $FilePath -NoNewline
            Write-Host "   Vite Config aktualisiert" -ForegroundColor Green
        } else {
            Write-Host "   Wuerde Vite Config aktualisieren" -ForegroundColor Cyan
        }
    } else {
        Write-Host "   Vite Config bereits aktuell" -ForegroundColor Cyan
    }
}

function Update-ProductionServer {
    $FilePath = "Frontend-Vuetify/productionserver.js"
    if (-not (Test-Path $FilePath)) {
        Write-Host "Warnung: $FilePath nicht gefunden - ueberspringe" -ForegroundColor Yellow
        return
    }

    Write-Host "Aktualisiere Production Server..." -ForegroundColor Yellow
    
    $Content = Get-Content $FilePath -Raw
    $OriginalContent = $Content
    
    # PORT Konstante - einfache String-Ersetzung
    $Content = $Content -replace "const PORT = 3001;", "const PORT = $($PortConfig['FRONTEND_PORT']);"
    
    if ($Content -ne $OriginalContent) {
        if (-not $DryRun) {
            $Content | Set-Content $FilePath -NoNewline
            Write-Host "   Production Server aktualisiert" -ForegroundColor Green
        } else {
            Write-Host "   Wuerde Production Server aktualisieren" -ForegroundColor Cyan
        }
    } else {
        Write-Host "   Production Server bereits aktuell" -ForegroundColor Cyan
    }
}

function Update-BackendProgram {
    $FilePath = "Backend/Program.cs"
    if (-not (Test-Path $FilePath)) {
        Write-Host "Warnung: $FilePath nicht gefunden - ueberspringe" -ForegroundColor Yellow
        return
    }

    Write-Host "Aktualisiere Backend Program.cs..." -ForegroundColor Yellow
    
    $Content = Get-Content $FilePath -Raw
    $OriginalContent = $Content
    
    # CORS Origins - einfache String-Ersetzung
    $Content = $Content -replace "http://localhost:3001", "http://localhost:$($PortConfig['FRONTEND_PORT'])"
    
    if ($Content -ne $OriginalContent) {
        if (-not $DryRun) {
            $Content | Set-Content $FilePath -NoNewline
            Write-Host "   Backend Program.cs aktualisiert" -ForegroundColor Green
        } else {
            Write-Host "   Wuerde Backend Program.cs aktualisieren" -ForegroundColor Cyan
        }
    } else {
        Write-Host "   Backend Program.cs bereits aktuell" -ForegroundColor Cyan
    }
}

function Update-Documentation {
    Write-Host "Aktualisiere Dokumentation..." -ForegroundColor Yellow
    
    $ReadmePath = "README.md"
    if (Test-Path $ReadmePath) {
        $Content = Get-Content $ReadmePath -Raw
        $OriginalContent = $Content
        
        # Port-Referenzen in der Dokumentation - einfache String-Ersetzung
        $Content = $Content -replace "localhost:3001", "localhost:$($PortConfig['FRONTEND_PORT'])"
        $Content = $Content -replace "localhost:8080", "localhost:$($PortConfig['BACKEND_PORT'])"
        $Content = $Content -replace "localhost:81", "localhost:$($PortConfig['WEBSERVER_PORT'])"
        
        if ($Content -ne $OriginalContent) {
            if (-not $DryRun) {
                $Content | Set-Content $ReadmePath -NoNewline
                Write-Host "   README.md aktualisiert" -ForegroundColor Green
            } else {
                Write-Host "   Wuerde README.md aktualisieren" -ForegroundColor Cyan
            }
        } else {
            Write-Host "   README.md bereits aktuell" -ForegroundColor Cyan
        }
    }
}

function Update-NginxConfig {
    Write-Host "Aktualisiere Nginx-Konfiguration..." -ForegroundColor Yellow
    
    # Development vhost.dev.conf
    $DevConfigPath = ".docker\webserver\config\vhost.dev.conf"
    if (Test-Path $DevConfigPath) {
        $Content = Get-Content $DevConfigPath -Raw
        $OriginalContent = $Content
        
        # Frontend Proxy Pass - Update Port
        $Content = $Content -replace "frontend:3001", "frontend:$($PortConfig['FRONTEND_PORT'])"
        $Content = $Content -replace "frontend:3002", "frontend:$($PortConfig['FRONTEND_PORT'])"
        
        # Backend Proxy Pass - Update Port  
        $Content = $Content -replace "backend:8080", "backend:$($PortConfig['BACKEND_PORT'])"
        
        if ($Content -ne $OriginalContent) {
            if (-not $DryRun) {
                $Content | Set-Content $DevConfigPath -NoNewline
                Write-Host "   vhost.dev.conf aktualisiert" -ForegroundColor Green
            } else {
                Write-Host "   Wuerde vhost.dev.conf aktualisieren" -ForegroundColor Cyan
            }
        } else {
            Write-Host "   vhost.dev.conf bereits aktuell" -ForegroundColor Cyan
        }
    } else {
        Write-Host "Warnung: $DevConfigPath nicht gefunden - ueberspringe" -ForegroundColor Yellow
    }
    
    # Production vhost.conf
    $ProdConfigPath = ".docker\webserver\config\vhost.conf"
    if (Test-Path $ProdConfigPath) {
        $Content = Get-Content $ProdConfigPath -Raw
        $OriginalContent = $Content
        
        # Backend Proxy Pass - Update Port
        $Content = $Content -replace "localhost:8080", "localhost:$($PortConfig['BACKEND_PORT'])"
        
        if ($Content -ne $OriginalContent) {
            if (-not $DryRun) {
                $Content | Set-Content $ProdConfigPath -NoNewline
                Write-Host "   vhost.conf aktualisiert" -ForegroundColor Green
            } else {
                Write-Host "   Wuerde vhost.conf aktualisieren" -ForegroundColor Cyan
            }
        } else {
            Write-Host "   vhost.conf bereits aktuell" -ForegroundColor Cyan
        }
    } else {
        Write-Host "Warnung: $ProdConfigPath nicht gefunden - ueberspringe" -ForegroundColor Yellow
    }
}

if ($DryRun) {
    Write-Host "DRY RUN MODE - Keine Aenderungen werden vorgenommen" -ForegroundColor Yellow
    Write-Host ""
}

Update-DockerCompose
Update-ViteConfig
Update-ProductionServer
Update-BackendProgram
Update-NginxConfig
Update-Documentation

Write-Host ""
Write-Host "Port-Update abgeschlossen" -ForegroundColor Green

if (-not $DryRun) {
    Write-Host ""
    Write-Host "Naechste Schritte:" -ForegroundColor Yellow
    Write-Host "   1. Container neu starten: docker-compose down && docker-compose up --build" -ForegroundColor Cyan
    Write-Host "   2. Services testen:" -ForegroundColor Cyan
    Write-Host "      - Frontend: http://localhost:$($PortConfig['FRONTEND_PORT'])" -ForegroundColor Cyan
    Write-Host "      - Backend:  http://localhost:$($PortConfig['BACKEND_PORT'])" -ForegroundColor Cyan
    Write-Host "      - Webserver: http://localhost:$($PortConfig['WEBSERVER_PORT'])" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Aenderungen ueberpruefen: git diff" -ForegroundColor Cyan
} else {
    Write-Host ""
    Write-Host "Fuehre das Script ohne -DryRun aus, um die Aenderungen anzuwenden" -ForegroundColor Yellow
}

@echo off
REM UGH Quick Start für Entwickler (Batch-Version)
REM Startet das Projekt mit automatischer Datenbank-Initialisierung

setlocal enabledelayedexpansion

echo.
echo ========================================
echo    UGH Quick Start für Entwickler
echo ========================================
echo.

REM Prüfe Parameter
set RESET_MODE=0
if "%1"=="-Reset" set RESET_MODE=1
if "%1"=="-reset" set RESET_MODE=1
if "%1"=="--help" goto :show_help
if "%1"=="-h" goto :show_help
if "%1"=="-?" goto :show_help

REM Prüfe Voraussetzungen
echo Prüfe Voraussetzungen...
docker --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: Docker nicht gefunden. Bitte installieren Sie Docker Desktop.
    pause
    exit /b 1
)
echo ✓ Docker gefunden

docker-compose --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: Docker Compose nicht gefunden.
    pause
    exit /b 1
)
echo ✓ Docker Compose gefunden

if not exist "compose.yaml" (
    echo ERROR: compose.yaml nicht gefunden. Bitte führen Sie dieses Skript im Projektverzeichnis aus.
    pause
    exit /b 1
)
echo ✓ compose.yaml gefunden

REM Starte Services
echo.
echo Starte Services...

if %RESET_MODE%==1 (
    echo ⚠ Reset-Modus: Lösche alle Container und Volumes...
    docker-compose down -v
) else (
    docker-compose down
)

echo Starte Services im Hintergrund...
docker-compose up -d
if errorlevel 1 (
    echo ERROR: Fehler beim Starten der Services. Prüfen Sie die Docker-Logs.
    goto :show_error
)

echo ✓ Services gestartet

REM Warte auf Services
echo.
echo Warte auf Services...
set /a attempts=0
set /a max_attempts=30

:wait_loop
set /a attempts+=1
echo   Prüfe Services... (Versuch %attempts%/%max_attempts%)

REM Prüfe Datenbank
docker ps --format "table {{.Names}}\t{{.Status}}" | findstr "ugh-db" | findstr "healthy" >nul
if not errorlevel 1 (
    echo ✓ Datenbank ist bereit
    goto :show_success
)

if %attempts% geq %max_attempts% (
    echo ⚠ Timeout beim Warten auf Services
    goto :show_success
)

timeout /t 2 /nobreak >nul
goto :wait_loop

:show_success
echo.
echo ========================================
echo    UGH Projekt erfolgreich gestartet!
echo ========================================
echo.
echo Zugangsdaten:
echo   Frontend:     http://localhost:3002
echo   Backend API:  http://localhost:8081
echo   Nginx:        http://localhost:81
echo   Datenbank:    localhost:3306
echo.
echo Test-Accounts:
echo   Admin:        adminuser@example.com / password
echo   User:         test@example.com / password
echo   User 2:       test1@example.com / password
echo.
echo Nützliche Befehle:
echo   Logs anzeigen:     docker-compose logs -f
echo   Services stoppen:  docker-compose down
echo   Reset Datenbank:   quick-start.bat -Reset
echo.
echo Status der Services:
docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"
echo.
pause
exit /b 0

:show_error
echo.
echo ========================================
echo    Fehler beim Starten des Projekts
echo ========================================
echo.
echo Häufige Lösungen:
echo   1. Docker Desktop starten
echo   2. Ports prüfen (81, 3002, 8081, 3306)
echo   3. Docker-Logs prüfen: docker-compose logs
echo   4. Mit Reset versuchen: quick-start.bat -Reset
echo.
echo Docker-Logs:
docker-compose logs --tail=20
echo.
pause
exit /b 1

:show_help
echo UGH Quick Start für Entwickler (Batch-Version)
echo ==============================================
echo.
echo Usage:
echo   quick-start.bat
echo   quick-start.bat -Reset
echo.
echo Parameters:
echo   -Reset           Startet mit frischer Datenbank (löscht alle Daten)
echo.
echo Dieses Skript:
echo   1. Prüft Voraussetzungen (Docker, etc.)
echo   2. Startet alle Services mit docker-compose
echo   3. Wartet bis alle Services bereit sind
echo   4. Zeigt Zugangsdaten und URLs
echo.
pause
exit /b 0 
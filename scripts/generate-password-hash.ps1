# PowerShell Script für Passwort-Hashing
# Verwendet den gleichen Algorithmus wie die UGH-Anwendung (PBKDF2 mit HMAC-SHA256)
# Allgemeine Version - generiert Hash und Salt für beliebige Passwörter

param(
    [string]$Password = ""
)

# Funktion zum Generieren eines zufälligen Salts
function Generate-Salt {
    param([int]$Length = 32)
    
    $salt = New-Object byte[] $Length
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    $rng.GetBytes($salt)
    $rng.Dispose()
    
    return [Convert]::ToBase64String($salt)
}

# Funktion zum Hashen des Passworts (PBKDF2 mit HMAC-SHA256)
function Hash-Password {
    param(
        [string]$Password,
        [string]$Salt
    )
    
    $saltBytes = [Convert]::FromBase64String($Salt)
    $deriveBytes = New-Object System.Security.Cryptography.Rfc2898DeriveBytes($Password, $saltBytes, 10000, [System.Security.Cryptography.HashAlgorithmName]::SHA256)
    $hash = $deriveBytes.GetBytes(32)
    $deriveBytes.Dispose()
    
    return [Convert]::ToBase64String($hash)
}

# Hauptlogik
Write-Host "=== UGH Passwort-Hash Generator ===" -ForegroundColor Green
Write-Host "Allgemeine Version - generiert Hash und Salt" -ForegroundColor Cyan
Write-Host ""

# Passwort-Eingabe
if ([string]::IsNullOrEmpty($Password)) {
    $Password = Read-Host "Bitte geben Sie das Passwort ein"
}

if ([string]::IsNullOrEmpty($Password)) {
    Write-Host "Fehler: Kein Passwort eingegeben!" -ForegroundColor Red
    exit 1
}

Write-Host "Passwort wird verarbeitet..." -ForegroundColor Yellow

# Salt und Hash generieren
$salt = Generate-Salt
$hashedPassword = Hash-Password -Password $Password -Salt $salt

# Ergebnisse ausgeben
Write-Host ""
Write-Host "=== Ergebnisse ===" -ForegroundColor Green
Write-Host "Passwort: $Password" -ForegroundColor Cyan
Write-Host "Salt: $salt" -ForegroundColor Cyan
Write-Host "Hash: $hashedPassword" -ForegroundColor Cyan
Write-Host ""

# Zusätzliche Informationen
Write-Host "=== Technische Details ===" -ForegroundColor Green
Write-Host "Algorithmus: PBKDF2 mit HMAC-SHA256" -ForegroundColor White
Write-Host "Iterationen: 10.000" -ForegroundColor White
Write-Host "Salt-Länge: 32 Bytes (Base64-kodiert)" -ForegroundColor White
Write-Host "Hash-Länge: 32 Bytes (Base64-kodiert)" -ForegroundColor White
Write-Host ""

Write-Host "=== Fertig! ===" -ForegroundColor Green
Write-Host "Verwenden Sie Hash und Salt für Ihre Anwendung." -ForegroundColor Cyan 
# PowerShell script to generate password hash for admin
# This uses the same algorithm as the C# PasswordService

Add-Type -AssemblyName System.Security

function Generate-Salt {
    param([int]$length = 32)
    
    $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
    $salt = New-Object byte[] $length
    $rng.GetBytes($salt)
    $rng.Dispose()
    return [Convert]::ToBase64String($salt)
}

function Hash-Password {
    param(
        [string]$password,
        [string]$salt
    )
    
    $saltBytes = [Convert]::FromBase64String($salt)
    $pbkdf2 = New-Object System.Security.Cryptography.Rfc2898DeriveBytes($password, $saltBytes, 10000, [System.Security.Cryptography.HashAlgorithmName]::SHA256)
    $hash = $pbkdf2.GetBytes(32)
    $pbkdf2.Dispose()
    return [Convert]::ToBase64String($hash)
}

# Generate hash for admin123
$password = "admin123"
$salt = Generate-Salt
$hash = Hash-Password -password $password -salt $salt

Write-Host "Password: $password"
Write-Host "Salt: $salt"
Write-Host "Hash: $hash"
Write-Host ""
Write-Host "SQL Update Statement:"
Write-Host "UPDATE users SET Password = '$hash', SaltKey = '$salt' WHERE Email_Address = 'admin@gmail.com';"

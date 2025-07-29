using OtpNet;
using QRCoder;
using System.Security.Cryptography;
using System.Text.Json;

namespace UGHApi.Services
{
    public interface ITwoFactorAuthService
    {
        string GenerateSecret();
        string GenerateQrCodeUri(string email, string secret, string issuer = "UGH Platform");
        byte[] GenerateQrCode(string qrCodeUri);
        bool ValidateCode(string secret, string code);
        List<string> GenerateBackupCodes(int count = 10);
        bool ValidateBackupCode(string userBackupCodes, string providedCode);
        string ValidateBackupCodeWithMessage(string userBackupCodes, string providedCode);
        string RemoveUsedBackupCode(string userBackupCodes, string usedCode);
    }

    public class TwoFactorAuthService : ITwoFactorAuthService
    {
        public string GenerateSecret()
        {
            var key = KeyGeneration.GenerateRandomKey(20);
            return Base32Encoding.ToString(key);
        }

        public string GenerateQrCodeUri(string email, string secret, string issuer = "UGH Platform")
        {
            return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}?secret={secret}&issuer={Uri.EscapeDataString(issuer)}";
        }

        public byte[] GenerateQrCode(string qrCodeUri)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        public bool ValidateCode(string secret, string code)
        {
            if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(code))
                return false;

            try
            {
                var secretBytes = Base32Encoding.ToBytes(secret);
                var totp = new Totp(secretBytes);
                
                // Allow for time drift (30 seconds before and after)
                var currentTime = DateTime.UtcNow;
                for (int i = -1; i <= 1; i++)
                {
                    var timeStep = currentTime.AddSeconds(i * 30);
                    var expectedCode = totp.ComputeTotp(timeStep);
                    if (expectedCode == code)
                        return true;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<string> GenerateBackupCodes(int count = 10)
        {
            var backupCodes = new List<string>();
            using var rng = RandomNumberGenerator.Create();
            
            for (int i = 0; i < count; i++)
            {
                var bytes = new byte[4]; // 8 characters (4 bytes = 8 hex chars)
                rng.GetBytes(bytes);
                var code = Convert.ToHexString(bytes).ToLowerInvariant();
                backupCodes.Add(code);
            }
            
            return backupCodes;
        }

        public bool ValidateBackupCode(string userBackupCodes, string providedCode)
        {
            if (string.IsNullOrEmpty(userBackupCodes) || string.IsNullOrEmpty(providedCode))
                return false;

            try
            {
                var codes = JsonSerializer.Deserialize<List<string>>(userBackupCodes);
                // Clean the provided code (remove any formatting) and make case-insensitive
                var cleanProvidedCode = providedCode.Replace("-", "").Replace(" ", "").ToLowerInvariant();
                
                // Debug logging
                Console.WriteLine($"ValidateBackupCode - UserBackupCodes: {userBackupCodes}");
                Console.WriteLine($"ValidateBackupCode - ProvidedCode: {providedCode}");
                Console.WriteLine($"ValidateBackupCode - CleanProvidedCode: {cleanProvidedCode}");
                Console.WriteLine($"ValidateBackupCode - Codes count: {codes?.Count ?? 0}");
                if (codes != null)
                {
                    foreach (var code in codes)
                    {
                        Console.WriteLine($"ValidateBackupCode - Available code: {code}");
                    }
                }
                
                var result = codes?.Any(code => code.ToLowerInvariant() == cleanProvidedCode) == true;
                Console.WriteLine($"ValidateBackupCode - Result: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ValidateBackupCode - Exception: {ex.Message}");
                return false;
            }
        }

        public string ValidateBackupCodeWithMessage(string userBackupCodes, string providedCode)
        {
            if (string.IsNullOrEmpty(userBackupCodes) || string.IsNullOrEmpty(providedCode))
                return "Backup-Code ist erforderlich.";

            try
            {
                var codes = JsonSerializer.Deserialize<List<string>>(userBackupCodes);
                if (codes == null || codes.Count == 0)
                    return "Keine Backup-Codes verfügbar.";

                // Clean the provided code (remove any formatting) and make case-insensitive
                var cleanProvidedCode = providedCode.Replace("-", "").Replace(" ", "").ToLowerInvariant();
                
                var isValid = codes.Any(code => code.ToLowerInvariant() == cleanProvidedCode);
                
                if (isValid)
                    return "OK";
                else
                    return "Backup-Code wurde bereits verwendet oder ist ungültig.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ValidateBackupCodeWithMessage - Exception: {ex.Message}");
                return "Fehler bei der Backup-Code-Validierung.";
            }
        }

        public string RemoveUsedBackupCode(string userBackupCodes, string usedCode)
        {
            if (string.IsNullOrEmpty(userBackupCodes))
                return userBackupCodes;

            try
            {
                var codes = JsonSerializer.Deserialize<List<string>>(userBackupCodes);
                if (codes != null)
                {
                    // Clean the used code and find the matching one
                    var cleanUsedCode = usedCode.Replace("-", "").Replace(" ", "").ToLowerInvariant();
                    var codeToRemove = codes.FirstOrDefault(code => code.ToLowerInvariant() == cleanUsedCode);
                    if (codeToRemove != null)
                    {
                        codes.Remove(codeToRemove);
                        return JsonSerializer.Serialize(codes);
                    }
                }
            }
            catch
            {
                // Return original if deserialization fails
            }
            
            return userBackupCodes;
        }
    }
}

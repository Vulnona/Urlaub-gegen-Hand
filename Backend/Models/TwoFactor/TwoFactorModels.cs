namespace UGHApi.Models.TwoFactor
{
    public class Setup2FARequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class Setup2FAResponse
    {
        public string Secret { get; set; } = string.Empty;
        public string QrCodeUri { get; set; } = string.Empty;
        public byte[] QrCodeImage { get; set; } = Array.Empty<byte>();
        public List<string> BackupCodes { get; set; } = new List<string>();
    }

    public class Verify2FASetupRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }

    public class Verify2FARequest
    {
        public string Email { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public bool IsBackupCode { get; set; } = false;
    }

    public class Disable2FARequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginWith2FARequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TwoFactorCode { get; set; } = string.Empty;
        public bool IsBackupCode { get; set; } = false;
    }

    public class TwoFactorStatusResponse
    {
        public bool IsEnabled { get; set; }
        public int BackupCodesRemaining { get; set; }
    }
}

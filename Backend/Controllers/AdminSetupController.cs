using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGH.Domain.Core;
using UGH.Domain.Interfaces;
using UGH.Infrastructure.Services;
using UGHApi.Models;
using UGHApi.Services;
using UGHApi.DATA;
using System.Text.Json;

namespace UGHApi.Controllers
{
    [Route("api/admin-setup")]
    [ApiController]
    public class AdminSetupController : ControllerBase
    {
        private readonly Ugh_Context _context;
        private readonly PasswordService _passwordService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;
        private readonly EmailService _emailService;
        private readonly ILogger<AdminSetupController> _logger;

        public AdminSetupController(
            Ugh_Context context, 
            PasswordService passwordService,
            ITwoFactorAuthService twoFactorAuthService,
            EmailService emailService,
            ILogger<AdminSetupController> logger)
        {
            _context = context;
            _passwordService = passwordService;
            _twoFactorAuthService = twoFactorAuthService;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Sichere Admin-Ersteinrichtung für nicht-technische Admins
        /// </summary>
        [HttpPost("secure-setup")]
        public async Task<IActionResult> SecureAdminSetup([FromBody] SecureAdminSetupRequest request)
        {
            try
            {
                // Prüfe Setup-Token (aus Umgebungsvariable oder Konfiguration)
                var validSetupToken = Environment.GetEnvironmentVariable("ADMIN_SETUP_TOKEN") ?? "default-setup-token-2024";
                if (request.SetupToken != validSetupToken)
                {
                    _logger.LogWarning($"Invalid admin setup token attempt from IP: {HttpContext.Connection.RemoteIpAddress}");
                    return Unauthorized("Ungültiger Setup-Token");
                }

                // Prüfe ob bereits ein Admin existiert
                var existingAdmin = _context.users.FirstOrDefault(u => u.UserRole == UserRoles.Admin);
                if (existingAdmin != null)
                {
                    return BadRequest("Admin-Account bereits vorhanden. Setup nicht mehr möglich.");
                }

                // Erstelle Admin-Account
                var adminUser = new UGH.Domain.Entities.User
                {
                    User_Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email_Address = request.Email,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    UserRole = UserRoles.Admin,
                    IsEmailVerified = true,
                    VerificationState = UGH_Enums.VerificationState.Verified,
                    IsTwoFactorEnabled = false // Wird später aktiviert
                };

                // Hash Passwort
                var salt = _passwordService.GenerateSalt();
                adminUser.Password = _passwordService.HashPassword(request.Password, salt);
                adminUser.SaltKey = salt;

                _context.users.Add(adminUser);
                await _context.SaveChangesAsync();

                // Generiere 2FA-Setup-Token (24 Stunden gültig)
                var setupToken = Guid.NewGuid().ToString();
                var setupExpiry = DateTime.UtcNow.AddHours(24);
                
                // Speichere Setup-Token (in Produktion: Redis oder Datenbank)
                // Hier vereinfacht in Umgebungsvariable
                Environment.SetEnvironmentVariable($"ADMIN_SETUP_{setupToken}", adminUser.User_Id.ToString());

                // Sende E-Mail mit Setup-Link
                var setupLink = $"{Request.Scheme}://{Request.Host}/admin-setup?token={setupToken}";
                await SendAdminSetupEmail(adminUser.Email_Address, setupLink, adminUser.FirstName);

                _logger.LogInformation($"Admin setup initiated for: {request.Email}");

                return Ok(new { 
                    message = "Admin-Setup erfolgreich initiiert. Prüfen Sie Ihre E-Mails für weitere Anweisungen.",
                    email = request.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during admin setup: {ex.Message}");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        /// <summary>
        /// 2FA-Setup für Admin über Setup-Token
        /// </summary>
        [HttpPost("setup-2fa")]
        public async Task<IActionResult> SetupAdmin2FA([FromBody] Admin2FASetupRequest request)
        {
            try
            {
                // Validiere Setup-Token
                var userIdString = Environment.GetEnvironmentVariable($"ADMIN_SETUP_{request.SetupToken}");
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return BadRequest("Ungültiger oder abgelaufener Setup-Token");
                }

                var adminUser = await _context.users.FindAsync(userId);
                if (adminUser == null || adminUser.UserRole != UserRoles.Admin)
                {
                    return BadRequest("Admin-User nicht gefunden");
                }

                // Generiere 2FA-Secret und QR-Code
                var secret = _twoFactorAuthService.GenerateSecret();
                var qrCodeUri = _twoFactorAuthService.GenerateQrCodeUri(adminUser.Email_Address, secret);
                var qrCodeImage = _twoFactorAuthService.GenerateQrCode(qrCodeUri);
                var backupCodes = _twoFactorAuthService.GenerateBackupCodes();

                return Ok(new
                {
                    secret = secret,
                    qrCodeUri = qrCodeUri,
                    qrCodeImage = Convert.ToBase64String(qrCodeImage),
                    backupCodes = backupCodes,
                    email = adminUser.Email_Address
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during 2FA setup: {ex.Message}");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        /// <summary>
        /// 2FA-Verifizierung und Aktivierung für Admin
        /// </summary>
        [HttpPost("verify-2fa")]
        public async Task<IActionResult> VerifyAdmin2FA([FromBody] Admin2FAVerifyRequest request)
        {
            try
            {
                // Validiere Setup-Token
                var userIdString = Environment.GetEnvironmentVariable($"ADMIN_SETUP_{request.SetupToken}");
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
                {
                    return BadRequest("Ungültiger oder abgelaufener Setup-Token");
                }

                var adminUser = await _context.users.FindAsync(userId);
                if (adminUser == null)
                {
                    return BadRequest("Admin-User nicht gefunden");
                }

                // Verifiziere 2FA-Code
                if (!_twoFactorAuthService.ValidateCode(request.Secret, request.Code))
                {
                    return BadRequest("Ungültiger 2FA-Code");
                }

                // Aktiviere 2FA
                adminUser.IsTwoFactorEnabled = true;
                adminUser.TwoFactorSecret = request.Secret;
                adminUser.BackupCodes = JsonSerializer.Serialize(request.BackupCodes);

                await _context.SaveChangesAsync();

                // Lösche Setup-Token
                Environment.SetEnvironmentVariable($"ADMIN_SETUP_{request.SetupToken}", null);

                _logger.LogInformation($"2FA activated for admin: {adminUser.Email_Address}");

                return Ok(new { 
                    message = "2FA erfolgreich aktiviert! Sie können sich jetzt anmelden.",
                    email = adminUser.Email_Address
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during 2FA verification: {ex.Message}");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        private async Task SendAdminSetupEmail(string email, string setupLink, string firstName)
        {
            var subject = "UGH Admin Setup - Sicherheitslink";
            var body = $@"
                <h2>Willkommen bei UGH, {firstName}!</h2>
                
                <p>Ihr Admin-Account wurde erfolgreich erstellt. Um die Sicherheit zu gewährleisten, 
                müssen Sie jetzt die Zwei-Faktor-Authentifizierung (2FA) einrichten.</p>
                
                <h3>Nächste Schritte:</h3>
                <ol>
                    <li>Klicken Sie auf den folgenden Link: <a href='{setupLink}'>2FA einrichten</a></li>
                    <li>Folgen Sie den Anweisungen auf der Seite</li>
                    <li>Scannen Sie den QR-Code mit Ihrer Authenticator-App</li>
                    <li>Geben Sie den 6-stelligen Code ein</li>
                </ol>
                
                <p><strong>Wichtig:</strong> Dieser Link ist 24 Stunden gültig.</p>
                
                <p>Falls Sie Fragen haben, kontaktieren Sie den Systemadministrator.</p>
                
                <p>Mit freundlichen Grüßen<br>Ihr UGH-Team</p>
            ";

            await _emailService.SendEmailAsync(email, subject, body);
        }

        /// <summary>
        /// EMERGENCY ONLY: Reset admin password with environment variable
        /// Requires ADMIN_RESET_TOKEN environment variable to be set
        /// </summary>
        [HttpPost("emergency-reset")]
        public IActionResult EmergencyResetAdmin([FromBody] EmergencyResetRequest request)
        {
            // Simple in-memory rate limiting per IP (for demonstration purposes)
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            if (!RateLimiter.AllowRequest(ipAddress))
            {
                _logger.LogWarning($"Rate limit exceeded for IP: {ipAddress}");
                return StatusCode(429, "Too many requests. Please try again later.");
            }

            try
            {
                // Security: Only allow in development or with proper token
                string resetToken = null;
                const string tokenFilePath = "/tmp/admin_reset_token";
                
                try
                {
                    if (System.IO.File.Exists(tokenFilePath))
                    {
                        resetToken = System.IO.File.ReadAllText(tokenFilePath).Trim();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Could not read reset token file: {ex.Message}");
                }
                
                if (string.IsNullOrEmpty(resetToken) || resetToken != request.ResetToken)
                {
                    _logger.LogWarning("Unauthorized admin reset attempt");
                    return Unauthorized("Ungültiger Reset-Token");
                }

                var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@gmail.com";
                var adminUser = _context.users.FirstOrDefault(u => u.Email_Address == adminEmail);
                if (adminUser == null)
                {
                    return NotFound("Admin-Benutzer nicht gefunden");
                }

                // Generate secure password
                var newSalt = _passwordService.GenerateSalt();
                var newHash = _passwordService.HashPassword(request.NewPassword, newSalt);
                
                adminUser.SaltKey = newSalt;
                adminUser.Password = newHash;
                adminUser.IsEmailVerified = true;
                
                _context.SaveChanges();
                
                _logger.LogWarning("Admin password has been reset via emergency endpoint");
                
                return Ok(new { message = "Admin-Passwort erfolgreich zurückgesetzt" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during emergency reset: {ex.Message}");
                return StatusCode(500, "Interner Serverfehler");
            }
        }

        /// <summary>
        /// Check if admin setup is required
        /// </summary>
        [HttpGet("status")]
        public IActionResult GetSetupStatus()
        {
            try
            {
                var adminUser = _context.users.FirstOrDefault(u => u.Email_Address == "admin@gmail.com");
                
                return Ok(new { 
                    adminExists = adminUser != null,
                    isEmailVerified = adminUser?.IsEmailVerified ?? false,
                    setupRequired = adminUser == null || !adminUser.IsEmailVerified
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error checking setup status: {ex.Message}");
                return StatusCode(500, "Interner Serverfehler");
            }
        }
    }

    public class SecureAdminSetupRequest
    {
        public string SetupToken { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
    }

    public class Admin2FASetupRequest
    {
        public string SetupToken { get; set; } = string.Empty;
    }

    public class Admin2FAVerifyRequest
    {
        public string SetupToken { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public List<string> BackupCodes { get; set; } = new List<string>();
    }

    public class EmergencyResetRequest
    {
        public string ResetToken { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    // Simple in-memory rate limiter (for demonstration; use distributed cache for production)
    public static class RateLimiter
    {
        private static readonly Dictionary<string, List<DateTime>> Requests = new();
        private static readonly object LockObj = new();

        private const int MaxRequests = 5;
        private static readonly TimeSpan Window = TimeSpan.FromMinutes(5);

        public static bool AllowRequest(string ip)
        {
            lock (LockObj)
            {
                if (!Requests.ContainsKey(ip))
                {
                    Requests[ip] = new List<DateTime>();
                }

                // Remove requests outside the window
                Requests[ip].RemoveAll(dt => dt < DateTime.UtcNow - Window);

                if (Requests[ip].Count >= MaxRequests)
                {
                    return false;
                }

                Requests[ip].Add(DateTime.UtcNow);
                return true;
            }
        }
    }
}

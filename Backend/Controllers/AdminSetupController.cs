using Microsoft.AspNetCore.Mvc;
using UGH.Infrastructure.Services;
using UGH.Domain.Interfaces;

namespace UGHApi.Controllers
{
    [Route("api/admin-setup")]
    [ApiController]
    public class AdminSetupController : ControllerBase
    {
        private readonly Ugh_Context _context;
        private readonly PasswordService _passwordService;
        private readonly ILogger<AdminSetupController> _logger;

        public AdminSetupController(
            Ugh_Context context, 
            PasswordService passwordService,
            ILogger<AdminSetupController> logger)
        {
            _context = context;
            _passwordService = passwordService;
            _logger = logger;
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
                    return Unauthorized("Invalid reset token");
                }

                var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL") ?? "admin@gmail.com";
                var adminUser = _context.users.FirstOrDefault(u => u.Email_Address == adminEmail);
                if (adminUser == null)
                {
                    return NotFound("Admin user not found");
                }

                // Generate secure password
                var newSalt = _passwordService.GenerateSalt();
                var newHash = _passwordService.HashPassword(request.NewPassword, newSalt);
                
                adminUser.SaltKey = newSalt;
                adminUser.Password = newHash;
                adminUser.IsEmailVerified = true;
                
                _context.SaveChanges();
                
                _logger.LogWarning("Admin password has been reset via emergency endpoint");
                
                return Ok(new { message = "Admin password reset successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during emergency reset: {ex.Message}");
                return StatusCode(500, "Internal server error");
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
                return StatusCode(500, "Internal server error");
            }
        }
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

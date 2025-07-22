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
            try
            {
                // Security: Only allow in development or with proper token
                var resetToken = Environment.GetEnvironmentVariable("ADMIN_RESET_TOKEN");
                
                if (string.IsNullOrEmpty(resetToken) || resetToken != request.ResetToken)
                {
                    _logger.LogWarning("Unauthorized admin reset attempt");
                    return Unauthorized("Invalid reset token");
                }

                var adminUser = _context.users.FirstOrDefault(u => u.Email_Address == "admin@gmail.com");
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
}

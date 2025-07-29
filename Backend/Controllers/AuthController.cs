using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Authentication;
using UGH.Contracts.Authentication;
using UGHApi.Applications.Authentication;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using UGH.Domain.Interfaces;
using UGH.Domain.Entities;
using UGHApi.Services;
using UGHApi.Models.TwoFactor;
using System.Text.Json;
using UGHApi.DATA;

namespace UGHApi.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Ugh_Context _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly IUserProvider _userProvider;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly PasswordService _passwordService;
        private readonly UserService _userService;
        private readonly ITwoFactorAuthService _twoFactorAuthService;        
        private readonly IConfiguration _configuration;

        public AuthController(Ugh_Context context, ILogger<AuthController> logger, IMediator mediator, TokenService tokenService, IUserRepository userRepository, EmailService emailService, PasswordService passwordService,IUserProvider userProvider, UserService userService, ITwoFactorAuthService twoFactorAuthService, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _tokenService = tokenService;
            _emailService = emailService;
            _userRepository = userRepository;
            _passwordService = passwordService;
            _userProvider = userProvider;
            _userService = userService;
            _twoFactorAuthService = twoFactorAuthService;
            _configuration = configuration;
        }

        #region user-authorization
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _mediator.Send(command);

                if (response.IsFailure)
                {
                    return BadRequest(new { Error = response.Error });
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Conflict: {ex.Message}");
                return Conflict(new { errorMessage = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            try
            {
                _logger.LogError($"=== AUTH CONTROLLER LOGIN CALLED FOR {command.Email} ===");
                _logger.LogError($"[DEBUG] LoginCommand received: Email={command.Email}, Password={command.Password}");

                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Bad request: {ModelState}");
                    return BadRequest(ModelState);
                }

                var response = await _mediator.Send(command);

                _logger.LogError($"=== MEDIATOR RESPONSE FOR {command.Email}: IsFailure={response.IsFailure} ===");

                if (response.IsFailure)
                {
                    _logger.LogError($"unauthorized: {response.Error.Message}");
                    return Unauthorized(
                        new { Code = response.Error.Code, Message = response.Error.Message }
                    );
                }

                return Ok(response.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> Verify([Required] string token)
        {
            try
            {
                var (htmlContent, mimeType) = await _mediator.Send(new VerifyEmailCommand(token));
                return Content(htmlContent, mimeType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception: {ex.Message} | {ex.StackTrace}");
                return Content(
                    $"<html><body><h1>Internal Server Error</h1><p>{ex.Message}</p></body></html>",
                    "text/html"
                );
            }
        }

        [HttpPost("refresh-request-token")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            try
            {
                var command = new RefreshTokenCommand(request.RefreshToken);
                var result = await _mediator.Send(command);

                if (result.IsFailure)
                {
                    return Unauthorized(result.Error);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("resend-email-verification")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendEmailVerification resendUrl)
        {
            try
            {
                if (!ModelState.IsValid)                
                    return BadRequest(ModelState);
                
                if (string.IsNullOrEmpty(resendUrl.Email) || !_emailService.IsValidEmail(resendUrl.Email))
                    return BadRequest();
                
                var user = await _userRepository.GetUserByEmailAsync(resendUrl.Email);
                if (user == null)
                    return NotFound();
                
                var verificationToken = _tokenService.GenerateNewEmailVerificator(user.User_Id);
                var emailSent = await _emailService.SendVerificationEmailAsync(resendUrl.Email, verificationToken);
                if (!emailSent)
                    return StatusCode(500, "Fehler beim Senden der Verifizierungs-E-Mail.");

                return Ok("Verification email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResendEmailVerification resendUrl)
        {
            try
            {
                if (!ModelState.IsValid)                
                    return BadRequest(ModelState);
                
                if (string.IsNullOrEmpty(resendUrl.Email) || !_emailService.IsValidEmail(resendUrl.Email))
                    return BadRequest();
                
                var user = await _userRepository.GetUserByEmailAsync(resendUrl.Email);
                if (user == null)
                    return NotFound();
                
                var token = _tokenService.GenerateNewPasswordResetToken(user.User_Id);
                var emailSent = await _emailService.SendResetPasswordEmailAsync(resendUrl.Email, token);
                if (!emailSent)
                    return StatusCode(500, "Fehler beim Senden der E-Mail.");

                return Ok("Passwort-Reset-Token erfolgreich gesendet.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Interner Serverfehler: {ex.Message}");
            }
        }

        public class Password{
            public String NewPassword{get; set;}
            public String? Token{get; set;}
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] Password pw)
        {            
            User user = null;
            bool isBackupCodePasswordChange = false;
            
            // First try to get user from current context (for backup code cases)
            var userId = _userProvider.UserId;
            if (userId != Guid.Empty)
            {
                user = await _context.users.FindAsync(userId);
                // If user found via context and no token provided, this is a backup code password change
                if (user != null && string.IsNullOrEmpty(pw.Token))
                {
                    isBackupCodePasswordChange = true;
                    _logger.LogInformation($"Backup code password change detected for user: {user.Email_Address}");
                }
            }
            
            // If no user found and token provided, try token-based reset
            if (user == null && !string.IsNullOrEmpty(pw.Token))
            {
                user = await _userService.GetUserByPasswordResetTokenAsync(pw.Token);
            }
            
            if (user == null)
                return BadRequest("User not found or invalid token");                           
            
            try {                
                var salt = _passwordService.GenerateSalt();
                user.Password =  _passwordService.HashPassword(pw.NewPassword, salt);
                user.SaltKey = salt;
                
                // If this is a backup code password change, reset 2FA
                if (isBackupCodePasswordChange)
                {
                    _logger.LogInformation($"Resetting 2FA for backup code password change: {user.Email_Address}");
                    user.IsTwoFactorEnabled = false;
                    user.TwoFactorSecret = null;
                    user.BackupCodes = null;
                }
                
                await _userRepository.UpdateUserAsync(user);
                return Ok("Passwort erfolgreich geändert");
            } catch (Exception ex) {
                _logger.LogError($"Error changing password: {ex.Message}");
                return StatusCode(500, new { Message = "Fehler beim Ändern des Passworts"});
            }            
        }

        
        
        [HttpPost("upload-id")]
        public async Task<IActionResult> UploadFile(
            [FromQuery] Guid userId,
            IFormFile fileRS,
            IFormFile fileVS
        )
        {
            try
            {
                _logger.LogInformation($"[DEBUG] AuthController: Upload-ID Request für UserId={userId}");
                _logger.LogInformation($"[DEBUG] AuthController: FileRS={fileRS?.FileName}, Größe={fileRS?.Length}, ContentType={fileRS?.ContentType}");
                _logger.LogInformation($"[DEBUG] AuthController: FileVS={fileVS?.FileName}, Größe={fileVS?.Length}, ContentType={fileVS?.ContentType}");

                // Validate input parameters
                if (userId == Guid.Empty)
                {
                    _logger.LogError("[DEBUG] AuthController: UserId ist leer");
                    return BadRequest("Ungültige User-ID");
                }

                if (fileRS == null)
                {
                    _logger.LogError("[DEBUG] AuthController: FileRS ist null");
                    return BadRequest("Rückseite des Ausweises ist erforderlich");
                }

                if (fileVS == null)
                {
                    _logger.LogError("[DEBUG] AuthController: FileVS ist null");
                    return BadRequest("Vorderseite des Ausweises ist erforderlich");
                }

                var command = new UploadFilesCommand(fileRS, fileVS, userId);
                var result = await _mediator.Send(command);

                _logger.LogInformation($"[DEBUG] AuthController: Upload erfolgreich abgeschlossen für UserId={userId}");
                return Ok(new { result.LinkRS, result.LinkVS });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning($"[DEBUG] AuthController: Validierungsfehler beim Upload-ID: {ex.Message}");
                return BadRequest(new { errorMessage = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"[DEBUG] AuthController: Fehler beim Upload-ID: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, new { errorMessage = "Beim Hochladen ist ein technischer Fehler aufgetreten. Bitte kontaktiere den Support." });
            }
        }

        [HttpGet("test-upload")]
        public IActionResult TestUpload()
        {
            return Ok(new { 
                message = "Upload-Endpoint ist verfügbar",
                timestamp = DateTime.UtcNow,
                status = "ready"
            });
        }

        #endregion

        #region two-factor-authentication

        [HttpPost("2fa/setup")]
        public async Task<IActionResult> Setup2FA([FromBody] Setup2FARequest request)
        {
            try
            {
                _logger.LogInformation($"2FA setup request received for email: {request?.Email}");
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"ModelState is invalid: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return NotFound("User not found");

                if (user.IsTwoFactorEnabled)
                {
                    _logger.LogWarning($"2FA setup attempted for user with already enabled 2FA: {request.Email}");
                    return BadRequest("2FA is already enabled for this user");
                }

                // For Admin accounts, allow 2FA setup without authentication (recovery scenario)
                if (user.UserRole == UserRoles.Admin)
                {
                    _logger.LogInformation($"Mandatory 2FA setup for admin user: {request.Email} (recovery mode)");
                }

                var secret = _twoFactorAuthService.GenerateSecret();
                var qrCodeUri = _twoFactorAuthService.GenerateQrCodeUri(user.Email_Address, secret);
                var qrCodeImage = _twoFactorAuthService.GenerateQrCode(qrCodeUri);
                var backupCodes = _twoFactorAuthService.GenerateBackupCodes();

                var response = new Setup2FAResponse
                {
                    Secret = secret,
                    QrCodeUri = qrCodeUri,
                    QrCodeImage = qrCodeImage,
                    BackupCodes = backupCodes
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during 2FA setup: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("2fa/reset")]
        public async Task<IActionResult> Reset2FA([FromBody] Setup2FARequest request)
        {
            try
            {
                _logger.LogInformation($"2FA reset request received for email: {request?.Email}");
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning($"ModelState is invalid: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return NotFound("User not found");

                // Store current 2FA settings for potential rollback
                var oldTwoFactorEnabled = user.IsTwoFactorEnabled;
                var oldTwoFactorSecret = user.TwoFactorSecret;
                var oldBackupCodes = user.BackupCodes;

                // Generate new 2FA settings but DON'T save them yet
                var newSecret = _twoFactorAuthService.GenerateSecret();
                var newBackupCodes = _twoFactorAuthService.GenerateBackupCodes();
                
                // Store new settings temporarily in session or cache
                // For now, we'll store them in the response and handle them in verify-setup
                var qrCodeUri = _twoFactorAuthService.GenerateQrCodeUri(user.Email_Address, newSecret);
                var qrCodeImage = _twoFactorAuthService.GenerateQrCode(qrCodeUri);

                var response = new Setup2FAResponse
                {
                    Secret = newSecret,
                    QrCodeUri = qrCodeUri,
                    QrCodeImage = qrCodeImage,
                    BackupCodes = newBackupCodes,
                    // Include old settings for rollback
                    OldTwoFactorEnabled = oldTwoFactorEnabled,
                    OldTwoFactorSecret = oldTwoFactorSecret,
                    OldBackupCodes = oldBackupCodes
                };

                _logger.LogInformation($"2FA reset setup completed for user: {request.Email} - old settings preserved");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during 2FA reset: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("2fa/verify-setup")]
        public async Task<IActionResult> Verify2FASetup([FromBody] Verify2FASetupRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return NotFound("Benutzer nicht gefunden");

                // Check if this is a reset scenario (old settings provided)
                bool isResetScenario = !string.IsNullOrEmpty(request.OldTwoFactorSecret);
                
                if (user.IsTwoFactorEnabled && !isResetScenario)
                    return BadRequest("2FA ist für diesen Benutzer bereits aktiviert");

                if (!_twoFactorAuthService.ValidateCode(request.Secret, request.Code))
                    return BadRequest("Ungültiger Verifizierungscode");

                // Save new 2FA settings to user
                user.IsTwoFactorEnabled = true;
                user.TwoFactorSecret = request.Secret;
                
                // Generate and save new backup codes
                var backupCodes = _twoFactorAuthService.GenerateBackupCodes();
                user.BackupCodes = JsonSerializer.Serialize(backupCodes);

                await _userRepository.UpdateUserAsync(user);

                _logger.LogInformation($"2FA setup completed successfully for user: {request.Email}");

                return Ok(new { Message = "2FA enabled successfully", BackupCodes = backupCodes });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during 2FA verification: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login-2fa")]
        public async Task<IActionResult> LoginWith2FA([FromBody] LoginWith2FARequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Prüfe das temporäre 2FA-Token
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var key = System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                try
                {
                    var validationParams = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = true
                    };
                    var principal = tokenHandler.ValidateToken(request.TwoFactorToken, validationParams, out var validatedToken);
                    var tokenType = principal.Claims.FirstOrDefault(x => x.Type == "TokenType")?.Value;
                    var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(emailClaim))
                        emailClaim = principal.Claims.FirstOrDefault(x => x.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
                    
                    _logger.LogInformation($"Token validation - Type: {tokenType}, UserId: {userIdClaim}, Email: {emailClaim}");
                    
                    if (tokenType != "2fa")
                        return BadRequest("Invalid 2FA token type");
                    if (userIdClaim == null || emailClaim == null)
                        return BadRequest("Invalid 2FA token claims");
                    // Prüfe, ob die UserId und Email mit dem Request übereinstimmen
                    var userFromEmail = await _userRepository.GetUserByEmailAsync(request.Email);
                    if (userIdClaim != null && request.Email != null && userFromEmail != null && userIdClaim != userFromEmail.User_Id.ToString())
                        return BadRequest("Token/User mismatch");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"2FA token validation failed: {ex.Message}");
                    _logger.LogError($"Token: {request.TwoFactorToken}");
                    _logger.LogError($"Email: {request.Email}");
                    return BadRequest("Invalid or expired 2FA token");
                }

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return BadRequest("Invalid credentials");

                // Passwortprüfung entfällt, da sie bereits im ersten Schritt geprüft wurde

                // Check if 2FA is enabled
                if (!user.IsTwoFactorEnabled)
                    return BadRequest("2FA is not enabled for this user");

                // Verify 2FA code
                bool isValid = false;
                bool isBackupCodeUsed = false;
                
                _logger.LogInformation($"2FA verification - IsBackupCode: {request.IsBackupCode}, Code: {request.TwoFactorCode}, UserBackupCodes: {user.BackupCodes}");
                
                if (request.IsBackupCode)
                {
                    // Check if backup code attempts are locked
                    if (user.IsBackupCodeLocked)
                    {
                        var lockoutDuration = TimeSpan.FromMinutes(30); // 30 minutes lockout
                        if (user.LastFailedBackupCodeAttempt.HasValue && 
                            DateTime.UtcNow - user.LastFailedBackupCodeAttempt.Value < lockoutDuration)
                        {
                            var remainingTime = lockoutDuration - (DateTime.UtcNow - user.LastFailedBackupCodeAttempt.Value);
                            return BadRequest($"Zu viele fehlgeschlagene Backup-Code-Versuche. Account für {remainingTime.Minutes} Minuten und {remainingTime.Seconds} Sekunden gesperrt.");
                        }
                        else
                        {
                            // Reset lockout after duration
                            user.IsBackupCodeLocked = false;
                            user.FailedBackupCodeAttempts = 0;
                            user.LastFailedBackupCodeAttempt = null;
                        }
                    }

                    var backupCodeMessage = _twoFactorAuthService.ValidateBackupCodeWithMessage(user.BackupCodes, request.TwoFactorCode);
                    isValid = backupCodeMessage == "OK";
                    _logger.LogInformation($"Backup code validation result: {isValid}, Message: {backupCodeMessage}");
                    
                    if (isValid)
                    {
                        // Reset failed attempts on successful login
                        user.FailedBackupCodeAttempts = 0;
                        user.LastFailedBackupCodeAttempt = null;
                        user.IsBackupCodeLocked = false;
                        
                        user.BackupCodes = _twoFactorAuthService.RemoveUsedBackupCode(user.BackupCodes, request.TwoFactorCode);
                        await _userRepository.UpdateUserAsync(user);
                        isBackupCodeUsed = true;
                        
                        // Log backup code usage for security monitoring
                        _logger.LogWarning($"Backup code used for user: {user.Email_Address}. Remaining codes: {user.BackupCodes?.Split(',').Length ?? 0}");
                    }
                    else
                    {
                        // Increment failed attempts for ANY invalid backup code (including "already used")
                        user.FailedBackupCodeAttempts++;
                        user.LastFailedBackupCodeAttempt = DateTime.UtcNow;
                        
                        // Lock account after 10 failed attempts
                        if (user.FailedBackupCodeAttempts >= 10)
                        {
                            user.IsBackupCodeLocked = true;
                            _logger.LogWarning($"Backup code brute force detected for user: {user.Email_Address}. Account locked for 30 minutes.");
                        }
                        
                        await _userRepository.UpdateUserAsync(user);
                        
                        // Return specific error message for backup code issues
                        if (user.IsBackupCodeLocked)
                        {
                            return BadRequest("Zu viele fehlgeschlagene Backup-Code-Versuche. Account für 30 Minuten gesperrt. Versuchen Sie es später erneut.");
                        }
                        else
                        {
                            var remainingAttempts = 10 - user.FailedBackupCodeAttempts;
                            return BadRequest(new { 
                                message = $"Ungültiger Backup-Code. Noch {remainingAttempts} Versuche verfügbar.",
                                remainingAttempts = remainingAttempts
                            });
                        }
                    }
                }
                else
                {
                    isValid = _twoFactorAuthService.ValidateCode(user.TwoFactorSecret, request.TwoFactorCode);
                    _logger.LogInformation($"TOTP code validation result: {isValid}");
                }

                if (!isValid)
                    return BadRequest("Invalid 2FA code");

                // Generate JWT token with correct memberships
                var activeMemberships = await _userRepository.GetActiveUserMembershipsAsync(user.User_Id);
                var token = _tokenService.GenerateJwtToken(user, activeMemberships);

                var response = new LoginResponse
                {
                    UserId = user.User_Id,
                    Email = user.Email_Address,
                    AccessToken = token,
                    FirstName = user.FirstName,
                    UserRole = user.UserRole.ToString()
                };

                // Add warning if backup code was used
                if (isBackupCodeUsed)
                {
                    response.Message = "Backup-Code verwendet. Bitte richten Sie 2FA neu ein für maximale Sicherheit.";
                    response.Requires2FAReset = true;
                    
                    // DO NOT reset 2FA automatically - let user choose whether to reset or continue
                    _logger.LogInformation($"Backup code used for user {user.Email_Address} - 2FA remains active until user chooses to reset");
                }

                _logger.LogInformation($"2FA login successful for user: {user.Email_Address}. Response: {System.Text.Json.JsonSerializer.Serialize(response)}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during 2FA login: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("2fa/disable")]
        public async Task<IActionResult> Disable2FA([FromBody] Disable2FARequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return NotFound("User not found");

                if (!user.IsTwoFactorEnabled)
                    return BadRequest("2FA is not enabled for this user");

                // Admin users cannot disable 2FA
                if (user.UserRole == UserRoles.Admin)
                {
                    _logger.LogWarning($"Admin user {user.Email_Address} attempted to disable 2FA");
                    return BadRequest("Admin accounts must keep 2FA enabled for security reasons");
                }

                // Verify password
                if (!_passwordService.VerifyPassword(request.Password, user.Password, user.SaltKey))
                    return BadRequest("Invalid password");

                // Disable 2FA
                user.IsTwoFactorEnabled = false;
                user.TwoFactorSecret = null;
                user.BackupCodes = null;

                await _userRepository.UpdateUserAsync(user);

                return Ok(new { Message = "2FA disabled successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during 2FA disable: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("2fa/status")]
        public async Task<IActionResult> Get2FAStatus([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return BadRequest("Email is required");

                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                    return NotFound("User not found");

                var backupCodesCount = 0;
                if (!string.IsNullOrEmpty(user.BackupCodes))
                {
                    try
                    {
                        var codes = JsonSerializer.Deserialize<List<string>>(user.BackupCodes);
                        backupCodesCount = codes?.Count ?? 0;
                    }
                    catch
                    {
                        backupCodesCount = 0;
                    }
                }

                return Ok(new TwoFactorStatusResponse
                {
                    IsEnabled = user.IsTwoFactorEnabled,
                    BackupCodesRemaining = backupCodesCount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred getting 2FA status: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion
    }
}

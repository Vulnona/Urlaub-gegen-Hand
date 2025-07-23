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

        public AuthController(Ugh_Context context, ILogger<AuthController> logger, IMediator mediator, TokenService tokenService, IUserRepository userRepository, EmailService emailService, PasswordService passwordService,IUserProvider userProvider, UserService userService, ITwoFactorAuthService twoFactorAuthService)
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
                if (!ModelState.IsValid)
                {
                    _logger.LogError($"Bad request: {ModelState}");
                    return BadRequest(ModelState);
                }

                var response = await _mediator.Send(command);

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
            public String Token{get; set;}
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] Password pw)
        {            
            var userId = _userProvider.UserId;
            User user = await _context.users.FindAsync(userId);
            if (user == null)
                user = await _userService.GetUserByPasswordResetTokenAsync(pw.Token);            
            if (user == null)
                return BadRequest();                           
            try {                
                var salt = _passwordService.GenerateSalt();
                user.Password =  _passwordService.HashPassword(pw.NewPassword, salt);
                user.SaltKey = salt;
                await _userRepository.UpdateUserAsync(user);
                return Ok("Passwort erfolgreich geändert");
            } catch {
                return StatusCode(500, new { Message = "Fehler"});
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
                var command = new UploadFilesCommand(fileRS, fileVS, userId);
                var result = await _mediator.Send(command);

                return Ok(new { result.LinkRS, result.LinkVS });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ein Fehler ist beim Hochladen der Dateien aufgetreten. Fehler: {ex}");
            }
        }

        #endregion

        #region two-factor-authentication

        [HttpPost("2fa/setup")]
        public async Task<IActionResult> Setup2FA([FromBody] Setup2FARequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return NotFound("User not found");

                if (user.IsTwoFactorEnabled)
                    return BadRequest("2FA is already enabled for this user");

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

                if (user.IsTwoFactorEnabled)
                    return BadRequest("2FA is already enabled for this user");

                if (!_twoFactorAuthService.ValidateCode(request.Secret, request.Code))
                    return BadRequest("Ungültiger Verifizierungscode");

                // Save 2FA settings to user
                user.IsTwoFactorEnabled = true;
                user.TwoFactorSecret = request.Secret;
                
                // Generate and save backup codes
                var backupCodes = _twoFactorAuthService.GenerateBackupCodes();
                user.BackupCodes = JsonSerializer.Serialize(backupCodes);

                await _userRepository.UpdateUserAsync(user);

                return Ok(new { Message = "2FA enabled successfully", BackupCodes = backupCodes });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred during 2FA verification: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("2fa/verify")]
        public async Task<IActionResult> Verify2FA([FromBody] Verify2FARequest request)
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

                bool isValid = false;

                if (request.IsBackupCode)
                {
                    isValid = _twoFactorAuthService.ValidateBackupCode(user.BackupCodes, request.Code);
                    if (isValid)
                    {
                        // Remove used backup code
                        user.BackupCodes = _twoFactorAuthService.RemoveUsedBackupCode(user.BackupCodes, request.Code);
                        await _userRepository.UpdateUserAsync(user);
                    }
                }
                else
                {
                    isValid = _twoFactorAuthService.ValidateCode(user.TwoFactorSecret, request.Code);
                }

                if (!isValid)
                    return BadRequest("Invalid verification code");

                return Ok(new { Message = "2FA verification successful" });
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

                var user = await _userRepository.GetUserByEmailAsync(request.Email);
                if (user == null)
                    return BadRequest("Invalid credentials");

                // Verify password first
                if (!_passwordService.VerifyPassword(request.Password, user.Password, user.SaltKey))
                    return BadRequest("Invalid credentials");

                // Check if 2FA is enabled
                if (!user.IsTwoFactorEnabled)
                    return BadRequest("2FA is not enabled for this user");

                // Verify 2FA code
                bool isValid = false;
                if (request.IsBackupCode)
                {
                    isValid = _twoFactorAuthService.ValidateBackupCode(user.BackupCodes, request.TwoFactorCode);
                    if (isValid)
                    {
                        user.BackupCodes = _twoFactorAuthService.RemoveUsedBackupCode(user.BackupCodes, request.TwoFactorCode);
                        await _userRepository.UpdateUserAsync(user);
                    }
                }
                else
                {
                    isValid = _twoFactorAuthService.ValidateCode(user.TwoFactorSecret, request.TwoFactorCode);
                }

                if (!isValid)
                    return BadRequest("Invalid 2FA code");

                // Generate JWT token
                var token = await _tokenService.GenerateJwtToken(user.Email_Address, user.User_Id);

                return Ok(new LoginResponse
                {
                    UserId = user.User_Id,
                    Email = user.Email_Address,
                    AccessToken = token,
                    FirstName = user.FirstName
                });
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

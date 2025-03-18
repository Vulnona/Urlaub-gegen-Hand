using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Authentication;
using UGH.Contracts.Authentication;
using UGHApi.Applications.Authentication;
using UGH.Infrastructure.Services;
using UGH.Domain.Interfaces;

namespace UGHApi.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;

        public AuthController(ILogger<AuthController> logger, IMediator mediator, TokenService tokenService, IUserRepository userRepository, EmailService emailService)
        {
            _logger = logger;
            _mediator = mediator;
            _tokenService = tokenService;
            _emailService = emailService;
            _userRepository = userRepository;
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
                    return StatusCode(500, "Failed to send verification email.");

                return Ok("Verification email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
                return StatusCode(500, $"An error occurred while uploading files. Error: {ex}");
            }
        }

        #endregion
    }
}

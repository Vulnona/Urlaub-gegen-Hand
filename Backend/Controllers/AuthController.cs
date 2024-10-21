using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UGH.Contracts.Authentication;
using UGH.Application.Authentication;
using MediatR;
using UGHApi.Services.UserProvider;
using UGHApi.Applications.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace UGHApi.Controllers
{
    [Route("api/authenticate")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;

        public AuthController(
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IMediator mediator,
            IUserProvider userProvider
        )
        {
            _configuration = configuration;
            _logger = logger;
            _mediator = mediator;
            _userProvider = userProvider;
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
                    return BadRequest(ModelState);
                }

                var response = await _mediator.Send(command);

                if (response.IsFailure)
                {
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
                var result = await _mediator.Send(new VerifyEmailCommand(token));

                if (result.IsSuccess)
                {
                    return Ok(result);
                }

                if (result.Error.Code == "Error.InvalidToken")
                {
                    return NotFound(result.Error.Message);
                }
                return StatusCode(500, result.Error.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
        public async Task<IActionResult> ResendVerificationEmail(
            [FromBody] ResendEmailVerification resendUrl
        )
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var command = new ResendVerificationEmailCommand(resendUrl.Email);
                var result = await _mediator.Send(command);

                if (result.IsFailure)
                {
                    if (result.Error.Message.Contains("InvalidEmail"))
                        return BadRequest();

                    if (result.Error.Message.Contains("UserNotFound"))
                        return NotFound();

                    if (result.Error.Message.Contains("EmailSendFailed"))
                        return StatusCode(500, "Email sent failed");

                    return StatusCode(500, "Internal server error.");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("upload-id")]
        public async Task<IActionResult> UploadFile(IFormFile fileRS, IFormFile fileVS)
        {
            try
            {
                var userId = _userProvider.UserId;

                var command = new UploadFilesCommand(fileRS, fileVS, userId);
                var result = await _mediator.Send(command);

                return Ok(new { result.LinkRS, result.LinkVS });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading files.");
            }
        }

        #endregion
    }
}

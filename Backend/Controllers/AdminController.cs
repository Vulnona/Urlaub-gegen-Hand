using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using UGH.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Admin;
using UGH.Domain.Entities;
using UGH.Domain.Core;
using MediatR;

namespace UGHApi.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly Ugh_Context _context;
    private readonly UserService _userService;
    private readonly IMediator _mediator;
    private readonly AdminVerificationMailService _mailService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(
        Ugh_Context context,
        UserService userService,
        AdminVerificationMailService mailService,
        ILogger<AdminController> logger,
        IMediator mediator
    )
    {
        _context = context;
        _userService = userService;
        _mailService = mailService;
        _mediator = mediator;
        _logger = logger;
    }

    #region verifyuserstate
    [HttpPut("verify-user/{userId}")]
    public async Task<IActionResult> VerifyUser([Required] Guid userId)
    {
        var result = await _mediator.Send(new VerifyUserCommand(userId));

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("update-verification-state/{userId}/{verificationState}")]
    public async Task<IActionResult> UpdateVerifyState(
        [Required] Guid userId,
        [Required] UGH_Enums.VerificationState verificationState
    )
    {
        var command = new UpdateVerifyStateCommand(userId, verificationState);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return StatusCode(500, result);
    }

    [HttpGet("get-all-users")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersByAdmin()
    {
        var result = await _mediator.Send(new GetAllUsersByAdminQuery());

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return StatusCode(500, result);
    }

    [HttpGet("get-user-by-id/{userId}")]
    public async Task<IActionResult> GetUserById([Required] Guid userId)
    {
        try
        {
            var query = new GetUserByIdQuery(userId);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound("User not found.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [AllowAnonymous]
    [HttpGet("get-user-profile/{userId}")]
    public async Task<IActionResult> GetProfile([Required] Guid userId)
    {
        try
        {
            var profile = await _mediator.Send(new GetProfileQuery(userId));

            if (profile == null)
            {
                _logger.LogWarning($"Profile with UserId {userId} not found.");
                return NotFound($"Profile with UserId {userId} not found.");
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        }
    }

    #endregion
}

using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Admin;
using UGH.Domain.Core;
using UGHApi.Applications.Admin;
using UGHApi.Shared;
using UGHApi.ViewModels;

namespace UGHApi.Controllers;

[Route("api/admin")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdminController> _logger;

    public AdminController(ILogger<AdminController> logger, IMediator mediator)
    {
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
    public async Task<ActionResult<PaginatedList<UserDTO>>> GetAllUsersByAdmin(
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        var result = await _mediator.Send(new GetAllUsersByAdminQuery(pageNumber, pageSize));

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

    [HttpDelete("delete-admin-user/{userId}")]
    public async Task<IActionResult> DeleteUserByAdmin([Required] Guid userId)
    {
        try
        {
            var result = await _mediator.Send(new DeleteAdminUserCommand(userId));

            if (result.IsFailure)
            {
                _logger.LogWarning($"Profile with UserId {userId} not found.");
                return NotFound($"Profile with UserId {userId} not found.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        }
    }

    //[HttpPost("assign-membership")]
    //public async Task<IActionResult> AssignMembership(AssignMembershipRequest request)
    //{
    //    var command = new RedeemMembershipCommand(request.UserId, request.MembershipId);

    //    var result = await _mediator.Send(command);

    //    if (!result)
    //        return BadRequest("Failed to assign membership");
    //    return Ok("Membership assigned successfully.");
    //}

    #endregion
}

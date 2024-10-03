using Microsoft.AspNetCore.Authorization;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using UGH.Application.Profiles;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Profile;
using UGH.Domain.ViewModels;
using UGH.Contracts.Profile;
using MediatR;

namespace UGHApi.Controllers;

[Route("api/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly Ugh_Context _context;
    private readonly TokenService _tokenService;
    private readonly ILogger<ProfileController> _logger;
    private readonly IMediator _mediator;
    private readonly IUserProvider _userProvider;

    public ProfileController(
        Ugh_Context context,
        IMediator mediator,
        TokenService tokenService,
        ILogger<ProfileController> logger,
        IUserProvider userProvider
    )
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
        _mediator = mediator;
        _userProvider = userProvider;
    }

    #region user-profile
    [Authorize]
    [HttpGet("get-user-profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = _userProvider.UserId;
            var query = new GetUserProfileQuery(userId);
            var userProfile = await _mediator.Send(query);

            return Ok(new { Profile = userProfile });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPut("update-profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileData profile)
    {
        var userId = _userProvider.UserId;
        var command = new UpdateProfileCommand(userId, profile);
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return StatusCode(500, result.Error);
    }

    [Authorize]
    [HttpPut("update-profile-picture")]
    public async Task<IActionResult> UpdateProfilePicture(ProfilePictureUpdateRequest request)
    {
        try
        {
            var userId = _userProvider.UserId;
            var command = new UpdateProfilePictureCommand(userId, request.ProfilePicture);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok("Profile picture updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    #endregion
}

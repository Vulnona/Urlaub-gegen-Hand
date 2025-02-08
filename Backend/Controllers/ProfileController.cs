using Microsoft.AspNetCore.Authorization;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using UGH.Application.Profiles;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Profile;
using UGH.Domain.ViewModels;
using UGH.Contracts.Profile;
using MediatR;

using UGH.Domain.Entities;
using UGH.Domain.Core;

namespace UGHApi.Controllers;

[Route("api/profile")]
[ApiController]
[Authorize]
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
    // get another users profile.
    [HttpGet("get-user-profile/{userId}")]
    public async Task<IActionResult> GetProfile(Guid userId)
    {
        var callerId = _userProvider.UserId;
         // implement as generell user verification check helper function
         try {            
             User caller = await _context.users.FindAsync(callerId);
             if (caller.VerificationState != UGH_Enums.VerificationState.Verified){
                 _logger.LogWarning($"Unverified User:{_userProvider.UserId} attempts to access the profile of {userId}");
                 return StatusCode(403, "forbidden"); 
             }
         } catch {
             _logger.LogError($"Error checking the verification status of {_userProvider.UserId}");
             return StatusCode(500, new { Message = "Authentication Error"});
         }
         try {
            User user = await _context.users.FindAsync(userId);
            if (user == null) {
                throw new Exception("User not found.");
            }
            var profile = new VisibleProfile
            {
            FirstName = user.FirstName,
            LastName = user.LastName,
            ProfilePicture = user.ProfilePicture,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            City = user.City,
            Country = user.Country,
            State = user.State,
            FacebookLink = user.Facebook_link,
            AverageRating = user.AverageRating
        };
            if (user.Hobbies != null)
                profile.Hobbies = user.Hobbies.Split(',').Select(h => h.Trim()).ToList() ?? new List<string>();
            if (user.Skills != null)
                profile.Skills = user.Skills.Split(',').Select(h => h.Trim()).ToList() ?? new List<string>();
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using UGH.Application.Profiles;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Profile;
using UGH.Domain.ViewModels;
using UGH.Contracts.Profile;
using MediatR;
using UGH.Domain.Interfaces;

using UGH.Domain.Entities;
using UGH.Domain.Core;
using UGHApi.DATA;

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
    private readonly IUserRepository _userRepository;
    
    public ProfileController(
        Ugh_Context context,
        IMediator mediator,
        TokenService tokenService,
        ILogger<ProfileController> logger,
        IUserProvider userProvider,
        IUserRepository userRepository
    )
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
        _mediator = mediator;
        _userProvider = userProvider;
        _userRepository = userRepository;
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

            // Debug-Logging: User-Profil inkl. Address
            _logger.LogWarning($"[DEBUG] UserProfile-API-Response: {System.Text.Json.JsonSerializer.Serialize(userProfile)}");

            // Explizit serialisieren, damit 'address' klein geschrieben wird und null explizit sichtbar ist
            var response = new {
                profile = new {
                    userProfile.FirstName,
                    userProfile.LastName,
                    userProfile.ProfilePicture,
                    userProfile.DateOfBirth,
                    userProfile.Gender,
                    address = userProfile.Address, // explizit klein
                    userProfile.FacebookLink,
                    userProfile.Link_RS,
                    userProfile.Link_VS,
                    userProfile.VerificationState,
                    userProfile.UserRating,
                    userProfile.Hobbies,
                    userProfile.Skills,
                    userProfile.MembershipEndDate
                }
            };
            _logger.LogWarning($"[DEBUG] UserProfile-API-Response (custom): {System.Text.Json.JsonSerializer.Serialize(response)}");
            return Ok(response);
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
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfile profile)
    {
        var userId = _userProvider.UserId;
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
                return StatusCode(500, "User is Null");
            if (profile != null)
            {
                user.Skills = profile.Skills;
                user.Hobbies = profile.Hobbies;                
                await _userRepository.UpdateUserAsync(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, "Error updating User Data.");
        }
        return Ok("User Profile updated successfully");
    }

    [Authorize]
    [HttpPut("update-user-data")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserData profile)
    {
        var userId = _userProvider.UserId;
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user is null)
                return StatusCode(500, "User is Null");
            if (profile != null)
            {
                user.FirstName = profile.FirstName;
                user.LastName = profile.LastName;
                user.DateOfBirth = profile.DateOfBirth;
                user.Gender = profile.Gender;
                user.Hobbies = profile.Hobbies;
                user.Skills = profile.Skills;

                Address address = null;
                if (profile.Id.HasValue)
                {
                    // Use existing address if id is provided
                    address = await _context.addresses.FindAsync(profile.Id.Value);
                    if (address == null)
                    {
                        return BadRequest("Address with provided id not found.");
                    }
                    // Optionally update address fields if changed
                    address.Latitude = profile.Latitude ?? 0.0;
                    address.Longitude = profile.Longitude ?? 0.0;
                    address.DisplayName = profile.DisplayName;
                    _context.addresses.Update(address);
                }
                else
                {
                    // Create new address
                    address = new Address
                    {
                        Latitude = profile.Latitude ?? 0.0,
                        Longitude = profile.Longitude ?? 0.0,
                        DisplayName = profile.DisplayName
                    };
                    _context.addresses.Add(address);
                    await _context.SaveChangesAsync();
                }

                user.AddressId = address.Id;
                user.Address = address;

                await _userRepository.UpdateUserAsync(user);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, "Error updating User Data.");
        }
        return Ok("User Data updated successfully");
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
            User user = await _context.users.Include(u => u.UserMemberships).FirstOrDefaultAsync(u => u.User_Id == userId);
            if (user == null) {
                throw new Exception("User not found.");
            }
            
            // Use the same rating calculation logic as GetUserProfileQueryHandler - new visibility logic
            double userRating = 0;
            var allReviews = await _context.reviews.AsQueryable().Where(r => r.ReviewedId == user.User_Id).ToListAsync();
            
            // Filter to only include visible reviews using the new logic
            var visibleReviews = allReviews.Where(r => IsReviewVisible(r)).ToList();
            
            if (visibleReviews.Count() > 0)
                userRating = Math.Round(visibleReviews.Average(r => r.RatingValue), 1);
                
            var profile = new VisibleProfile
            {
                UserId = user.User_Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture = user.ProfilePicture,
                Age = (int.Parse(DateTime.Today.ToString("yyyyMMdd")) - int.Parse(user.DateOfBirth.ToString("yyyyMMdd"))) / 10000,
                Gender = user.Gender,
                FacebookLink = user.Facebook_link,
                AverageRating = userRating, // Use the same calculation
                MembershipEndDate = user.UserMemberships
                    .Where(m => m.IsMembershipActive)
                    .OrderBy(m => m.CreatedAt)
                    .FirstOrDefault()?.Expiration,
                Latitude = user.Address?.Latitude,
                Longitude = user.Address?.Longitude,
                DisplayName = user.Address?.DisplayName,
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

    private bool IsReviewVisible(UGH.Domain.Entities.Review review)
    {
        // Check if review is explicitly marked as visible
        if (review.IsVisible)
            return true;
        
        // Check if 14 days have passed since creation
        if (review.VisibilityDate.HasValue && DateTime.UtcNow >= review.VisibilityDate.Value)
            return true;
        
        return false;
    }
    #endregion
}


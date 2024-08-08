using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using UGHApi.Services;

namespace UGHApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly TokenService _tokenService;

        public ProfileController(UghContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet("profile/get-profile")]
        public async Task<IActionResult> GetProfile(string token)
        {
            try
            {
                var accessUserId = await _tokenService.GetUserIdFromToken(token);

                if (accessUserId == null)
                {
                    return Unauthorized("Invalid token");
                }
                var userId = accessUserId.Value;

                var profile = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (profile == null)
                {
                    var newProfile = new UserProfile
                    {
                        User_Id = userId,
                    };

                    _context.userprofiles.Add(newProfile);
                    await _context.SaveChangesAsync();

                    profile = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                }

                return Ok(new { Profile = profile });
            }
            catch (Exception ex)
            {
                // Log the exception 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("profile/update-profile")]
        public async Task<IActionResult> UpdateProfile(string token, [FromBody] UserProfile profile)
        {
            try
            {
                var accessUserId = await _tokenService.GetUserIdFromToken(token);

                if (accessUserId == null)
                {
                    return Unauthorized("Invalid token");
                }

                var userId = accessUserId.Value;

                if (profile == null || profile.User_Id != userId)
                {
                    return BadRequest("Profile object is null or User ID mismatch");
                }

                var existingProfile = await _context.userprofiles.Include(p => p.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (existingProfile == null)
                {
                    _context.userprofiles.Add(profile);
                    await _context.SaveChangesAsync();
                    return Ok(profile);
                }

                // Update profile fields
                existingProfile.Hobbies = profile.Hobbies;
                existingProfile.Options = profile.Options;
                if (profile.UserPic != null)
                {
                    existingProfile.UserPic = profile.UserPic;
                }

                // Update user fields if provided in the profile
                if (profile.User != null)
                {
                    existingProfile.User.FirstName = profile.User.FirstName;
                    existingProfile.User.LastName = profile.User.LastName;
                    existingProfile.User.DateOfBirth = profile.User.DateOfBirth;
                    existingProfile.User.Gender = profile.User.Gender;
                    existingProfile.User.Street = profile.User.Street;
                    existingProfile.User.HouseNumber = profile.User.HouseNumber;
                    existingProfile.User.PostCode = profile.User.PostCode;
                    existingProfile.User.City = profile.User.City;
                    existingProfile.User.State = profile.User.State;
                    existingProfile.User.Country = profile.User.Country;
                    existingProfile.User.Facebook_link = profile.User.Facebook_link;
                    existingProfile.User.Link_RS = profile.User.Link_RS;
                    existingProfile.User.Link_VS = profile.User.Link_VS;
                }

                // Update the database
                _context.userprofiles.Update(existingProfile);
                _context.users.Update(existingProfile.User);
                await _context.SaveChangesAsync();

                return Ok(existingProfile);
            }
            catch (Exception ex)
            {
                // Log the exception (use your preferred logging mechanism)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

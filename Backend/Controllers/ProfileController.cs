using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;
using UGHApi.Services;

namespace UGHApi.Controllers
{
    [Route("api/profile")]
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
        #region user-profile
        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetProfile([Required]string token)
        {
            try
            {
                var userAccessId = await _tokenService.GetUserIdFromToken(token);
                if (userAccessId == null)
                {
                    return Unauthorized();
                }
                var userId = userAccessId.Value;
                var userProfile = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (userProfile == null)
                {
                    var newUserProfile = new UserProfile
                    {
                        User_Id = userId,
                    };

                    _context.userprofiles.Add(newUserProfile);
                    await _context.SaveChangesAsync();
                    userProfile = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                }

                return Ok(new { Profile = userProfile });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the user profiles.");
            }
        }

        [HttpPut("add-or-update-profile")]
        public async Task<IActionResult> UpdateProfile([Required]string token, [FromBody] UserProfile profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var accessUserId = await _tokenService.GetUserIdFromToken(token);

                if (accessUserId == null)
                {
                    return Unauthorized();
                }

                var userId = accessUserId.Value;

                if (profile == null || profile.User_Id != userId)
                {
                    return BadRequest();
                }

                var existingProfile = await _context.userprofiles.Include(p => p.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (existingProfile == null)
                {
                    _context.userprofiles.Add(profile);
                    await _context.SaveChangesAsync();
                    return Ok(profile);
                }

                existingProfile.Hobbies = profile.Hobbies;
                existingProfile.Options = profile.Options;
                if (profile.UserPic != null)
                {
                    existingProfile.UserPic = profile.UserPic;
                }

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

                _context.userprofiles.Update(existingProfile);
                _context.users.Update(existingProfile.User);
                await _context.SaveChangesAsync();

                return Ok(existingProfile);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while updating the user profile.");
            }
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;
using UGHApi.Services;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly UserService _userService;
        private readonly AdminVerificationMailService _mailService;

        public AdminController(UghContext context, UserService userService, AdminVerificationMailService mailService)
        {
            _context = context;
            _userService = userService;
            _mailService = mailService;
        }

        #region verifyuserstate
        [HttpPut("verify-user/{userId}")]
        public async Task<IActionResult> VerifyUser([Required] int userId)
        {
            try
            {
                var user = await _context.users.FindAsync(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (user.VerificationState == UGH_Enums.VerificationState.Verified)
                {
                    return BadRequest("User verification already completed.");
                }

                user.VerificationState = UGH_Enums.VerificationState.Verified;
                await _context.SaveChangesAsync();

                return Ok("User verification completed successfully.");
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpPost("update-verification-state/{userId}/{verificationState}")]
        public async Task<IActionResult> UpdateVerifyState([Required]int userId,[Required] UGH_Enums.VerificationState verificationState)
        {
            try
            {
                var user = await _context.users.FindAsync(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                user.VerificationState = verificationState;
                await _context.SaveChangesAsync();

                if (verificationState == UGH_Enums.VerificationState.VerificationFailed ||
                    verificationState == UGH_Enums.VerificationState.Verified)
                {
                    _userService.DeleteUserInfo(userId);

                    if (verificationState == UGH_Enums.VerificationState.VerificationFailed)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(5));
                    }

                    var request = new ConfirmationReq
                    {
                        toEmail = user.Email_Address,
                        userName = $"{user.FirstName} {user.LastName}",
                        status = verificationState == UGH_Enums.VerificationState.VerificationFailed
                            ? "Verification Failed"
                            : "Verified"
                    };

                    await _mailService.SendConfirmationEmailAsync(request);
                }

                return Ok("Successfully updated verification state of user.");
            }
            catch (DbUpdateException )
            {   
                return StatusCode(500, "Database error occurred while updating verification state");
            }
            catch (Exception )
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersByAdmin()
        {
            try
            {
                var users = await _context.users.Include(u => u.CurrentMembership)
                                                .OrderBy(u => u.VerificationState)
                                                .ToListAsync();

                if (users == null || !users.Any())
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception) 
            { 
                return StatusCode(500, $"Internal server error"); 
            }
        }

        [HttpGet("get-user-by-id/{id}")]
        public async Task<ActionResult<User>> GetUserById([FromQuery][Required]int id)
        {
            try
            {
                var user = await _context.users.FindAsync(id);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                return user;
            }
            catch (Exception)
            {
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpGet("get-user-profile/{userId}")]
        public async Task<IActionResult> GetProfile([Required]int userId)
        {
            try
            {
                var checkProfile = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (checkProfile == null)
                {
                    var newProfile = new UserProfile
                    {
                        User_Id = userId,
                    };

                    _context.userprofiles.Add(newProfile);
                    _context.SaveChanges();
                }
                var profile = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (profile != null)
                {
                    // Generate a unique token for the profile
                    var token = Guid.NewGuid().ToString();
                    profile.Token = token;
                    _context.SaveChanges();

                    return Ok(new { Profile = profile, Token = token });
                }
                var user = await _context.users.FindAsync(userId);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound("Profile not found.");
            }
            catch (Exception) 
            {
                return StatusCode(500, $"Internal server error");
            }
        }
        #endregion
    }
}

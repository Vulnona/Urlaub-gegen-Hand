using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using UGHApi.Services;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly userservice _userservice;
        private readonly AdminVerificationMailService _mailService;

        public AdminController(UghContext context, userservice userservice, AdminVerificationMailService mailService)
        {
            _context = context;
            _userservice = userservice;
            _mailService = mailService;
        }

        #region verifyuserstate
        [HttpGet("admin/verify-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult VerifyUser(int userId)
        {
            try
            {
                var user = _context.users.Find(userId);

                if (user == null)
                {
                    return NotFound("User not found.");
                }

                if (user.VerificationState == UGH_Enums.VerificationState.verified)
                {
                    return BadRequest("User verification is already completed.");
                }

                user.VerificationState = UGH_Enums.VerificationState.verified;
                _context.SaveChanges();

                return Ok("User verification completed successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception (use your preferred logging mechanism)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("admin/update-verify-state/{userId}/{verificationState}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVerifyState(int userId, UGH_Enums.VerificationState verificationState)
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

                if (verificationState == UGH_Enums.VerificationState.verificationFailed)
                {
                    _userservice.DeleteUserInfo(userId);
                    await Task.Delay(TimeSpan.FromMinutes(5));

                    var request = new ConfirmationReq
                    {
                        toEmail = user.Email_Address,
                        userName = user.FirstName + " " + user.LastName,
                        status = "verification failed"
                    };
                    _mailService.SendConfirmationEmailAsync(request);
                }
                else if (verificationState == UGH_Enums.VerificationState.verified)
                {
                    _userservice.DeleteUserInfo(userId);

                    var request = new ConfirmationReq
                    {
                        toEmail = user.Email_Address,
                        userName = user.FirstName + " " + user.LastName,
                        status = "verified"
                    };
                    _mailService.SendConfirmationEmailAsync(request);
                }

                return Ok("Successfully updated verification state of user.");
            }
            catch (DbUpdateException ex)
            {
                // Log the exception 
                return StatusCode(500, $"Database error occurred while updating verification state: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("admin/get-all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllusersByAdmin()
        {
            try
            {
                var users = await _context.users.OrderBy(u => u.VerificationState).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                // Log the exception 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("admin/user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUserById(int id)
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
            catch (Exception ex)
            {
                // Log the exception 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("profile/get-profile-for-admin/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            try
            {
                var checkPro = await _context.userprofiles.Include(u => u.User).FirstOrDefaultAsync(p => p.User_Id == userId);
                if (checkPro == null)
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
            catch (Exception ex)
            {
                // Log the exception 
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion
    }
}

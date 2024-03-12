using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using UGHApi.Models;
using UGHApi.Services;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly UserService _userService; private readonly IConfiguration _configuration;

        public AdminController(UghContext context, UserService userService, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _userService = userService;
        }
        #region verifyUserState
        [HttpGet("verify-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult VerifyUser(int userId)
        {
            var user = _context.Users.Find(userId);
           
            if (user == null)
            {
                return NotFound(); // User not found
            }

            if (user.VerificationState == UGH_Enums.VerificationState.verified)
            {
                return BadRequest("User verification is already completed."); // User verification already completed
            }

            // Perform verification logic here, for example:
            user.VerificationState = UGH_Enums.VerificationState.verified;
            _context.SaveChanges();

            return Ok("User verification completed successfully.");
        }

        [HttpPost("updateverifystate/{userId}/{verificationState}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVerifyState(int userId, UGH_Enums.VerificationState verificationState)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(); // User not found
            }
            var State = verificationState;
            // Update verification state
            user.VerificationState = verificationState;

            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
                return Ok("Sucessfully Updated VerificationState of User"); // Success
            }
            catch (DbUpdateException)
            {
                // Handle database update exception
                return StatusCode(500, "Database error occurred while updating verification state.");
            }
        }
        [HttpGet("getallusers")]
        [Authorize(Roles = "Admin")] // Optional: Restrict access to admin role
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersByAdmin()
        {
            string baseUrl = _configuration["BaseUrl"];
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                user.SetImageUrl(user.IdCard, baseUrl);
            }
            return users;
        }
        #endregion
        
    }
}

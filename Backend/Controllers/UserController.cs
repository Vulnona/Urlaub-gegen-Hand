using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly IConfiguration _configuration;

        public UserController(UghContext context, IConfiguration configuration)
        {
            _context = context;
        _configuration = configuration;
        }

        // GET: api/User
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }

            string baseUrl = _configuration["BaseUrl"];
            var Users= await _context.Users.ToListAsync();
            foreach (var user in Users)
            {
                user.SetImageUrl(user.IdCard,baseUrl);
            }
            return Ok(Users);
        }

        // GET: api/User/5
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }

            string baseUrl = _configuration["BaseUrl"];
            var user = await _context.Users.FindAsync(id);
          
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                user.SetImageUrl(user.IdCard,baseUrl);
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("user/{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.User_Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("user")]
        public async Task<ActionResult<User>> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'UghContext.Users'  is null.");
          }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.User_Id }, user);
        }

        // DELETE: api/User/5
        [HttpDelete("user/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserPatchDTO userDTO)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            // Update user details based on provided DTO
            user.Password = userDTO.Password ?? user.Password;
            user.SaltKey = userDTO.SaltKey ?? user.SaltKey;
            user.FirstName = userDTO.FirstName ?? user.FirstName;
            user.LastName = userDTO.LastName ?? user.LastName;
            if (userDTO.DateOfBirth != null)
            {
                DateTime parsedDateOfBirth = DateTime.Parse(userDTO.DateOfBirth);
                DateOnly dateOnly = new DateOnly(parsedDateOfBirth.Year, parsedDateOfBirth.Month, parsedDateOfBirth.Day);
                user.DateOfBirth = dateOnly;
            }
           
            user.Gender = userDTO.Gender ?? user.Gender;
            user.Street = userDTO.Street ?? user.Street;
            user.HouseNumber = userDTO.HouseNumber ?? user.HouseNumber;
            user.PostCode = userDTO.PostCode ?? user.PostCode;
            user.City = userDTO.City ?? user.City;
            user.Country = userDTO.Country ?? user.Country;
            user.Email_Adress = userDTO.Email_Adress ?? user.Email_Adress;
            user.IsEmailVerified = userDTO.IsEmailVerified ?? user.IsEmailVerified;
            user.FacebookUrl = userDTO.FacebookUrl ?? user.FacebookUrl;
            user.CouponCode = userDTO.CouponCode ?? user.CouponCode;
            user.IdCard = userDTO.IdCard ?? user.IdCard;
            user.IdDocumentUrl = userDTO.IdDocumentUrl ?? user.IdDocumentUrl;
            user.VerificationState = userDTO.VerificationState ?? user.VerificationState;

            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
                return Ok("Successfully updated user details"); // Success
            }
            catch (DbUpdateException)
            {
                // Handle database update exception
                return StatusCode(500, "Database error occurred while updating user details.");
            }
        }


        #region verifyUserState
        [HttpPatch("verify-user/{userId}")]
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

        [HttpPatch("update-verify-state/{userId}/{verification-state}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVerifyState(int userId, UGH_Enums.VerificationState verificationState)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            // Update verification state
            user.VerificationState = verificationState;

            try
            {
                await _context.SaveChangesAsync(); // Save changes to the database
                return Ok("Successfully Updated VerificationState of User"); // Success
            }
            catch (DbUpdateException)
            {
                // Handle database update exception
                return StatusCode(500, "Database error occurred while updating verification state.");
            }
        }

        #endregion

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.User_Id == id)).GetValueOrDefault();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(UghContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region users-info
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _context.users.ToListAsync();
                if (!users.Any()) NotFound();
                return Ok(users);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-user-by-id/{id}")]
        public async Task<ActionResult<User>> GetUser([Required]int id)
        {
            try
            {
                var user = await _context.users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("upload-id")]
        public async Task<IActionResult> UploadID([FromBody] UploadIDViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existUser = await _context.users.FindAsync(model.Id);
                if (existUser == null)
                {
                    return NotFound();
                }

                existUser.Link_VS = model.Link_VS;
                existUser.Link_RS = model.Link_RS;
                await _context.SaveChangesAsync();
                _logger.LogInformation("ID Uploaded Successfully");
                return Ok("ID Uploaded Successfully");
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser([Required]int id)
        {
            try
            {
                var user = await _context.users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<bool> UserExistsAsync([Required] int id)
        {
            if (_context == null || _context.users == null)
            {
                return false;
            }
            return await _context.users.AnyAsync(e => e.User_Id == id);
        }
        #endregion
    }
}
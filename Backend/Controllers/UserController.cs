using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHModels;

namespace UGHApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UghContext _context;

        public UserController(UghContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet("get-all-users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            if(!ModelState.IsValid)
            {
            return BadRequest(ModelState); 
            }
            if (_context.users == null)
            {
                return NotFound();
            }
            try
            {
                return await _context.users.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/User/5
        [HttpGet("get-user/{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.users == null)
            {
                return NotFound();
            }
            try
            {
                var user = await _context.users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return user;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("upload-id/{id}")]
        public async Task<IActionResult> UploadID(int id, string link_vs, string link_rs)
        {

            if (string.IsNullOrEmpty(link_vs) || string.IsNullOrEmpty(link_rs))
            {
                return BadRequest("Link_VS or Link_RS is null or empty.");
            }
            try
            {
                var existUser = await _context.users.FindAsync(id);
                if (existUser == null)
                {
                    return NotFound("User not found.");
                }
                existUser.Link_VS = link_vs;
                existUser.Link_RS = link_rs;

                await _context.SaveChangesAsync();
                return Ok("Links updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the changes: {ex.Message}");
            }
        }

        // DELETE: api/User/5
        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.users == null)
            {
                return NotFound();
            }
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool UserExists(int id)
        {
            return (_context.users?.Any(e => e.User_Id == id)).GetValueOrDefault();
        }
    }
}

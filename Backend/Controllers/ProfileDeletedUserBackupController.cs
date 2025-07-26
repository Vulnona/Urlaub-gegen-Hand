using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.DATA;
using Backend.Models;

namespace UGHApi.Controllers
{
    [ApiController]
    [Route("api/profile/delete-user-and-backup")]
    public class ProfileDeletedUserBackupController : ControllerBase
    {
        private readonly Ugh_Context _context;
        public ProfileDeletedUserBackupController(Ugh_Context context)
        {
            _context = context;
        }

        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> DeleteUserAndBackup()
        {
            // Token aus Header extrahieren und validieren wie beim Login
            var authHeader = Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var tokenService = HttpContext.RequestServices.GetService(typeof(UGH.Infrastructure.Services.TokenService)) as UGH.Infrastructure.Services.TokenService;
            Guid? userId = null;
            try
            {
                userId = await tokenService.GetUserIdFromToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Token validation failed: {ex.Message}");
                return Unauthorized();
            }
            Console.WriteLine($"[DEBUG] Extracted userId from JWT claims: {userId}");
            if (userId == null)
                return Unauthorized();

            var user = await _context.users.Include(u => u.UserProfile).FirstOrDefaultAsync(u => u.User_Id == userId);
            if (user == null)
                return NotFound();

            // Backup-Objekt erstellen
            var backup = new DeletedUserBackup
            {
                UserId = user.User_Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                Email = user.Email_Address,
                Skills = user.UserProfile?.Skills,
                Hobbies = user.UserProfile?.Hobbies,
                ProfilePicture = user.UserProfile?.UserPic != null ? Convert.ToBase64String(user.UserProfile.UserPic) : null,
                DeletedAt = DateTime.UtcNow
            };
            _context.DeletedUserBackups.Add(backup);
            //User löschen
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        // Admin-Endpunkt: Backups abrufen
        [HttpGet("deleted-user-backups")]
        public async Task<IActionResult> GetDeletedUserBackups()
        {
            // Admin  Check: Nur Admins dürfen Backups abrufen...
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin)
                return Forbid();

            var backups = await _context.DeletedUserBackups.OrderByDescending(b => b.DeletedAt).ToListAsync();
            return Ok(backups);
        }
    }
}

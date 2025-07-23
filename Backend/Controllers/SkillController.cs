using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UGHApi.DATA;

namespace UGHApi.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly Ugh_Context _context;
        private readonly ILogger<SkillController> _logger;

        public SkillController(Ugh_Context context, ILogger<SkillController> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region skills

        [HttpGet("get-all-skills")]
        public async Task<ActionResult> GetAllSkills()
        {
            try
            {
                var allSkills = await _context.skills.ToListAsync();
                if (!allSkills.Any())
                    return NotFound();
                return Ok(allSkills);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}

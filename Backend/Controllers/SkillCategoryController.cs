using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/skill-category")]
    [ApiController]
    public class SkillCategoryController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly ILogger<SkillCategoryController> _logger;
        public SkillCategoryController(UghContext context, ILogger<SkillCategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region parent-skills

        [HttpGet("get-all-skills")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            try
            {
                var skillCategories = await _context.skills.Where(b => b.ParentSkill_ID == null).ToListAsync();
                if(!skillCategories.Any()) return NotFound();
                return Ok(skillCategories);
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
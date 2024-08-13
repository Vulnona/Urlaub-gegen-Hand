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

        public SkillCategoryController(UghContext context)
        {
            _context = context;
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the parent skills.");
            }
        }
        #endregion
    }
}
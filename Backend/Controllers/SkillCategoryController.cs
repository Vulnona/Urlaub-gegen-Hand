using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class SkillCategoryController : ControllerBase
    {
        private readonly UghContext _context;

        public SkillCategoryController(UghContext context)
        {
            _context = context;
        }

        // GET: api/SkillCategory
        [HttpGet("skill-category")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.skills == null)
          {
              return NoContent();
          }
            var skillCategories = _context.skills.Where ( b=> b.ParentSkill_ID == null);
            return await skillCategories.ToListAsync();
        }
    }
}

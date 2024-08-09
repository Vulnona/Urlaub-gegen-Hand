using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly UghContext _context;

        public SkillController(UghContext context)
        {
            _context = context;
        }

        // GET: api/Skill/5
        [HttpGet("skill/{category_id}")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills(int Category_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.skills == null)
            {
                return NotFound();
            }

            try
            {
                var skills = _context.skills.Where(b => b.ParentSkill_ID == Category_id);

                if (!skills.Any())
                {
                    return NotFound();
                }

                return await skills.ToListAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-all-skills")]
        public async Task<ActionResult> GetAllSkills()
        {
            try
            {
                var allSkills = await _context.skills.ToListAsync();
                return Ok(allSkills);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

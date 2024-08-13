using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/skills")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly UghContext _context;

        public SkillController(UghContext context)
        {
            _context = context;
        }
        #region skills
        [HttpGet("get-skill-by-category-id/{category_id}")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills([FromQuery][Required]int Category_id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                var skills = _context.skills.Where(b => b.ParentSkill_ID == Category_id);

                if (!skills.Any())
                {
                    return NotFound("No skill found.");
                }

                return await skills.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the skill");
            }
        }

        [HttpGet("get-all-skills")]
        public async Task<ActionResult> GetAllSkills()
        {
            try
            {
                var allSkills = await _context.skills.ToListAsync();
                if (!allSkills.Any()) return NotFound("Skills not found."); 
                return Ok(allSkills);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the skills.");
            }
        }
        #endregion
    }
}

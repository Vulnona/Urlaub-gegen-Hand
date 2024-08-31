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
        private readonly ILogger<SkillController> _logger;

        public SkillController(UghContext context, ILogger<SkillController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region skills
        [HttpGet("get-skill-by-category-id/{category_id}")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills([Required]int Category_id)
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
                    return NotFound();
                }

                return await skills.ToListAsync();
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-all-skills")]
        public async Task<ActionResult> GetAllSkills()
        {
            try
            {
                var allSkills = await _context.skills.ToListAsync();
                if (!allSkills.Any()) return NotFound(); 
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

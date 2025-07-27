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

        [HttpGet("hierarchical")]
        public async Task<ActionResult> GetHierarchicalSkills()
        {
            var allSkills = await _context.skills.ToListAsync();
            // Explizite Encoding-Korrektur für falsch dekodierte Umlaute
            foreach (var skill in allSkills)
            {
                if (skill.SkillDescrition != null)
                {
                    var bytes = System.Text.Encoding.Default.GetBytes(skill.SkillDescrition);
                    skill.SkillDescrition = System.Text.Encoding.UTF8.GetString(bytes);
                }
            }
            var parentSkills = allSkills.Where(s => s.ParentSkill_ID == null).ToList();
            var result = parentSkills.Select(parent => new {
                id = parent.Skill_ID,
                name = parent.SkillDescrition,
                children = allSkills.Where(s => s.ParentSkill_ID == parent.Skill_ID)
                    .Select(child => new {
                        id = child.Skill_ID,
                        name = child.SkillDescrition
                    }).ToList()
            }).ToList();
            return Ok(result);
        }
        #endregion
    }
}

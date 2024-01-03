using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillCategoryController : ControllerBase
    {
        private readonly UghContext _context;

        public SkillCategoryController(UghContext context)
        {
            _context = context;
        }

        // GET: api/SkillCategory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills()
        {
          if (_context.Skills == null)
          {
              return NotFound();
          }
            var skillCategories = _context.Skills.Where ( b=> b.ParentSkill_ID == null);
            return await skillCategories.ToListAsync();
        }
    }
}

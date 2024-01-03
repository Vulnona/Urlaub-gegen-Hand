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
    public class SkillController : ControllerBase
    {
        private readonly UghContext _context;

        public SkillController(UghContext context)
        {
            _context = context;
        }

       

        // GET: api/Skill/5
        [HttpGet("{Category_id}")]
        public async Task<ActionResult<IEnumerable<Skill>>> GetSkills(int Category_id)
        {
          if (_context.Skills == null)
          {
              return NotFound();
          }
            var skills =  _context.Skills.Where( b=> b.ParentSkill_ID==Category_id);

            if (skills.Count() == 0)
            {
                return NotFound();
            }

            return await skills.ToListAsync();
        }


    }
}

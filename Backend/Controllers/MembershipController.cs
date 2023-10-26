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
    public class MembershipController : ControllerBase
    {
        private readonly UghContext _context;

        public MembershipController(UghContext context)
        {
            _context = context;
        }

        // GET: api/Membership
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Membership>>> GetMemberships()
        {
          if (_context.Memberships == null)
          {
              return NotFound();
          }
            return await _context.Memberships.ToListAsync();
        }

        // GET: api/Membership/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Membership>> GetMembership(int id)
        {
          if (_context.Memberships == null)
          {
              return NotFound();
          }
            var membership = await _context.Memberships.FindAsync(id);

            if (membership == null)
            {
                return NotFound();
            }

            return membership;
        }

        // PUT: api/Membership/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMembership(int id, Membership membership)
        {
            if (id != membership.MembershipID)
            {
                return BadRequest();
            }

            _context.Entry(membership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MembershipExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Membership
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Membership>> PostMembership(Membership membership)
        {
          if (_context.Memberships == null)
          {
              return Problem("Entity set 'UghContext.Memberships'  is null.");
          }
            _context.Memberships.Add(membership);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMembership", new { id = membership.MembershipID }, membership);
        }

        // DELETE: api/Membership/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            if (_context.Memberships == null)
            {
                return NotFound();
            }
            var membership = await _context.Memberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            _context.Memberships.Remove(membership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MembershipExists(int id)
        {
            return (_context.Memberships?.Any(e => e.MembershipID == id)).GetValueOrDefault();
        }
    }
}

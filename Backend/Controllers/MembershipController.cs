using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly UghContext _context;

        public MembershipController(UghContext context)
        {
            _context = context;
        }

        // GET: api/Membership
        [HttpGet("membership/get-membership")]
        public async Task<ActionResult<IEnumerable<Membership>>> Getmemberships()
        {
          if (_context.memberships == null)
          {
              return NotFound();
          }
            return await _context.memberships.ToListAsync();
        }

        // GET: api/Membership/5
        [HttpGet("membership/{id}")]
        public async Task<ActionResult<Membership>> GetMembership(int id)
        {
          if (_context.memberships == null)
          {
              return NotFound();
          }
            var membership = await _context.memberships.FindAsync(id);

            if (membership == null)
            {
                return NotFound();
            }

            return membership;
        }

        // PUT: api/Membership/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("membership/update-membership/{id}")]
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
        [HttpPost("membership/add-new-membership")]
        public async Task<ActionResult<Membership>> PostMembership(Membership membership)
        {
          if (_context.memberships == null)
          {
              return Problem("Entity set 'UghContext.memberships'  is null.");
          }
            _context.memberships.Add(membership);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMembership", new { id = membership.MembershipID }, membership);
        }

        // DELETE: api/Membership/5
        [HttpDelete("membership/delete-membership/{id}")]
        public async Task<IActionResult> DeleteMembership(int id)
        {
            if (_context.memberships == null)
            {
                return NotFound();
            }
            var membership = await _context.memberships.FindAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            _context.memberships.Remove(membership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MembershipExists(int id)
        {
            return (_context.memberships?.Any(e => e.MembershipID == id)).GetValueOrDefault();
        }
    }
}

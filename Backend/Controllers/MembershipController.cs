using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using UGHApi.Models;
using UGHModels;

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
        [HttpGet("membership/validate-subscriptionId")]
        public async Task<IActionResult> ValidateSubscriptionId([FromQuery]int sid)
        {
            try
            {
                var storeId = "106078763";
                var productId = sid; 
                var token = "secret_MdRTZzpz65Cpp9KHxj3pvKvyPP1k3z3L"; 

                var options = new RestClientOptions($"https://app.ecwid.com/api/v3/{storeId}/products/{productId}");
                var client = new RestClient(options);
                var request = new RestRequest("");

                request.AddHeader("accept", "application/json");
                request.AddHeader("Authorization", $"Bearer {token}");

                var response = await client.GetAsync(request);

                if (response.IsSuccessful)
                {
                    var subscription = JsonConvert.DeserializeObject<SubscriptionResponse>(response.Content);
                    var result = new
                    {
                        subscription.Id,
                        subscription.Name,
                        
                    };
                    return Ok(response.Content);
                }
                else
                {
                    
                    var errorDetails = new
                    {
                        StatusCode = response.StatusCode,
                        Content = response.Content
                    };

                    return StatusCode((int)response.StatusCode, errorDetails);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing your request.", message = ex.Message });
            }
        }
        public class SubscriptionResponse
        {
            public string Id { get; set; }
            public string Name { get; set; }
            // Add other properties as needed
        }
        [HttpPost("membership/check-active-membership/{subId}")] //This api is created to check the active membership of the user by subscripitonId as input
        public async Task<IActionResult> CheckActiveMembership(int subId)
        {
            var membership = await _context.memberships.FindAsync(subId);

            if (membership == null)
            {
                return NotFound("Membership not found");
            }

            bool isActive = membership.Expiration > DateTime.UtcNow;

            return Ok(new { IsActive = isActive });
        }
        
        [HttpGet("membership/check-active-membership-byuserId/{userId}")]    //This api is created to check the active  membership of the user by userId as input
        public async Task<IActionResult> CheckActiveMembershipByUserId(int userId)
        {
            // Find the user
            var user = await _context.users
                .Include(u => u.CurrentMembership)
                .FirstOrDefaultAsync(u => u.User_Id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.CurrentMembership == null)
            {
                return NotFound("User does not have an active membership");
            }

            // Get the membership
            var membership = await _context.memberships.FindAsync(user.CurrentMembership.MembershipID);

            if (membership == null)
            {
                return NotFound("Membership not found");
            }

            bool isActive = membership.Expiration > DateTime.UtcNow;

            return Ok(new { IsActive = isActive });
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

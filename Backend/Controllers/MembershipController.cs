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
    [Route("api/membership")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly UghContext _context;

        public MembershipController(UghContext context)
        {
            _context = context;
        }
        #region user-membership-using-ecwid
        [HttpGet("validate-subscriptionId")]
        public async Task<IActionResult> ValidateSubscriptionId([FromQuery]int sid)
        {
            try
            {
                var storeId = "";
                var productId = sid; 
                var token = ""; 
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
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching the valid subscription id.");
            }
        }
        public class SubscriptionResponse
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        [HttpPost("check-active-membership/{subId}")] //API to check the active membership of the user by subscripitonId
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
        
        [HttpGet("check-active-membership-by-userId/{userId}")]    //API to check the active  membership of the user by userId
        public async Task<IActionResult> CheckActiveMembershipByUserId([FromQuery]int userId)
        {
            var user = await _context.users.Include(u => u.CurrentMembership).FirstOrDefaultAsync(u => u.User_Id == userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.CurrentMembership == null)
            {
                return NotFound("User does not have an active membership");
            }
            var membership = await _context.memberships.FindAsync(user.CurrentMembership.MembershipID);

            if (membership == null)
            {
                return NotFound("Membership not found");
            }

            bool isActive = membership.Expiration > DateTime.UtcNow;

            return Ok(new { IsActive = isActive });
        }

        private bool MembershipExists(int id)
        {
            return (_context.memberships?.Any(e => e.MembershipID == id)).GetValueOrDefault();
        }
        #endregion
    }
}

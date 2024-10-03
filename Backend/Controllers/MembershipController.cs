using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;
using UGHApi.Applications.Memberships;

namespace UGHApi.Controllers;

[Route("api/membership")]
[ApiController]
public class MembershipController : ControllerBase
{
    private readonly Ugh_Context _context;
    private readonly ILogger<MembershipController> _logger;
    private readonly IMediator _mediator;

    public MembershipController(
        Ugh_Context context,
        IMediator mediator,
        ILogger<MembershipController> logger
    )
    {
        _context = context;
        _mediator = mediator;
        _logger = logger;
    }

    #region user-membership-using-ecwid
    [HttpGet("validate-subscriptionId")]
    public async Task<IActionResult> ValidateSubscriptionId([FromQuery] int sid)
    {
        try
        {
            var storeId = "";
            var productId = sid;
            var token = "";
            var options = new RestClientOptions(
                $"https://app.ecwid.com/api/v3/{storeId}/products/{productId}"
            );
            var client = new RestClient(options);
            var request = new RestRequest("");

            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", $"Bearer {token}");

            var response = await client.GetAsync(request);

            if (response.IsSuccessful)
            {
                var subscription = JsonConvert.DeserializeObject<SubscriptionResponse>(
                    response.Content
                );
                var result = new { subscription.Id, subscription.Name, };
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
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpGet("get-all-memberships")]
    public async Task<IActionResult> GetAllMemberShips()
    {
        try
        {
            var query = new GetAllMembershipsQuery();
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    public class SubscriptionResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    [Authorize]
    [HttpPost("check-active-membership/{subId}")] //API to check the active membership of the user by subscripitonId
    public async Task<IActionResult> CheckActiveMembership(int subId)
    {
        var membership = await _context.memberships.FindAsync(subId);

        if (membership == null)
        {
            return NotFound("Membership not found");
        }
        bool isActive = membership.IsActive;

        return Ok(new { IsActive = isActive });
    }

    [Authorize]
    [HttpGet("check-active-membership-by-userId/{userId}")]
    public async Task<IActionResult> CheckActiveMembershipByUserId(Guid userId)
    {
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
        var membership = await _context.memberships.FindAsync(user.CurrentMembership.MembershipID);

        if (membership == null)
        {
            return NotFound("Membership not found");
        }

        bool isActive = membership.IsActive || user.CurrentMembership.IsActive;

        return Ok(new { IsActive = isActive });
    }

    private bool MembershipExists(int id)
    {
        return (_context.memberships?.Any(e => e.MembershipID == id)).GetValueOrDefault();
    }
    #endregion
}

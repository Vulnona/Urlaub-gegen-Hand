using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Offers;
using UGH.Domain.ViewModels;
using MediatR;

namespace UGHApi.Controllers;

[Route("api/offer")]
[ApiController]
public class OfferController : ControllerBase
{
    private readonly Ugh_Context _context;
    private readonly ILogger<OfferController> _logger;
    private readonly EmailService _emailService;
    private readonly IUserProvider _userProvider;
    private readonly IMediator _mediator;

    public OfferController(
        Ugh_Context context,
        ILogger<OfferController> logger,
        IMediator mediator,
        EmailService emailService,
        IUserProvider userProvider
    )
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
        _emailService = emailService;
        _userProvider = userProvider;
    }

    #region offers
    [HttpGet("get-all-offers")]
    public async Task<IActionResult> GetOffersAsync(
        string searchTerm,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        try
        {
            var userId = _userProvider.UserId;
            var query = new GetOffersQuery(searchTerm, userId, pageNumber, pageSize);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(new { Code = result.Error.Code, Message = result.Error.Message });
            }

            return Ok(
                new
                {
                    result.Value.Items,
                    result.Value.TotalCount,
                    result.Value.PageNumber,
                    result.Value.PageSize,
                    result.Value.TotalPages
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("get-offer-by-user")]
    public async Task<IActionResult> GetOfferAsync()
    {
        try
        {
            var userId = _userProvider.UserId;
            var query = new GetOfferByUserQuery(userId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(new { Code = result.Error.Code, Message = result.Error.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("get-offer-by-id/{OfferId:int}")]
    public async Task<IActionResult> GetOffer([Required] int OfferId)
    {
        try
        {
            var query = new GetOfferByIdQuery(OfferId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(new { Code = result.Error.Code, Message = result.Error.Message });
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPost("add-new-offer")]
    public async Task<IActionResult> AddOffer([FromForm] OfferViewModel offerViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new AddOfferCommand(offerViewModel);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(new { Message = result.Error.Message });
            }

            return Ok("New Offer Added Successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpDelete("delete-offer/{OfferId:int}")]
    public async Task<IActionResult> DeleteOfferAsync([Required] int OfferId)
    {
        try
        {
            var command = new DeleteOfferCommand(OfferId);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return NotFound(result.Error.Message);
            }

            return Ok($"Offer with ID {OfferId} deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPost("apply-offer")]
    public async Task<IActionResult> ApplyForOffer([Required] int offerId)
    {
        try
        {
            var userId = _userProvider.UserId;
            var command = new ApplyForOfferCommand(offerId, userId);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok("Application submitted successfully, and notification sent to the host.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [Authorize]
    [HttpPut("update-application-status")]
    public async Task<IActionResult> UpdateApplicationStatus(
        [Required] int offerId,
        [Required] Guid userId,
        [Required] bool isApprove
    )
    {
        var hostId = _userProvider.UserId;
        var command = new UpdateApplicationStatusCommand(offerId, userId, hostId, isApprove);
        var result = await _mediator.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Message);
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("offer-applications")]
    public async Task<IActionResult> GetOfferApplicationsByHost()
    {
        try
        {
            var hostId = _userProvider.UserId;
            var query = new GetOfferApplicationsByHostQuery(hostId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error.Message);
            }

            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                $"Exception occurred while fetching offer applications: {ex.Message} | StackTrace: {ex.StackTrace}"
            );
            return StatusCode(
                500,
                "Internal server error occurred while fetching offer applications."
            );
        }
    }
    #endregion
}

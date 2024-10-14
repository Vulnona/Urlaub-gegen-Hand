using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using UGHApi.Services.UserProvider;
using UGHApi.Applications.Reviews;
using UGH.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Reviews;
using UGH.Contracts.Review;
using MediatR;

namespace UGHApi.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IMediator _mediator;
    private readonly IUserProvider _userProvider;

    public ReviewController(
        IReviewService reviewService,
        IMediator mediator,
        IUserProvider userProvider
    )
    {
        _reviewService = reviewService;
        _mediator = mediator;
        _userProvider = userProvider;
    }

    [Authorize]
    [HttpGet("get-all-reviews")]
    public async Task<IActionResult> GetAllReviews()
    {
        try
        {
            var query = new GetAllReviewsQuery();
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return StatusCode(500, result.Error.Message);
            }

            return Ok(result.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while fetching all reviews.");
        }
    }

    [Authorize]
    [HttpPost("add-review")]
    public async Task<IActionResult> AddReview([FromBody] CreateReviewRequest review)
    {
        if (review == null || !ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid input parameters or model state." });
        }

        var userId = _userProvider.UserId;

        var command = new AddReviewCommand(
            review.OfferId,
            review.RatingValue,
            review.ReviewComment,
            userId,
            review.ReviewedUserId
        );
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok(new { message = "Review added successfully." });
        }
        else
        {
            return BadRequest(new { message = result.Error.Message.ToString() });
        }
    }

    [Authorize]
    [HttpPut("update-review")]
    public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewRequest updateReviewData)
    {
        try
        {
            var userId = _userProvider.UserId;
            var command = new UpdateReviewCommand(updateReviewData, userId);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        catch (Exception )
        {
            return StatusCode(500, "Internal server error occurred while updating the review.");
        }
    }

    [Authorize]
    [HttpDelete("delete-review")]
    public async Task<IActionResult> DeleteReview([Required] int reviewId, [Required] string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email is required.");
        }

        try
        {
            var command = new DeleteReviewCommand(reviewId, email);
            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(result);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while deleting the review.");
        }
    }

    [Authorize]
    [HttpGet("get-user-reviews")]
    public async Task<IActionResult> GetAllReviewsByUserId(Guid userId)
    {
        try
        {
            var query = new GetAllReviewsByUserIdQuery(userId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while fetching user reviews.");
        }
    }

    [Authorize]
    [HttpGet("get-offer-reviews")]
    public async Task<IActionResult> GetAllReviewsByOfferId(int offerId)
    {
        try
        {
            var query = new GetAllReviewsByOfferIdQuery(offerId);
            var result = await _mediator.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while fetching user reviews.");
        }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using UGHApi.Services.UserProvider;
using UGH.Infrastructure.Services;
using UGH.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using UGH.Application.Reviews;
using UGH.Domain.Interfaces;

namespace UGHApi.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IUserProvider _userProvider;
    private readonly ReviewRepository _reviewRepository;
    private readonly IOfferRepository _offerRepository;
    
    public ReviewController(
        IUserProvider userProvider,
        ReviewRepository reviewRepository,
        IOfferRepository offerRepository
    )
    {
        _userProvider = userProvider;
        _reviewRepository = reviewRepository;
        _offerRepository = offerRepository;
    }

public class CreateReviewRequest
{
#pragma warning disable CS8632
    [Required(ErrorMessage = "OfferId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "OfferId cannot be zero or negative.")]
    public int offerId { get; set; }

    [Required(ErrorMessage = "RatingValue is required.")]
    [Range(1, 5, ErrorMessage = "RatingValue must be between 1 and 5.")]
    public int ratingValue { get; set; }

    public string? reviewComment { get; set; }
    public Guid? reviewedUserId { get; set; } = null;

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
        String result = await _reviewRepository.AddReview(review.offerId, review.ratingValue, review.reviewComment, userId, review.reviewedUserId);
        
        if (result == "Review added successfully.")
            return Ok(new { message = "Review added successfully." });
        else
            return BadRequest(new { message = result });
    }

    [Authorize]
    [HttpGet("get-user-reviews")]
    public async Task<IActionResult> GetAllReviewsByUserId(Guid userId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var reviews = await _reviewRepository.GetAllReviewsByUserIdAsync(userId, pageNumber, pageSize);
            if (reviews == null)
            {
                return NotFound();
            }

            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while fetching user reviews.");
        }
    }

    [Authorize]
    [HttpGet("get-offer-reviews")]
    public async Task<IActionResult> GetAllReviewsByOfferId(int offerId, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var reviews = await _reviewRepository.GetReviewsByOfferIdAsync(offerId, pageNumber, pageSize);
            
            if (reviews == null)
            {
                return NotFound();
            }

            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while fetching offer reviews.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("get-all-offers-for-reviews-admin")]
    public async Task<IActionResult> GetAllOffersForReviewsForAdmin(string searchTerm, int pageNumber = 1,
        int pageSize = 10)
    {
        try
        {
            var reviews = await _offerRepository.GetAllOffersForReviewsAsync(searchTerm, pageNumber, pageSize);
            if (reviews == null)
                return NotFound();
            return Ok(reviews);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error occurred while fetching user reviews.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;
namespace UGHApi.Controllers
{
    [Route("api/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly ILogger<ReviewController> _logger;
        public ReviewController(UghContext context, ILogger<ReviewController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region review
        [HttpPost("add-review")]
        public async Task<IActionResult> AddReview(Review review,[Required] string email)
        {
            if (!ModelState.IsValid || review == null || string.IsNullOrEmpty(email))
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _context.users.FirstOrDefaultAsync(u => u.Email_Address == email);
                if (user == null)
                {
                    return NotFound("User with the provided email does not exist.");
                }

                var existingReview = await _context.reviews
                    .Include(r => r.User)
                    .FirstOrDefaultAsync(r => r.User.Email_Address == email && r.OfferId == review.OfferId);

                if (existingReview != null)
                {
                    return Conflict("A review by this user for this offer already exists");
                }

                review.UserId = user.User_Id;
                await _context.reviews.AddAsync(review);
                await _context.SaveChangesAsync();

                return Ok("Review added successfully.");
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("update-review")]
        public async Task<IActionResult> UpdateReviewStatus([Required]int reviewId, reviewStatus newStatus)
        {
            try
            {
                if(!ModelState.IsValid ) return BadRequest(ModelState);
                var review = await _context.reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
                if (review == null)
                {
                    return NotFound();
                }
                review.Status = newStatus;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-all-reviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _context.reviews.Include(r => r.User).Include(r => r.Offer).ToListAsync();
                if (!reviews.Any()) return NotFound(); 
                return Ok(reviews);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-user-by-offerId/{offerId}")]
        public async Task<IActionResult> GetUserByOfferId([Required]int offerId)
        {
            try
            {
                var offer = await _context.offers.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == offerId);
                if (offer == null)
                {
                    return NotFound("Offer not found.");
                }

                var user = offer.User;
                if (user == null)
                {
                    return NotFound("User not found for the provided offer ID.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-user-by-review-id/{reviewId}")]
        public async Task<IActionResult> GetUserByReviewId([Required] int reviewId)
        {
            try
            {
                var review = await _context.reviews.Include(r => r.User).FirstOrDefaultAsync(r => r.Id == reviewId);

                if (review == null)
                {
                    return NotFound("Review not found.");
                }

                var user = review.User;
                if (user == null)
                {
                    return NotFound("User not found for the given review.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("check-review-status")]
        public async Task<IActionResult> CheckReviewStatus([Required] int userId,[Required] int offerId)
        {
            try
            {
                var review = await _context.reviews
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.OfferId == offerId);

                if (review == null)
                {
                    return Ok(new { Status = "Apply" });
                }

                switch (review.Status)
                {
                    case reviewStatus.Pending:
                        return Ok(new { Status = "Applied" });
                    case reviewStatus.Approved:
                        return Ok(new { Status = "ViewDetails" }); 
                    default:
                        return Ok(new { Status = "Apply" });
                }
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-reviews-for-user-offers/{userId}")]
        public async Task<IActionResult> GetReviewsForUserOffers([Required] int userId)
        {
            try
            {
                var reviews = await _context.reviews.Include(r => r.User).Include(r => r.Offer).Where(r => r.Offer.User_Id == userId).ToListAsync();

                if (!reviews.Any())
                {
                    return NotFound("No reviews found for offers created by the specified user.");
                }

                return Ok(reviews);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    #endregion
}
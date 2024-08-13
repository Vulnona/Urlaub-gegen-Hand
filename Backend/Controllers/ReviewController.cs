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
        public ReviewController(UghContext context)
        {
            _context = context;
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while adding review.");
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
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("get-all-reviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = await _context.reviews.Include(r => r.User).Include(r => r.Offer).ToListAsync();
                if (!reviews.Any()) return NotFound("Review for the entered ID not found."); 
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("get-user-by-offerId/{offerId}")]
        public async Task<IActionResult> GetUserByOfferId([FromQuery][Required]int offerId)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the user details for the given offer id.");
            }
        }
        [HttpGet("get-user-by-review-id/{reviewId}")]
        public async Task<IActionResult> GetUserByReviewId([FromQuery][Required] int reviewId)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while fetching the user for the entered review id.");
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the review status.");
            }
        }
        [HttpGet("get-reviews-for-user-offers/{userId}")]
        public async Task<IActionResult> GetReviewsForUserOffers([FromQuery][Required] int userId)
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while fetching the reviews for the entered user offer.");
            }
        }
    }
    #endregion
}
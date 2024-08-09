using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpPost("adding-review")]
        public IActionResult AddReview(Review review, string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (review == null || string.IsNullOrEmpty(email))
            {
                return BadRequest("Review or email is null.");
            }
            try
            {
                var user = _context.users.FirstOrDefault(u => u.Email_Address == email);
                if (user == null)
                {
                    return NotFound("User with the provided email does not exist.");
                }

                var existingReview = _context.reviews.Include(r => r.User)
                    .FirstOrDefault(r => r.User.Email_Address == email && r.OfferId == review.OfferId);
                if (existingReview != null)
                {
                    return Conflict("A review by this user for this offer already exists.");
                }
                review.UserId = user.User_Id;
                _context.reviews.Add(review);
                _context.SaveChanges();
                return Ok("Review added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }
        [HttpPut("update-review-status")]
        public IActionResult UpdateReviewStatus(int reviewId, reviewStatus newStatus)
        {
            try
            {
                var review = _context.reviews.FirstOrDefault(r => r.Id == reviewId);
                if (review == null)
                {
                    return NotFound("Review not found.");
                }
                review.Status = newStatus;
                _context.SaveChanges();
                return Ok("Review status updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }
        [HttpGet("get-all-reviews")]
        public IActionResult GetAllReviews()
        {
            try
            {
                var reviews = _context.reviews
                    .Include(r => r.User)
                    .Include(r => r.Offer)
                    .ToList();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpGet("get-user-by-offerId/{offerId}")]
        public IActionResult GetUserByOfferId(int offerId)
        {
            try
            {
                var offer = _context.offers
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.Id == offerId);
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
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpGet("get-user-by-review-id/{reviewId}")]
        public IActionResult GetUserByReviewId(int reviewId)
        {
            try
            {
                var review = _context.reviews
                    .Include(r => r.User)
                    .FirstOrDefault(r => r.Id == reviewId);
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
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpGet("check-review-status")]
        public IActionResult CheckReviewStatus(int userId, int offerId)
        {
            try
            {
                var review = _context.reviews
                    .FirstOrDefault(r => r.UserId == userId && r.OfferId == offerId);
                if (review == null)
                {
                    return Ok(new { Status = "Apply" });
                }
                switch (review.Status)
                {
                    case reviewStatus.Pending:
                        return Ok(new { Status = "Applied" });
                    case reviewStatus.Approved:
                        return Ok(new { Status = "ViewDetails" });    // after Approved host can see the detailed of userB  (Call==>  API GetusersByOfferId )
                    default:
                        return Ok(new { Status = "Apply" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpGet("get-reviews-for-user-offers/{userId}")]
        public IActionResult GetReviewsForUserOffers(int userId)
        {
            try
            {
                var reviews = _context.reviews
                    .Include(r => r.User)
                    .Include(r => r.Offer)
                    .Where(r => r.Offer.User_Id == userId)
                    .ToList();
                if (!reviews.Any())
                {
                    return NotFound("No reviews found for offers created by the specified user.");
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

    }

}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/review-user-offer")]
    [ApiController]
    public class ReviewOfferUserController : ControllerBase
    {
        private readonly UghContext _context;

        public ReviewOfferUserController(UghContext context)
        {
            _context = context;
        }
        #region review-offers
        [HttpGet("get-all-reviews")]
        public async Task<IActionResult> GetReviewOfferUsers()
        {
            try
            {
                var reviews = await _context.reviewofferusers
                    .Include(r => r.Offer)
                    .ToListAsync();
                if (!reviews.Any())
                    return NotFound("No reviews found.");

                return Ok(reviews);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while fetching the reviews.");
            }
        }


        [HttpGet("get-review-by-offerId/{id}")]
        public async Task<IActionResult> GetReviewOfferUser([FromQuery][Required] int id)
        {
            try
            {
                var review = await _context.reviewofferusers
                    .Include(r => r.Offer)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (review == null)
                    return NotFound("No review found.");

                return Ok(review);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the review.");
            }
        }

        [HttpPost("create-review")]
        public async Task<IActionResult> CreateReviewOfferUser([FromBody] ReviewOfferUser reviewOfferUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewOfferUser == null)
                return BadRequest();

            try
            {
                var offer = await _context.offers.FirstOrDefaultAsync(o => o.Id == reviewOfferUser.OfferId);
                if (offer == null)
                    return NotFound("Offer not found.");

                var existingReview = await _context.reviewofferusers
                    .FirstOrDefaultAsync(r => r.OfferId == reviewOfferUser.OfferId && r.UserId == reviewOfferUser.UserId);
                if (existingReview != null)
                    return Conflict();

                var reviewLoginUser = await _context.reviewloginusers
                    .FirstOrDefaultAsync(r => r.OfferId == reviewOfferUser.OfferId);
                if (reviewLoginUser != null && reviewLoginUser.IsReviewPeriodOver)
                    return BadRequest();

                reviewOfferUser.UserId = offer.User_Id;

                await _context.reviewofferusers.AddAsync(reviewOfferUser);
                await _context.SaveChangesAsync();

                await AddPostReviewEntry(offer.Id);

                return Ok("Review created successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the review");
            }
        }

        private async Task AddPostReviewEntry([Required]int offerId)
        {
            try
            {
                var reviewOfferUser = await _context.reviewofferusers
                    .Where(r => r.OfferId == offerId)
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefaultAsync();

                var reviewLoginUser = await _context.reviewloginusers
                    .Where(r => r.OfferId == offerId)
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefaultAsync();

                if (reviewOfferUser != null && reviewLoginUser != null)
                {
                    var postReview = new ReviewPost
                    {
                        ReviewOfferUserId = reviewOfferUser.Id,
                        ReviewLoginUserId = reviewLoginUser.Id,
                        OfferUserReviewPost = reviewOfferUser.AddReviewForOfferUser,
                        LoginUserReviewPost = reviewLoginUser.AddReviewForLoginUser,
                        CreatedAt = DateTime.Now
                    };

                    await _context.reviewposts.AddAsync(postReview);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update-review-by-id/{id}")]
        public async Task<IActionResult> UpdateReviewOfferUser([Required] int id, [FromBody] ReviewOfferUser reviewOfferUser)
        {
            if (!ModelState.IsValid || reviewOfferUser == null)
            {
                return BadRequest(ModelState);
            }

            if (id != reviewOfferUser.Id)
            {
                return BadRequest();
            }

            try
            {
                var existingReview = await _context.reviewofferusers.FindAsync(id);
                if (existingReview == null)
                {
                    return NotFound("No existing review found.");
                }

                existingReview.AddReviewForOfferUser = reviewOfferUser.AddReviewForOfferUser;
                existingReview.CreatedAt = reviewOfferUser.CreatedAt; 

                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the review.");
            }
        }

        [HttpDelete("delete-review/{id}")]
        public async Task<IActionResult> DeleteReviewOfferUser([Required] int id)
        {
            try
            {
                var review = await _context.reviewofferusers.FindAsync(id);
                if (review == null)
                {
                    return NotFound("No review found.");
                }

                _context.reviewofferusers.Remove(review);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the review.");
            }
        }

    }
    #endregion
}
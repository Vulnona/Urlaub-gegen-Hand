using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/review-login-user")]
    [ApiController]
    public class ReviewLoginUserController : ControllerBase
    {
        private readonly UghContext _context;

        public ReviewLoginUserController(UghContext context)
        {
            _context = context;
        }
        #region login-user-review
        [HttpGet("get-all-reviews")]
        public async Task<IActionResult> GetReviewLoginUsers()
        {
            try
            {
                var reviews = await _context.reviewloginusers.ToListAsync();
                if (!reviews.Any()) return BadRequest(ModelState); 
                return Ok(reviews);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"No reviews found.");
            }
        }

        [HttpGet("get-review-by-userId/{id}")]
        public async Task<IActionResult> GetReviewLoginUser([FromQuery][Required] int id)
        {
            try
            {
                var review = await _context.reviewloginusers.FindAsync(id);
                if (review == null) return NotFound("No reviews found.");

                return Ok(review);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the reviews by the entered user id.");
            }
        }

        [HttpPost("create-review")]
        public async Task<IActionResult> CreateReviewLoginUser([FromBody] ReviewLoginUser reviewLoginUser)
        {
            try
            {
            if (!ModelState.IsValid) return BadRequest(ModelState);
                var offer = await _context.offers.FirstOrDefaultAsync(o => o.Id == reviewLoginUser.OfferId);
                if (offer == null)
                    return NotFound("Offer not found.");

                var existingReview = await _context.reviewloginusers
                    .FirstOrDefaultAsync(r => r.OfferId == reviewLoginUser.OfferId && r.UserId == reviewLoginUser.UserId);
                if (existingReview != null)
                    return Conflict();

                var reviewOfferUser = await _context.reviewofferusers
                    .FirstOrDefaultAsync(r => r.OfferId == reviewLoginUser.OfferId);
                if (reviewOfferUser != null && reviewOfferUser.IsReviewPeriodOver)
                    return BadRequest();

                _context.reviewloginusers.Add(reviewLoginUser);
                await _context.SaveChangesAsync();

               await AddPostReviewEntry(offer.Id);

                return Ok("Review created successfully for the offer.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while creating the review.");
            }
        }

        private async Task AddPostReviewEntry([Required] int offerId)
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

                    _context.reviewposts.Add(postReview);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw new Exception("An error occurred while adding the post review entry.");
            }
        }

        [HttpPut("update-review-by-id/{id}")]
        public async Task<IActionResult> UpdateReviewLoginUser([FromQuery][Required] int id, [FromBody] ReviewLoginUser reviewLoginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != reviewLoginUser.Id) return BadRequest();

            try
            {
                var existingReview = await _context.reviewloginusers.FindAsync(id);
                if (existingReview == null)
                    return NotFound("No review found.");

                existingReview.AddReviewForLoginUser = reviewLoginUser.AddReviewForLoginUser;
                existingReview.CreatedAt = reviewLoginUser.CreatedAt;

                _context.reviewloginusers.Update(existingReview);
                await _context.SaveChangesAsync();

                return Ok("Review updated successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the review.");
            }
        }

        [HttpDelete("delete-review/{id}")]
        public async Task<IActionResult> DeleteReviewLoginUser([FromQuery][Required] int id)
        {
            try
            {
                var review = await _context.reviewloginusers.FindAsync(id);
                if (review == null) return NotFound("No review found.");

                _context.reviewloginusers.Remove(review);
                await _context.SaveChangesAsync();
                return Ok("Review deleted successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the review.");
            }
        }
    }
    #endregion
}
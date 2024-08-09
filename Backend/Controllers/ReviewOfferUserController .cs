using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/review-offer-user")]
    [ApiController]
    public class ReviewOfferUserController : ControllerBase
    {
        private readonly UghContext _context;

        public ReviewOfferUserController(UghContext context)
        {
            _context = context;
        }

        // GET: api/ReviewOfferUser
        [HttpGet("get-review")]
        public IActionResult GetReviewOfferUsers()
        {
            try
            {
                var reviews = _context.reviewofferusers
                    .Include(r => r.Offer)
                    .ToList();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        // GET: api/ReviewOfferUser/5
        [HttpGet("get-review-offer-user/{id}")]
        public IActionResult GetReviewOfferUser(int id)
        {
            try
            {
                var review = _context.reviewofferusers
                    .Include(r => r.Offer)
                    .FirstOrDefault(r => r.Id == id);
                if (review == null)
                    return NotFound();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpPost("create-review-offer-user")]
        public async Task<IActionResult> CreateReviewOfferUser([FromBody] ReviewOfferUser reviewOfferUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (reviewOfferUser == null)
                return BadRequest("ReviewOfferUser object is null");

            try
            {
                var offer = _context.offers.FirstOrDefault(o => o.Id == reviewOfferUser.OfferId);
                if (offer == null)
                    return NotFound("Offer not found");

                var existingReview = _context.reviewofferusers
                    .FirstOrDefault(r => r.OfferId == reviewOfferUser.OfferId && r.UserId == reviewOfferUser.UserId);
                if (existingReview != null)
                    return Conflict("Duplicate review");

                var reviewLoginUser = _context.reviewloginusers
                    .FirstOrDefault(r => r.OfferId == reviewOfferUser.OfferId);
                if (reviewLoginUser != null && reviewLoginUser.IsReviewPeriodOver)
                    return BadRequest("You cannot add a review before the other user has finished their review period.");

                reviewOfferUser.UserId = offer.User_Id;

                _context.reviewofferusers.Add(reviewOfferUser);
                await _context.SaveChangesAsync();

                AddPostReviewEntry(offer.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }

        private void AddPostReviewEntry(int offerId)
        {
            try
            {
                var reviewOfferUser = _context.reviewofferusers
                    .Where(r => r.OfferId == offerId)
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefault();

                var reviewLoginUser = _context.reviewloginusers
                    .Where(r => r.OfferId == offerId)
                    .OrderByDescending(r => r.CreatedAt)
                    .FirstOrDefault();

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
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }

        [HttpPut("update-review-offer-user/{id}")]
        public IActionResult UpdateReviewOfferUser(int id, [FromBody] ReviewOfferUser reviewOfferUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (reviewOfferUser == null)
                return BadRequest("ReviewOfferUser object is null");

            if (id != reviewOfferUser.Id)
                return BadRequest("ID mismatch");

            try
            {
                var existingReview = _context.reviewofferusers.Find(id);
                if (existingReview == null)
                    return NotFound();

                existingReview.AddReviewForOfferUser = reviewOfferUser.AddReviewForOfferUser;
                existingReview.CreatedAt = reviewOfferUser.CreatedAt;

                _context.reviewofferusers.Update(existingReview);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }

        [HttpDelete("delete-review-offer-user/{id}")]
        public IActionResult DeleteReviewOfferUser(int id)
        {
            try
            {
                var review = _context.reviewofferusers.Find(id);
                if (review == null)
                    return NotFound();

                _context.reviewofferusers.Remove(review);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status501NotImplemented, ex.Message);
            }
        }
    }
}

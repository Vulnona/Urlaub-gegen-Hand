using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("get-review-login-users")]
        public IActionResult GetReviewLoginUsers()
        {
            try
            {
                var reviews = _context.reviewloginusers.ToList();
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpGet("get-review/{id}")]
        public IActionResult GetReviewLoginUser(int id)
        {
            try
            {
                var review = _context.reviewloginusers.Find(id);
                if (review == null) return NotFound();

                return Ok(review);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpPost("create-review-login-user")]
        public async Task<IActionResult> CreateReviewLoginUser([FromBody] ReviewLoginUser reviewLoginUser)
        {
            if (reviewLoginUser == null)
                return BadRequest("ReviewLoginUser object is null");

            try
            {
                var offer = _context.offers.FirstOrDefault(o => o.Id == reviewLoginUser.OfferId);
                if (offer == null)
                    return NotFound("Offer not found");

                var existingReview = _context.reviewloginusers
                    .FirstOrDefault(r => r.OfferId == reviewLoginUser.OfferId && r.UserId == reviewLoginUser.UserId);
                if (existingReview != null)
                    return Conflict("Duplicate review");

                var reviewOfferUser = _context.reviewofferusers
                    .FirstOrDefault(r => r.OfferId == reviewLoginUser.OfferId);
                if (reviewOfferUser != null && reviewOfferUser.IsReviewPeriodOver)
                    return BadRequest("You cannot add a review before the other user has finished their review period.");

                _context.reviewloginusers.Add(reviewLoginUser);
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

        [HttpPut("update-review-login-user/{id}")]
        public IActionResult UpdateReviewLoginUser(int id, [FromBody] ReviewLoginUser reviewLoginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != reviewLoginUser.Id) return BadRequest();

            try
            {
                var existingReview = _context.reviewloginusers.Find(id);
                if (existingReview == null) return NotFound();

                existingReview.AddReviewForLoginUser = reviewLoginUser.AddReviewForLoginUser;
                existingReview.CreatedAt = reviewLoginUser.CreatedAt;

                _context.reviewloginusers.Update(existingReview);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteReviewLoginUser(int id)
        {
            try
            {
                var review = _context.reviewloginusers.Find(id);
                if (review == null) return NotFound();

                _context.reviewloginusers.Remove(review);
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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;


namespace UGHApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserRatingController : ControllerBase
    {
        private readonly UghContext _context;
        public UserRatingController(UghContext context)
        {
            _context = context;
        }
        [HttpPost("add-rating-to-host")]
        public async Task<IActionResult> AddRatingToHost([FromBody] RatingUserLogin ratingUserLogin)
        {
            try
            {

            if (ratingUserLogin == null)
            {
                return BadRequest("RatingUserLogin object is null");
            }
            var offer = await _context.offers.FindAsync(ratingUserLogin.OfferId);
            if (offer == null)
            {
                return NotFound("Offer not found");
            }
            if (offer.User_Id == ratingUserLogin.User_Id)
            {
                return BadRequest("The user ID associated with the offer matches the user ID in the rating");
            }
            var existingRating = await _context.ratinguserlogins
                .FirstOrDefaultAsync(r => r.User_Id == ratingUserLogin.User_Id && r.OfferId == ratingUserLogin.OfferId);
            if (existingRating != null)
            {
                return BadRequest("A rating for this user and offer already exists");
            }
            var rating = new RatingUserLogin
            {
                HostRating = ratingUserLogin.HostRating,
                SubmissionDate = DateTime.UtcNow,
                User_Id = ratingUserLogin.User_Id,
                OfferId = ratingUserLogin.OfferId
            };
            _context.ratinguserlogins.Add(rating);
            await _context.SaveChangesAsync();
            return Ok("Rating submitted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpPost("add-rating-to-user")]
        public async Task<IActionResult> AddRatingToUser([FromBody] RatingHostLogin ratingHostLogin)
        {
            try
            {
                if (ratingHostLogin == null)
                {
                    return Unauthorized("RatingHostLogin object is null");
                }
                var offer = await _context.offers.FindAsync(ratingHostLogin.OfferId);
                if (offer == null)
                {
                    return NotFound("Offer not found");
                }
                if (offer.User_Id != ratingHostLogin.User_Id)
                {
                    var existingRating = await _context.ratinghostlogins
                        .FirstOrDefaultAsync(r => r.User_Id == ratingHostLogin.User_Id && r.OfferId == ratingHostLogin.OfferId);
                    if (existingRating == null)
                    {
                        var rating = new RatingHostLogin
                        {
                            UserRating = ratingHostLogin.UserRating,
                            SubmissionDate = DateTime.UtcNow,
                            User_Id = ratingHostLogin.User_Id,
                            OfferId = ratingHostLogin.OfferId
                        };
                        _context.ratinghostlogins.Add(rating);
                        await _context.SaveChangesAsync();
                        return Ok("Rating submitted successfully");
                    }
                    else
                    {
                        return BadRequest("A rating for this user and offer already exists");
                    }
                }
                else
                {
                    return BadRequest("The user ID associated with the offer matches the user ID in the rating");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }

        [HttpGet("get-rating-by-user-id/{userId}")] //for all
        public async Task<IActionResult> GetRatingByUserId(int userId)
        {
            try
            {
                var ratings = await _context.ratinghostlogins
                    .Include(r => r.Offer)
                    .Where(r => r.User_Id == userId)
                    .ToListAsync();
                if (!ratings.Any())
                {
                    var query = from offer in _context.offers
                                join userRating in _context.ratinguserlogins
                                on offer.Id equals userRating.OfferId
                                where offer.User_Id == userId
                                select new
                                {
                                    userRating.Id,
                                    userRating.HostRating,
                                    userRating.SubmissionDate,
                                    userRating.User_Id,
                                    userRating.OfferId,
                                    Offer = offer
                                };
                    var hostratings = await query.ToListAsync();
                    if (!hostratings.Any())
                    {
                        return NotFound("No ratings found for the specified host ID");
                    }
                    var averageratings = hostratings.Average(r => r.HostRating);
                    var ratingsCounts = hostratings.Count();
                    return Ok(new
                    {
                        AverageRating = averageratings,
                        ratingsCount = ratingsCounts,
                        ratings = hostratings
                    });
                }
                var averageRating = ratings.Average(r => r.UserRating);
                var ratingsCount = ratings.Count();
                return Ok(new
                {
                    AverageRating = averageRating,
                    ratingsCount = ratingsCount,
                    ratings = ratings
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
    }

}
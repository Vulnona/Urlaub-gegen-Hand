using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;


namespace UGHApi.Controllers
{
    [Route("api/user-rating")]
    [ApiController]
    public class UserRatingController : ControllerBase
    {
        private readonly UghContext _context;
        public UserRatingController(UghContext context)
        {
            _context = context;
        }
        #region user-ratings
        [HttpPost("add-rating-to-host")]
        public async Task<IActionResult> AddRatingToHost([FromBody] RatingUserLogin ratingUserLogin)
        {
            try
            {
                if (ratingUserLogin == null)
                {
                    return BadRequest("No ratings found for the logged in user.");
                }

                var offer = await _context.offers.FindAsync(ratingUserLogin.OfferId);
                if (offer == null)
                {
                    return NotFound("No offer found for the logged in user.");
                }
                else if (offer.User_Id == ratingUserLogin.User_Id)
                {
                    return BadRequest();
                }

                var existingRating = await _context.ratinguserlogins.FirstOrDefaultAsync(r => r.User_Id == ratingUserLogin.User_Id && r.OfferId == ratingUserLogin.OfferId);
                if (existingRating != null)
                {
                    return BadRequest("No existing rating found.");
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

                return Ok("User rating added successfully.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the rating to the host.");
            }
        }

        [HttpPost("add-rating-to-user")]
        public async Task<IActionResult> AddRatingToUser([FromBody] RatingHostLogin ratingHostLogin)
        {
            try
            {
                if (ratingHostLogin == null)
                {
                    return Unauthorized();
                }
                var offer = await _context.offers.FindAsync(ratingHostLogin.OfferId);
                if (offer == null)
                {
                    return NotFound("No offer found.");
                }
                if (offer.User_Id != ratingHostLogin.User_Id)
                {
                    var existingRating = await _context.ratinghostlogins.FirstOrDefaultAsync(r => r.User_Id == ratingHostLogin.User_Id && r.OfferId == ratingHostLogin.OfferId);
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
                        return Ok("User rating added successfully.");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status304NotModified, "An error occurred while posting rating to the user.");
            }
        }

        [HttpGet("get-rating-by-user-id/{userId}")]
        public async Task<IActionResult> GetRatingByUserId([FromQuery][Required]int userId)
        {
            try
            {
                var ratings = await _context.ratinghostlogins.Include(r => r.Offer).Where(r => r.User_Id == userId).ToListAsync();
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
                    var hostRatings = await query.ToListAsync();
                    if (!hostRatings.Any())
                    {
                        return BadRequest("No ratings found.");
                    }
                    var averageRatings = hostRatings.Average(r => r.HostRating);
                    var ratingsCounts = hostRatings.Count();
                    return Ok(new
                    {
                        AverageRating = averageRatings,
                        ratingsCount = ratingsCounts,
                        ratings = hostRatings
                    });
                }
                var averageRating = ratings.Average(r => r.UserRating);
                var ratingsCount = ratings.Count();
                return Ok(new
                {
                    AverageRating = averageRating,
                    RatingsCount = ratingsCount,
                    Ratings = ratings
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status204NoContent, "An error occurred while fetching ratings by user id.");
            }
        }
        #endregion
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ReviewPostController : ControllerBase
    {
        private readonly UghContext _context;

        public ReviewPostController(UghContext context)
        {
            _context = context;
        }

        [HttpGet("post-review/{userId}")]
        public async Task<ActionResult<IEnumerable<ReviewPost>>> GetPostreviewsByUserId(int userId)
        {
            try
            {
                var postreviews = await _context.reviewposts
                    .Include(r => r.ReviewLoginUser)
                    .Include(r => r.ReviewLoginUser.Offer)
                    .Where(r => r.ReviewOfferUser.Offer.User_Id == userId || r.ReviewLoginUser.UserId == userId)
                    .ToListAsync();

                if (!postreviews.Any())
                {
                    return NotFound();
                }

                return Ok(postreviews);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpGet("post-review/get-all-post")]
        public IActionResult GetAllPost()
        {
            try
            {
                var getall = _context.reviewposts.ToList();
                return Ok(getall);
            }
            catch (Exception ex)
            {
                // Log the exception 
                Console.Error.WriteLine("No posts found! Please ensure to have posts in d");

                // Return a generic error message to the client
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
    }
}
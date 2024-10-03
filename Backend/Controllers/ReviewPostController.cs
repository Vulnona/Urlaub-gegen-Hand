//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using UGHApi.Models;

//namespace UGHApi.Controllers
//{
//    [Route("api/post-review")]
//    [ApiController]
//    public class ReviewPostController : ControllerBase
//    {
//        private readonly Ugh_Context _context;
//        private readonly ILogger<ReviewPostController> _logger;

//        public ReviewPostController(Ugh_Context context, ILogger<ReviewPostController> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }
//        #region post-user-review

//        [HttpGet("get-posted-review-by-user-id/{userId}")]
//        public async Task<ActionResult<IEnumerable<ReviewPost>>> GetPostReviewsByUserId([Required] int userId)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                var postReviews = await _context.reviewposts.Include(r => r.ReviewLoginUser).Include(r => r.ReviewLoginUser.Offer).Where(r => r.ReviewOfferUser.Offer.User_Id == userId || r.ReviewLoginUser.UserId == userId).ToListAsync();

//                if (!postReviews.Any())
//                {
//                    return NotFound();
//                }

//                return Ok(postReviews);
//            }
//            catch (Exception ex)
//            {
//               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }

//        [HttpGet("get-all-posts")]
//        public async Task<ActionResult<List<ReviewPost>>> GetAllPost()
//        {
//            try
//            {
//                var getAll = await _context.reviewposts.ToListAsync();

//                if (getAll == null || !getAll.Any())
//                {
//                    return BadRequest();
//                }
//                return Ok(getAll);
//            }
//            catch (Exception ex)
//            {
//               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
//                return StatusCode(500, $"Internal server error: {ex.Message}");
//            }
//        }
//        #endregion
//    }
//}
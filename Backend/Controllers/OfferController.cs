using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/offer")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly ILogger<OfferController> _logger;
        public OfferController(UghContext context, ILogger<OfferController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region offers
        [HttpGet("get-all-offers")]
        public async Task<IActionResult> GetOffersAsync(string searchTerm)
        {
            try
            {
                IQueryable<Offer> query = _context.offers.Include(o => o.User);

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(o => o.Title.Contains(searchTerm) || o.skills.Contains(searchTerm) || o.Location.Contains(searchTerm) || o.state.Contains(searchTerm));
                }

                var result = await query.ToListAsync();
                if(result.IsNullOrEmpty()) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-offer-by-id/{OfferId:int}")]
        public async Task<IActionResult> GetOfferAsync([Required]int OfferId)
        {
            try
            {
                var offer = await _context.offers.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == OfferId);

                if (offer == null)
                    return NotFound();

                return Ok(offer);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("add-new-offer")]
        public async Task<IActionResult> AddOffer([FromForm] OfferViewModel offerViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var user = _context.users.Include(u => u.CurrentMembership).FirstOrDefault(u => u.User_Id == offerViewModel.User_Id);

                if (user == null)
                    return BadRequest("User not found.");

                if (user.CurrentMembership == null)
                {
                    return BadRequest("User is not authorized to add an offer.");
                }
                var offer = new Offer
                {
                    Title = offerViewModel.Title,
                    Description = offerViewModel.Description,
                    Location = offerViewModel.Location,
                    Contact = offerViewModel.Contact,
                    Accomodation = offerViewModel.Accommodation,
                    accomodationsuitable = offerViewModel.AccommodationSuitable,
                    skills=offerViewModel.Skills,
                    User_Id = offerViewModel.User_Id,
                    country=offerViewModel.Country,
                    state=offerViewModel.State,
                    city=offerViewModel.City,
                };

                if (offerViewModel.Image != null && offerViewModel.Image.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await offerViewModel.Image.CopyToAsync(memoryStream);
                    offer.ImageData = memoryStream.ToArray();
                    offer.ImageMimeType = offerViewModel.Image.ContentType;
                }

                _context.offers.Add(offer);
                await _context.SaveChangesAsync();
                _logger.LogInformation("New Offer Added Successfully!");
                return Ok();
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("delete-offer/{OfferId:int}")]
        public async Task<IActionResult> DeleteOfferAsync([Required]int OfferId)
        {
            try
            {
                var offer = await _context.offers.FindAsync(OfferId);
                if (offer == null)
                {
                    return NotFound("Offer not found.");
                }

                _context.offers.Remove(offer);
                await _context.SaveChangesAsync();
                return Ok(offer);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
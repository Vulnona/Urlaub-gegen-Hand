using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly UghContext _context;
        public OfferController(UghContext context)
        {
            _context = context;
        }
        [HttpGet("offer/get-all-offers")]
        public IActionResult Getoffers(string searchTerm)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var searchResult = _context.offers
                        
                        .Include(o => o.User)
                        .Where(o => o.Title.Contains(searchTerm) || o.skills.Contains(searchTerm) || o.Location.Contains(searchTerm) ||o.state.Contains(searchTerm))
                        .ToList();

                    return Ok(searchResult);
                }
                else
                {
                    var alloffers = _context.offers
                        
                         .Include(o => o.User)
                        .ToList();
                    return Ok(alloffers);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }


        [HttpGet("offer/get-offer-by-id/{OfferId:int}")]
        public IActionResult GetOffer(int OfferId)
        {
            try
            {
                var offer = _context.offers
                   
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.Id == OfferId);

                if (offer == null)
                    return NotFound("Offer not found.");

                return Ok(offer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpPost("offer/add-new-offer")]
        public async Task<IActionResult> AddOffer([FromForm] OfferViewModel offerViewModel, string email)
        {
            try
            {
                if (offerViewModel == null)
                    return BadRequest("Offer data is null.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var user = _context.users.Include(u => u.CurrentMembership).FirstOrDefault(u => u.Email_Address == email);

                if (user == null)
                    return BadRequest("User not found.");

                // Check if CurrentMembership exists and its MembershipID is greater than 033
                if (user.CurrentMembership == null)
                {
                    return BadRequest("User is not authorized to add an offer.");
                }
                // Create a new Offer object
                var offer = new Offer
                {
                    Title = offerViewModel.Title,
                    Description = offerViewModel.Description,
                    Location = offerViewModel.Location,
                    Contact = offerViewModel.Contact,
                    Accomodation = offerViewModel.Accomodation,
                    accomodationsuitable = offerViewModel.accomodationsuitable,
                    skills=offerViewModel.skills,
                    //Region_ID = offerViewModel.Region_ID,
                    User_Id = offerViewModel.User_Id,
                    country=offerViewModel.country,
                    state=offerViewModel.state,
                    city=offerViewModel.city,
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

                return Ok("Offer successfully posted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, ex.Message);
            }
        }

        [HttpPut("offer/update-offer")]
        public IActionResult UpdateOffer([FromBody] Offer offer)
        {
            try
            {
                if (offer == null) return NotFound();
                if (!ModelState.IsValid) return BadRequest(ModelState);
                _context.offers.Update(offer);
                _context.SaveChanges();
                return Ok(offer);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);

            }
        }

        [HttpDelete("offer/delete-offer/{OfferId:int}")]
        public IActionResult DeleteOffer(int OfferId)
        {
            try
            {
                var offer = _context.offers.Find(OfferId);
                if (offer == null)
                    return NotFound("Offer not found.");

                _context.offers.Remove(offer);
                _context.SaveChanges();

                return Ok(offer);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
        public class OfferModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
        }
    }
}
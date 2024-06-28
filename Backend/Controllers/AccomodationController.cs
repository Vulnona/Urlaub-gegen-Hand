using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;
 
namespace UGHApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccomodationController : ControllerBase
    {
        private readonly UghContext _context;
        public AccomodationController(UghContext context)
        {
            _context = context;
        }
        [HttpGet("accomodation/get-all-accomodations")]
        public IActionResult GetAccomodation()
        {
            try
            {
                var accomodation = _context.accomodations.ToList();
                return Ok(accomodation);
            }
            catch (Exception ex) 
            { 
             return StatusCode(StatusCodes.Status204NoContent, ex.Message);

            }
        }
        [HttpGet("accomodation/get-accomodation-by-id/{AccomodationId:int}")]
        public IActionResult Getaccomodations(int AcomodationId)
        {
            try
            {
                var accomodation = _context.accomodations.Find(AcomodationId);
                if (accomodation == null) return NotFound();
                return Ok(accomodation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent,ex.Message);
            }
        }
        [HttpPost("accomodation/add-new-accomodation")]
        public IActionResult AddAccomodation([FromBody] Accomodation accomodation)
        {
            try
            {
                if (accomodation == null)
                    return BadRequest("Offer data is null.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.accomodations.Add(accomodation);
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }
        [HttpPut("accomodation/update-accomodation")]
        public IActionResult UpdateAccomodation([FromBody] Accomodation accomodation)
        {
            try
            {
                if (accomodation == null) return NotFound();
                if (!ModelState.IsValid) return BadRequest(ModelState);
                _context.accomodations.Update(accomodation);
                _context.SaveChanges();
                return Ok(accomodation);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);

            }
        }
        [HttpDelete("accomodation/delete-accomodation/{AccommodationId:int}")]
        public IActionResult DeleteAccommodation(int AccommodationId)
        {
            try
            {
                var accommodation = _context.accomodations.Find(AccommodationId);
                if (accommodation == null)
                    return NotFound("Accommodation not found.");

                _context.accomodations.Remove(accommodation); 
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }

    }
}
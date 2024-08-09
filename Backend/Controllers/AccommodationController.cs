using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/accommodation")]
    [ApiController]
    public class AccommodationController : ControllerBase
    {
        private readonly UghContext _context;
        public AccommodationController(UghContext context)
        {
            _context = context;
        }
        [HttpGet("accommodation/get-all-accommodations")]
        public IActionResult GetAccommodations()
        {
            try
            {
                var getAllAccommodations = _context.accomodations.ToList();
                return Ok(getAllAccommodations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);

            }
        }
        [HttpGet("accommodation/get-accommodation-by-id/{accommodationId:int}")]
        public IActionResult GetAccommodation(int accommodationId)
        {
            try
            {
                var getAccommodation = _context.accomodations.Find(accommodationId);
                if (getAccommodation == null)
                    return NotFound();
                return Ok(getAccommodation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpPost("accommodation/add-new-accommodation")]
        public IActionResult AddAccommodation([FromBody] Accommodation accommodation)
        {
            try
            {
                if (accommodation == null || !ModelState.IsValid)
                return BadRequest("Accommodation for the offer is null!");
                _context.accomodations.Add(accommodation);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }
        [HttpPut("accommodation/update-accommodation")]
        public IActionResult UpdateAccommodation([FromBody] Accommodation accommodation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return NotFound();
                _context.accomodations.Update(accommodation);
                _context.SaveChanges();
                return Ok(accommodation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);

            }
        }
        [HttpDelete("accommodation/delete-accommodation/{accommodationId:int}")]
        public IActionResult DeleteAccommodation(int accommodationId)
        {
            try
            {
                var findAccommodation = _context.accomodations.Find(accommodationId);
                if (findAccommodation == null)
                    return NotFound("Accommodation not found.");
                _context.accomodations.Remove(findAccommodation);
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
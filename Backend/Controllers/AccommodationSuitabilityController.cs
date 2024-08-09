using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/accommodation-suitability")]
    [ApiController]
    public class AccommodationSuitabilityController : ControllerBase
    {
        private readonly UghContext _context;
        public AccommodationSuitabilityController(UghContext context)
        {
            _context = context;
        }
        [HttpGet("get-all-suitable-accommodations")]
        public IActionResult GetAllAccommodationSuitability ()
        {
            try
            {
                var result = _context.accommodationsuitables.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpGet("get-suitable-accommodation-by-id/{suitableAccommodationId:int}")]
        public IActionResult GetSuitableAccommodationById(int suitableAccommodationId)
        {
            try
            {
                var suitableAccommodation = _context.accommodationsuitables.Find(suitableAccommodationId);
                if (suitableAccommodation == null) 
                    return NotFound();
                return Ok(suitableAccommodation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpPost("add-new-accommodation")]
        public IActionResult AddSuitableAccommodation([FromBody] SuitableAccommodation suitableAccommodation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Suitable accommodation is null.");
                _context.accommodationsuitables.Add(suitableAccommodation);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }
        [HttpPut("update-accommodation-by-id")]
        public IActionResult UpdateSuitableAccommodation([FromBody] SuitableAccommodation suitableAccommodation)
        {
            try
            {
                if (!ModelState.IsValid) 
                    return NotFound();
                _context.accommodationsuitables.Update(suitableAccommodation);
                _context.SaveChanges();
                return Ok(suitableAccommodation);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);

            }
        }
        [HttpDelete("delete-accommodation/{suitableAccommodationId:int}")]
        public IActionResult DeleteSuitableAccommodation(int accommodationSuitableId)
        {
            try
            {
                var findAccommodationSuitable = _context.accommodationsuitables.Find(accommodationSuitableId);
                if (findAccommodationSuitable == null)
                    return NotFound("Accommodation not found.");

                _context.accommodationsuitables.Remove(findAccommodationSuitable);
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

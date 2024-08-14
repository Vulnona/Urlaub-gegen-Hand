using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/accommodation")]
    [ApiController]

    public class AccommodationController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly ILogger<AccommodationController> _logger;
        public AccommodationController(UghContext context, ILogger<AccommodationController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region Accommodations
        [HttpGet("get-all-accommodations")]
        public async Task<IActionResult> GetAccommodations()
        {
            try
            {
                var getAllAccommodations = await _context.accomodations.ToListAsync();
                if(!getAllAccommodations.Any()) return NotFound();
                return Ok(getAllAccommodations);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-accommodation-by-id/{accommodationId:int}")]
        public async Task<IActionResult> GetAccommodation([Required] int accommodationId)
        {
            try
            {
                var getAccommodation = await _context.accomodations.FindAsync(accommodationId);
                if (getAccommodation == null) return NotFound();
                return Ok(getAccommodation);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add-new-accommodation")]
        public async Task<IActionResult> AddAccommodation([FromBody] Accommodation accommodation)
        {
            try
            {
                if (accommodation == null || !ModelState.IsValid)
                    return BadRequest("Accommodation for the offer is null!");

                await _context.accomodations.AddAsync(accommodation);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-accommodation")]
        public async Task<IActionResult> UpdateAccommodation([FromBody] Accommodation accommodation)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                _context.accomodations.Update(accommodation);
                await _context.SaveChangesAsync();
                return Ok(accommodation);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-accommodation/{accommodationId:int}")]
        public async Task<IActionResult> DeleteAccommodation([Required] int accommodationId)
        {
            try
            {
                var findAccommodation = await _context.accomodations.FindAsync(accommodationId);
                if (findAccommodation == null)
                    return NotFound("Accommodation not found.");

                _context.accomodations.Remove(findAccommodation);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    #endregion
}

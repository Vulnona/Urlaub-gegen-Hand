using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UGH.Domain.Entities;
using UGHApi.DATA;

namespace UGHApi.Controllers
{
    [Route("api/accommodation")]
    [ApiController]

    public class AccommodationController : ControllerBase
    {
        private readonly Ugh_Context _context;
        private readonly ILogger<AccommodationController> _logger;
        public AccommodationController(Ugh_Context context, ILogger<AccommodationController> logger)
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
    }
    #endregion
}

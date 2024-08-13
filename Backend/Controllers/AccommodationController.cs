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

        public AccommodationController(UghContext context)
        {
            _context = context;
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
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
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
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
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
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
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
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
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
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
    #endregion
}

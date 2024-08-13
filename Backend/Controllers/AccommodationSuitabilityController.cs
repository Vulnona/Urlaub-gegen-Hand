using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UGHApi.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

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

        #region accommodation-suitablity
        [HttpGet("get-all-suitable-accommodations")]
        public async Task<IActionResult> GetAllAccommodationSuitability()
        {
            try
            {
                var result = await _context.accommodationsuitables.ToListAsync();
                if (!result.Any()) return NotFound();
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status204NoContent,"Suitable Accommodations not found!");
            }
        }

        [HttpGet("get-suitable-accommodation-by-id/{suitableAccommodationId:int}")]
        public async Task<IActionResult> GetSuitableAccommodationById([FromQuery][Required] int suitableAccommodationId)
        {
            try
            {
                var suitableAccommodation = await _context.accommodationsuitables.FindAsync(suitableAccommodationId);
                if (suitableAccommodation == null)
                    return NotFound("Suitable Accommodation can not be null");
                return Ok(suitableAccommodation);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status204NoContent,"Suitable Accomodation not found");
            }
        }

        [HttpPost("add-new-accommodation")]
        public async Task<IActionResult> AddSuitableAccommodation([FromBody] SuitableAccommodation suitableAccommodation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Suitable accommodation is null.");

                await _context.accommodationsuitables.AddAsync(suitableAccommodation);
                await _context.SaveChangesAsync();

                return Ok("Suitable Accommodation added successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status304NotModified, "An error occurred while adding the suitable accommodation");
            }
        }

        [HttpPut("update-accommodation-by-id")]
        public async Task<IActionResult> UpdateSuitableAccommodation([FromBody] SuitableAccommodation suitableAccommodation)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.accommodationsuitables.Update(suitableAccommodation);
                await _context.SaveChangesAsync();
                return Ok("Suitable Accommodation updated successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status304NotModified,"Error occurred while updating the suitable accommodation");
            }
        }

        [HttpDelete("delete-accommodation/{suitableAccommodationId:int}")]
        public async Task<IActionResult> DeleteSuitableAccommodation([Required] int accommodationSuitableId)
        {
            try
            {
                var findAccommodationSuitable = await _context.accommodationsuitables.FindAsync(accommodationSuitableId);
                if (findAccommodationSuitable == null)
                    return NotFound("Accommodation not found.");

                _context.accommodationsuitables.Remove(findAccommodationSuitable);
                await _context.SaveChangesAsync();

                return Ok("Suitabl Accommodation deleted successfully!");
            }
            catch (Exception )
            {
                return StatusCode(StatusCodes.Status400BadRequest, "An error occurred while deleting the Suitable Accommodation");
            }
        }
        #endregion
    }
}

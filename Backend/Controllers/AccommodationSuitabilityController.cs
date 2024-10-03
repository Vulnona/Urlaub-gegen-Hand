using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UGH.Domain.Entities;

namespace UGHApi.Controllers;

[Route("api/accommodation-suitability")]
[ApiController]
public class AccommodationSuitabilityController : ControllerBase
{
    private readonly Ugh_Context _context;
    private readonly ILogger<AccommodationSuitabilityController> _logger;

    public AccommodationSuitabilityController(
        Ugh_Context context,
        ILogger<AccommodationSuitabilityController> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    #region accommodation-suitablity
    [HttpGet("get-all-suitable-accommodations")]
    public async Task<IActionResult> GetAllAccommodationSuitability()
    {
        try
        {
            var result = await _context.accommodationsuitables.ToListAsync();
            if (!result.Any())
                return NotFound();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("get-suitable-accommodation-by-id/{suitableAccommodationId:int}")]
    public async Task<IActionResult> GetSuitableAccommodationById(
        [Required] int suitableAccommodationId
    )
    {
        try
        {
            var suitableAccommodation = await _context.accommodationsuitables.FindAsync(
                suitableAccommodationId
            );
            if (suitableAccommodation == null)
                return NotFound();
            return Ok(suitableAccommodation);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("add-new-accommodation")]
    public async Task<IActionResult> AddSuitableAccommodation(
        [FromBody] SuitableAccommodation suitableAccommodation
    )
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest("Suitable accommodation is null.");

            await _context.accommodationsuitables.AddAsync(suitableAccommodation);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status304NotModified, ex.Message);
        }
    }

    [HttpPut("update-accommodation-by-id")]
    public async Task<IActionResult> UpdateSuitableAccommodation(
        [FromBody] SuitableAccommodation suitableAccommodation
    )
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.accommodationsuitables.Update(suitableAccommodation);
            await _context.SaveChangesAsync();
            return Ok(suitableAccommodation);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("delete-accommodation/{suitableAccommodationId:int}")]
    public async Task<IActionResult> DeleteSuitableAccommodation(
        [Required] int accommodationSuitableId
    )
    {
        try
        {
            var findAccommodationSuitable = await _context.accommodationsuitables.FindAsync(
                accommodationSuitableId
            );
            if (findAccommodationSuitable == null)
                return NotFound("Accommodation not found.");

            _context.accommodationsuitables.Remove(findAccommodationSuitable);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
    #endregion
}

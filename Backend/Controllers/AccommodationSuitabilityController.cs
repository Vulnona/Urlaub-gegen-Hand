using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UGH.Domain.Entities;
using UGHApi.DATA;

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
    #endregion
}

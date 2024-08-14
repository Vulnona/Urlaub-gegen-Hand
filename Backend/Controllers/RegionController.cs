using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace UGHApi.Controllers
{
    [Route("api/region")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly UghContext _context;
        private readonly ILogger<RegionController> _logger;
        public RegionController(UghContext context, ILogger<RegionController> logger)
        {
            _context = context;
            _logger = logger;
        }
        #region regions
        [HttpGet("get-all-regions")]
        public async Task<IActionResult> GetRegion()
        {
            try
            {
                var regions = await _context.regions.ToListAsync();
                if(!regions.Any()) return NotFound();
                return Ok(regions);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-all-countries")]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                var countries = await _context.countries.ToListAsync();
                if(!countries.Any()) return NotFound();
                return Ok(countries);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-state-by-countryId/{countryId}")]
        public async Task<IActionResult> GetState([Required]int countryId)
        {
            try
            {
                var states = await _context.states.Where(s => s.CountryId == countryId).ToListAsync();
                if(!states.Any()) return NotFound();
                return Ok(states);
            }
            catch (Exception ex)
            {
               _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("get-city-by-stateId/{stateId}")]
        public async Task<IActionResult> GetCity([Required]int stateId)
        {
            try
            {
                var cities = await _context.cities.Where(c => c.StateId == stateId).ToListAsync();
                if(!cities.Any()) return NotFound();
                return Ok(cities);
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
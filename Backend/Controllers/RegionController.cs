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
        public RegionController(UghContext context)
        {
            _context = context;
        }
        #region regions
        [HttpGet("get-all-regions")]
        public async Task<IActionResult> GetRegion()
        {
            try
            {
                var regions = await _context.regions.ToListAsync();
                if(!regions.Any()) return NotFound("No Regions found.");
                return Ok(regions);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status204NoContent,"An error occurred while fetching the Regions.");
            }
        }

        [HttpGet("get-all-countries")]
        public async Task<IActionResult> GetCountry()
        {
            try
            {
                var countries = await _context.countries.ToListAsync();
                if(!countries.Any()) return NotFound("No countries found.");
                return Ok(countries);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred while fetching the countries.");
            }
        }
        [HttpGet("get-state-by-countryId/{countryId}")]
        public async Task<IActionResult> GetState([FromQuery][Required]int countryId)
        {
            try
            {
                var states = await _context.states.Where(s => s.CountryId == countryId).ToListAsync();
                if(!states.Any()) return NotFound("No states found.");
                return Ok(states);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the states.");
            }
        }
        [HttpGet("get-city-by-stateId/{stateId}")]
        public async Task<IActionResult> GetCity([FromQuery][Required]int stateId)
        {
            try
            {
                var cities = await _context.cities.Where(c => c.StateId == stateId).ToListAsync();
                if(!cities.Any()) return NotFound("No cities found.");
                return Ok(cities);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching the cities.");
            }
        }

    }
    #endregion
}
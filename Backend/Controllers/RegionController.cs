using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("getall-region")]
        public IActionResult GetRegion()
        {
            try
            {
            var region = _context.regions.ToList();
            return Ok(region);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }

        [HttpGet("getall-country")]
        public IActionResult GetCountry()
        {
            try
            {
                var country = _context.countries.ToList();
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpGet("get-state-bycountryId/{countryId}")]
        public IActionResult GetState(int countryId)
        {
            try
            {
                var states = _context.states
                    .Where(s => s.CountryId == countryId)
                    .ToList();
                return Ok(states);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("get-city-bystateId/{stateId}")]
        public IActionResult GetCity(int stateId)
        {
            try
            {
                var cities = _context.cities
                    .Where(s => s.StateId == stateId)
                    .ToList();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }

 }

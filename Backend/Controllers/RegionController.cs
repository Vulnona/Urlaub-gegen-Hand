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
    }

 }

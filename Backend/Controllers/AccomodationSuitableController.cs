using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class accomodationsuitableController : ControllerBase
    {
        private readonly UghContext _context;
        public accomodationsuitableController(UghContext context)
        {
            _context = context;
        }
        [HttpGet("suitable-accomodation/get-all-suitable-accomodation")]
        public IActionResult GetAllaccomodationsuitable()
        {
            try
            {
                var result = _context.accomodationsuitables.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }

        }
        [HttpGet("suitable-accomodation/get-suitable-accomodation-by-id/{accomodationsuitableId:int}")]
        public IActionResult Getaccomodationsuitables(int accomodationsuitableId)
        {
            try
            {
                var accomodationsuitable = _context.accomodationsuitables.Find(accomodationsuitableId);
                if (accomodationsuitable == null) return NotFound();
                return Ok(accomodationsuitable);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status204NoContent, ex.Message);
            }
        }
        [HttpPost("suitable-accomodation/add-new-accomodation")]
        public IActionResult Addaccomodationsuitable([FromBody] accomodationsuitable accomodationsuitable)
        {
            try
            {
                if (accomodationsuitable == null)
                    return BadRequest("accomodationsuitable data is null.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                _context.accomodationsuitables.Add(accomodationsuitable);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);
            }
        }
        [HttpPut("suitable-accomodation/update-accomodation-by-id")]
        public IActionResult Updateaccomodationsuitable([FromBody] accomodationsuitable accomodationsuitable)
        {
            try
            {
                if (accomodationsuitable == null) return NotFound();
                if (!ModelState.IsValid) return BadRequest(ModelState);
                _context.accomodationsuitables.Update(accomodationsuitable);
                _context.SaveChanges();
                return Ok(accomodationsuitable);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status304NotModified, ex.Message);

            }
        }
        [HttpDelete("suitable-accomodation/delete-accomodation/{accomodationsuitableId:int}")]
        public IActionResult Deleteaccomodationsuitable(int accomodationsuitableId)
        {
            try
            {
                var accomodationsuitable = _context.accomodationsuitables.Find(accomodationsuitableId);
                if (accomodationsuitable == null)
                    return NotFound("Accommodation not found.");

                _context.accomodationsuitables.Remove(accomodationsuitable);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}

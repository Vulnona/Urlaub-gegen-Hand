using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/custom-mail")]
    [ApiController]
    public class CustomMailController : ControllerBase
    {
        private readonly EmailService _emailService;
        private readonly ILogger<CustomMailController> _logger;
        public CustomMailController(EmailService emailService,ILogger<CustomMailController> logger)
        {
            _emailService = emailService;   
            _logger = logger;
        }
        #region send-custom-verification-email
        [HttpPost("send")]
        public async Task<IActionResult> CustomSendEmail([FromBody] CustomMailBody mailBody)
        {
            try
            {
                await _emailService.SendEmailAsync(mailBody.To, mailBody.Subject, mailBody.Body);
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
}

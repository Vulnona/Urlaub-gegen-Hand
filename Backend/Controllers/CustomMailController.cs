using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CustomMailController : ControllerBase
    {
        private readonly EmailService _emailService;
        public CustomMailController(EmailService emailService)
        {
            _emailService = emailService;   
        }
        [HttpPost("custom-mail/send")]
        public IActionResult CustomSendEmail([FromBody] CustomMailBody mailBody)
        {
            try
            {
                _emailService.SendEmail(mailBody.to, mailBody.subject, mailBody.body);
                return Ok("Email sent successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send email: {ex.Message}");
            }
        }
    }
}

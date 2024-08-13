using Microsoft.AspNetCore.Mvc;
using UGHApi.Models;

namespace UGHApi.Controllers
{
    [Route("api/custom-mail")]
    [ApiController]
    public class CustomMailController : ControllerBase
    {
        private readonly EmailService _emailService;
        public CustomMailController(EmailService emailService)
        {
            _emailService = emailService;   
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
                return StatusCode(500,ex.Message);
            }
        }
        #endregion
    }
}

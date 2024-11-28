namespace UGH.Infrastructure.Services;
using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MimeKit;



public class EmailService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _logger = logger;
    }
    #region email-services
    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_configuration["MailSettings:DisplayName"], _configuration["MailSettings:Mail"]));
            emailMessage.To.Add(new MailboxAddress("", recipientEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), false);
                await client.AuthenticateAsync(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> SendVerificationEmailAsync(string email, Guid verificationToken)
    {
        try
        {
            var currentRequest = _httpContextAccessor.HttpContext?.Request;
            if (currentRequest == null)
            {
                throw new InvalidOperationException("HttpContext is not available.");
            }

            string currentScheme = currentRequest.Scheme;
            string currentHost = currentRequest.Host.Value;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["MailSettings:DisplayName"], _configuration["MailSettings:Mail"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Verify your email address";
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h2>Please 
                        <a href='{currentScheme}://{currentHost}/api/authenticate/verify-email?token={verificationToken}'>
                            click here
                        </a> 
                    to verify your email address.</h2>"
            };
            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), false);
                await client.AuthenticateAsync(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            _logger.LogInformation("Verification Email Sent Successfully To The User: {email}", email);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return false;
        }
    }
    public bool IsValidEmail(string email)
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
    #endregion
}

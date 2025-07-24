using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;
using UGHApi.Models;

namespace UGH.Infrastructure.Services;

public class EmailService
{
    private readonly IConfiguration _configuration;
    private readonly TemplateSettings _templateSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, IOptions<TemplateSettings> templateSettings, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _templateSettings = templateSettings.Value;
        _logger = logger;        
    }
    #region email-services
    public async Task<bool> SendEmailAsync(string recipientEmail, string subject, string body)
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
             _logger.LogInformation($"Email sent successfully to {recipientEmail}");
             return true;
        }
        catch (Exception ex)
        {
           _logger.LogError($"Exception occurred sending Mail: {ex.Message} | StackTrace: {ex.StackTrace}");
           return false;
        }
    }
    public async Task<bool> SendVerificationEmailAsync(string email, Guid verificationToken)
    {
        try
        {
            String htmlBody = $"<h2><a href='{_configuration["BaseUrl"]}/api/authenticate/verify-email?token={verificationToken}'>Klicke hier</h2> </a>um deine Emailadresse zu verifizieren.</p>";
            var emailSent = await SendEmailAsync(email,"Verifiziere deine Emailadresse", htmlBody); 
            if (emailSent)
            {
                _logger.LogInformation("Verification Email Sent Successfully To The User: {email}",email);
                return true;
            }
            else
            {
                _logger.LogError("Failed to send verification email to {email}",email);
                return false;
            }
        }
        catch (Exception ex)
        {
           _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return false;
        }
    }
    
        public async Task<bool> SendResetPasswordEmailAsync(string email, Guid verificationToken)
    {
        try
        {
            String htmlBody = $"<h2><a href='{_configuration["BaseUrlFrontend"]}/change-password?token={verificationToken}'>Klicke hier</h2> </a>um dein Passwort neu zu setzen.</p>";
            var emailSent = await SendEmailAsync(email,"Passwort-Reset-Link", htmlBody); 
            if (emailSent)
            {
                _logger.LogInformation("Password-Reset-Link Sent Successfully To The User: {email}",email);
                return true;
            }
            else
            {
                _logger.LogError("Failed to send password reset email to {email}",email);
                return false;
            }
        }
        catch (Exception ex)
        {
           _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return false;
        }
    }
    
    public async Task<bool> SendTemplateEmailAsync(string RecipientEmail, string Template, string FirstName, String ExpirationDate = "", int DaysRemaining=0)
    {
        try
        {
            string subject;
            string templatePath;
            switch(Template){
                case string str when str.Equals("Verified"):
                    subject="Dein Account wurde verifiziert.";
                    templatePath = _templateSettings.SuccessTemplate;
                    break;
                case string str when str.Equals("Verification Failed"):
                    subject="Die Verifikation deines Accounts wurde abgelehnt.";
                    templatePath = _templateSettings.FailedTemplate;
                    break;
                default:
                    throw new Exception($"Invalid Call parameter for function SendTemplateEmailAsync: '{Template}'.");
            }
            if (!string.IsNullOrEmpty(templatePath))
            {
                string body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("FirstName", FirstName);
                // if there is no ExpirationDate in the body, replace won't do anything (the cost is probably neglectable)
                body = body.Replace("ExpirationDate", ExpirationDate);
                var emailSent = await SendEmailAsync(RecipientEmail, subject, body); 
                if (emailSent)
                {
                    _logger.LogInformation("{1} Email sent successfully", Template);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to send {Template} email to {RecipientEmail}", Template, RecipientEmail);
                    return false;
                }
            }
            else
            {
                throw new FileNotFoundException($"Template file for status '{Template}' not found.");
            }
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

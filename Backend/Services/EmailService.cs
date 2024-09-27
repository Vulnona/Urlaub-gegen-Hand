using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;
using UGHApi.Models;

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
            String htmlBody = $"<h2>Please <a href='{_configuration["BaseUrl"]}/api/authenticate/verify-email?token={verificationToken}'>click here</h2> </a>to verify your email address.</p>";
            await SendEmailAsync(email,"Verify your email address", htmlBody); 
            _logger.LogInformation("Verification Email Sent Successfully To The User: {email}",email);
            return true;
        }
        catch (Exception ex)
        {
           _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            return false;
        }
    }
    public async Task SendTemplateEmailAsync(string RecipientEmail, string Status, string FirstName)
    {
        _logger.LogInformation("Entering verification {1},{2},{3}",RecipientEmail, Status, FirstName);
        try
        {
            //string templatePath = GetTemplatePath(Status);
            string subject;
            string templatePath;
            switch(Status){
                case string str when str.Equals("Verified"):
                    subject="Dein Account wurde verifiziert.";
                    templatePath = _templateSettings.SuccessTemplate;
                    break;
                case string str when str.Equals("Verification Failed"):
                    subject="Die Verifikation deines Accounts wurde abgelehnt";
                    templatePath = _templateSettings.FailedTemplate;
                    break;
                default:
                    throw new Exception($"Invalid Call parameter '{Status}'.");
            }
            if (!string.IsNullOrEmpty(templatePath))
            {
                string body = await File.ReadAllTextAsync(templatePath);
                body = body.Replace("FirstName", FirstName);
                await SendEmailAsync(RecipientEmail, subject, body); 
                _logger.LogInformation("{1} Email sent successfully", Status);
            }
            else
            {
                throw new FileNotFoundException($"Template file for status '{Status}' not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new Exception(ex.Message);
        }
    }

    public bool IsValidEmail(string email)
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
    #endregion
}

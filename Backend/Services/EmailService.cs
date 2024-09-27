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
    public async Task SendAdminVerificationConfirmationEmailAsync(ConfirmationReq request)
    {
        try
        {
            string templatePath = GetTemplatePath(request.status);
            if (!string.IsNullOrEmpty(templatePath))
            {
                string body = await File.ReadAllTextAsync(templatePath);
                await SendEmailAsync(request.toEmail,"Your verification by Admin", body); 
                Console.WriteLine("Admin Verfication Email sent successfully");
            }
            else
            {
                throw new FileNotFoundException($"Template file for status '{request.status}' not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception occurred: {ex.Message} | StackTrace: {ex.StackTrace}");
            throw new Exception(ex.Message);
        }
    }
    private string GetTemplatePath(string status)
    {
        string templatePath = status switch
            {
                "Verified" => Environment.GetEnvironmentVariable("TemplateSettings__SuccessTemplate") ?? _templateSettings.SuccessTemplate,
                "Verification Failed" => Environment.GetEnvironmentVariable("TemplateSettings__FailedTemplate") ?? _templateSettings.FailedTemplate,
                _ => null
            };
                
        if (string.IsNullOrEmpty(templatePath))
        {
            return null;
        }
        
        return File.Exists(templatePath) ? templatePath : null;
    }
    public bool IsValidEmail(string email)
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
    #endregion
}

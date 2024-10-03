using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UGH.Contracts.Confirmation;
using MimeKit;
using MailKit.Net.Smtp;

namespace UGH.Infrastructure.Services
{
    public class AdminVerificationMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly TemplateSettings _templateSettings;
        private readonly ILogger<AdminVerificationMailService> _logger;

        public AdminVerificationMailService(
            IOptions<MailSettings> mailSettings,
            IOptions<TemplateSettings> templateSettings,
            ILogger<AdminVerificationMailService> logger
        )
        {
            _mailSettings = mailSettings.Value;
            _templateSettings = templateSettings.Value;
            _logger = logger;
        }

        #region user-verification-by-admin
        public async Task SendConfirmationEmailAsync(ConfirmationRequest request)
        {
            try
            {
                string templatePath = GetTemplatePath(request.status);
                if (!string.IsNullOrEmpty(templatePath))
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(
                        new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail)
                    );
                    emailMessage.To.Add(new MailboxAddress("", request.toEmail));
                    emailMessage.Subject = "Your verification by Admin";
                    string body = await File.ReadAllTextAsync(templatePath);
                    emailMessage.Body = new TextPart("html") { Text = body };

                    using (var client = new SmtpClient())
                    {
                        await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
                        await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                        await client.SendAsync(emailMessage);
                        await client.DisconnectAsync(true);
                    }
                    _logger.LogInformation("Email sent successfully");
                }
                else
                {
                    throw new FileNotFoundException(
                        $"Template file for status '{request.status}' not found."
                    );
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
            string templateFileName = status switch
            {
                "Verified" => Environment.GetEnvironmentVariable("TemplateSettings__SuccessTemplate") ?? _templateSettings.SuccessTemplate,
                "Verification Failed" => Environment.GetEnvironmentVariable("TemplateSettings__FailedTemplate") ?? _templateSettings.FailedTemplate,
                _ => null
            };

            //string templateFileName = status switch
            //{
            //    "Verified" => "Template/confirmation.html",
            //    "Verification Failed" => "Template/DeclinedConfirmation.html",
            //    _ => null
            //};

            if (string.IsNullOrEmpty(templateFileName))
            {
                return null;
            }

            // Build a relative path from the application root
            string rootPath = Directory.GetCurrentDirectory(); // Get the root directory of the app
            string templatePath = Path.Combine(rootPath, templateFileName); // Combine root and relative template path

            // Ensure the file exists
            return File.Exists(templatePath) ? templatePath : null;
        }
        #endregion

        public class TemplateSettings
        {
            public string SuccessTemplate { get; set; }
            public string FailedTemplate { get; set; }
        }
    }
}

using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using UGHApi.Models;
using System.IO;

namespace UGHApi.Services
{
    public class AdminVerificationMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly TemplateSettings _templateSettings;

        public AdminVerificationMailService(
            IOptions<MailSettings> mailSettings,
            IOptions<TemplateSettings> templateSettings)
        {
            _mailSettings = mailSettings.Value;
            _templateSettings = templateSettings.Value;
        }
        #region user-verification-by-admin
        public async Task SendConfirmationEmailAsync(ConfirmationReq request)
        {
            try
            {
                string templatePath = GetTemplatePath(request.status);
                if (!string.IsNullOrEmpty(templatePath))
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
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
                    Console.WriteLine("Email sent successfully");
                }
                else
                {
                    throw new FileNotFoundException($"Template file for status '{request.status}' not found.");
                }
            }
            catch (Exception ex)
            {
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
        #endregion
    }
}
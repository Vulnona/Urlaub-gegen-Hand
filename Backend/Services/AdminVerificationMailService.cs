using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using UGHApi.Models;

namespace UGHApi.Services
{
    public class AdminVerificationMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IConfiguration _configuration;

        public AdminVerificationMailService(IOptions<MailSettings> mailSettings, IConfiguration configuration)
        {
            _mailSettings = mailSettings.Value;
            _configuration = configuration;
        }

        public async Task SendConfirmationEmailAsync(ConfirmationReq request)
        {
            try
            {
                string templatePath = GetTemplatePath(request.status);

                if (templatePath != null && File.Exists(templatePath))
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
                    emailMessage.To.Add(new MailboxAddress("", request.toEmail));
                    emailMessage.Subject = "Welcome";

                    string body = File.ReadAllText(templatePath);

                    emailMessage.Body = new TextPart("html") { Text = body };

                    using (var client = new SmtpClient())
                    {
                        client.Connect(_mailSettings.Host, _mailSettings.Port, false);
                        client.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                        client.Send(emailMessage);
                        client.Disconnect(true);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Template file '{templatePath}' not found.");
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

        private string GetTemplatePath(string status)
        {
            string templateDirectory = _configuration["EmailTemplatesDirectory"];
            string templateFileName = "";

            switch (status)
            {
                case "verified":
                    templateFileName = "Confirmation.html";
                    break;
                case "verification failed":
                    templateFileName = "DeclinedConfirmation.html";
                    break;
                default:
                    return null;
            }

            return Path.Combine(templateDirectory, templateFileName);
        }
    }
}

using MimeKit;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmail(string recipientEmail, string subject, string body)
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
                client.Connect(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), false);
                client.Authenticate(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
          
            Console.WriteLine($"Email sending failed: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            throw; 
        }
    }

    public bool SendVerificationEmail(string email, Guid verificationToken)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configuration["MailSettings:DisplayName"], _configuration["MailSettings:Mail"]));
            message.To.Add(new MailboxAddress(email, email)); 
            message.Subject = "Verify your email address";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Please <a href='{_configuration["BaseUrl"]}/api/auth/verify-email?token={verificationToken}'>click here</a> to verify your email address.</p>";
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(_configuration["MailSettings:Host"], Convert.ToInt32(_configuration["MailSettings:Port"]), false);
                client.Authenticate(_configuration["MailSettings:Mail"], _configuration["MailSettings:Password"]);
                client.Send(message);
                client.Disconnect(true);
            }

            return true; 
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"Email sending failed: {ex.Message}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
            }
            return false; 
        }
    }

    public bool IsValidEmail(string email)
    {
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
}

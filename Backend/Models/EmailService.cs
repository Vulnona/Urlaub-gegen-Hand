using MailKit.Net.Smtp;
using MimeKit;
using System.Text.RegularExpressions;
using UGHModels;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void SendEmail(string recipientEmail, string subject, string body)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Your App Name", "your-email@example.com"));
        emailMessage.To.Add(new MailboxAddress("", recipientEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = body };

        using (var client = new SmtpClient())
        {
            client.Connect("smtp.example.com", 587, false);
            client.Authenticate("your-email@example.com", "your-password");
            client.Send(emailMessage);
            client.Disconnect(true);
        }
    }

    public bool SendVerificationEmail(string email, Guid verificationToken)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Your Name", "fullstackdeveloper44@gmail.com"));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = "Verify your email address";
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Please <a href='{_configuration["BaseUrl"]}/api/auth/verifyemail?token={verificationToken}'>click here</a> to verify your email address.</p>";
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("fullstackdeveloper44@gmail.com", "htaj aymk rbpx iirc");
                client.Send(message);
                client.Disconnect(true);
            }

            return true; // Email sent successfully
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return false; // Email sending failed
        }
    }
    public bool IsValidEmail(string email)
    {
        // Simple email validation using regex
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, emailPattern);
    }
}

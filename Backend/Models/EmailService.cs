using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
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
}

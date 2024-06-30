using MimeKit;
using System.Net.Mail;

namespace Sofra.Api.Services.EmailServices
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your App Name", "your-email@example.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                //client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                //client.Authenticate("yourEmail@gmail.com", "yourGeneratedPassword");
                //client.Send(message);
                //client.Disconnect(true);
            }
        }
    }
}

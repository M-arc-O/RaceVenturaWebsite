using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RaceVentura.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using SmtpClient client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = _configuration.GetValue<string>("SmtpServer"),
                Port = _configuration.GetValue<int>("SmtpPort"),
                Credentials = new NetworkCredential(_configuration.GetValue<string>("GmailUserName"), _configuration.GetValue<string>("GmailPassword"))
            };

            MailAddress from = new MailAddress("race.ventura.info@gmail.com", "Race Ventura", System.Text.Encoding.UTF8);
            MailAddress to = new MailAddress(email);

            using MailMessage message = new MailMessage(from, to)
            {
                Body = htmlMessage,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                Subject = subject,
                SubjectEncoding = System.Text.Encoding.UTF8
            };

            await client.SendMailAsync(message);
        }
    }
}

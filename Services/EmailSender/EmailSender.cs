using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace FClub.Backend.Common.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;

        public EmailSender(EmailSenderOptions options)
        {
            _options = options;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_options.ServiceMail, _options.MailPassword)
            };

            return client.SendMailAsync(
                new MailMessage(
                    from: _options.ServiceMail,
                    to: email,
                    subject,
                    htmlMessage));
        }
    }
}

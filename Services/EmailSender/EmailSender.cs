using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;
using System.Net;

namespace FClub.Backend.Common.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSenderOptions _options;

        public EmailSender(EmailSenderOptions options)
        {
            _options = options;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                var message = new MimeMessage();
                Console.WriteLine(_options.ServiceMail);
                Console.WriteLine(email);
                message.From.Add(MailboxAddress.Parse(_options.ServiceMail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

                smtp.Timeout = 30000;

                await smtp.ConnectAsync(
                    _options.SmtpHost,
                    _options.SmtpPort,
                    SecureSocketOptions.SslOnConnect);

                await smtp.AuthenticateAsync(
                    new NetworkCredential(_options.ServiceMail, _options.MailPassword));

                await smtp.SendAsync(message);
            }
            catch (AuthenticationException ex)
            {
                throw new InvalidOperationException("Failed to authenticate with SMTP server", ex);
            }
            catch (SmtpCommandException ex)
            {
                throw;
            }
            catch (SmtpProtocolException ex)
            {
                throw;
            }
            finally
            {
                if (smtp != null)
                {
                    if (smtp.IsConnected)
                    {
                        await smtp.DisconnectAsync(true);
                    }
                    smtp.Dispose();
                }
            }
        }
    }
}

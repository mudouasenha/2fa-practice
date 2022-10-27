using Doodle.Services.EmailSender.Abstractions;
using Doodle.Services.EmailSender.Models;
using Doodle.Services.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Doodle.Services.EmailSender
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IOptions<EmailSenderOptions> _emailSenderOptions;

        public EmailSenderService(IOptions<EmailSenderOptions> emailSenderOptions) => _emailSenderOptions = emailSenderOptions;

        public async Task SendEmailAsync(IEnumerable<string> destinataries, string subject, string htmlMessage)
        {
            var emailMessage = new EmailMessage(destinataries, subject, htmlMessage);

            var email = CreateEmail(emailMessage);
            await Send(email);
        }

        private MimeMessage CreateEmail(EmailMessage emailMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSenderOptions.Value.FromName, _emailSenderOptions.Value.From));
            email.To.AddRange(emailMessage.Destinataries);
            email.Subject = emailMessage.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = emailMessage.Content
            };

            return email;
        }

        private async Task Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailSenderOptions.Value.SmtpServer, _emailSenderOptions.Value.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailSenderOptions.Value.From, _emailSenderOptions.Value.Password);
                await client.SendAsync(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
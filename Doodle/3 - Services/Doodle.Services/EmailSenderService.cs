using Doodle.Services.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Doodle.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IOptions<EmailSenderOptions> _emailSenderOptions;

        public EmailSenderService(IOptions<EmailSenderOptions> emailSenderOptions)
        {
            _emailSenderOptions = emailSenderOptions;
        }

        public Task SendEmailAsync(IEnumerable<string> destinataries, string subject, string htmlMessage)
        {
            var message = new MailMessage(destinataries, subject, htmlMessage);

            var mailMessage = CreateEmailBody(message);
            throw new NotImplementedException();
        }

        private MimeMessage CreateEmailBody(MailMessage mailMessage)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailSenderOptions.Value.FromName, _emailSenderOptions.Value.From));
            mimeMessage.To.AddRange(mailMessage.Destinataries);
            mimeMessage.Subject = mailMessage.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = mailMessage.Content
            };

            return mimeMessage;
        }

        private async void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailSenderOptions.Value.SmtpServer, _emailSenderOptions.Value.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailSenderOptions.Value.From, _emailSenderOptions.Value.Password);
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

    public interface IEmailSenderService
    {
        Task SendEmailAsync(IEnumerable<string> destinataries, string subject, string htmlMessage)
    };

    public class MailMessage
    {
        public MailMessage(IEnumerable<string> destinataries, string subject, string content)
        {
            Destinataries = new List<MailboxAddress>();
            Destinataries.AddRange(destinataries.Select(p => new MailboxAddress(name: p, address: p)));
            Subject = subject;
            Content = content;
        }

        public List<MailboxAddress> Destinataries { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }
    }
}
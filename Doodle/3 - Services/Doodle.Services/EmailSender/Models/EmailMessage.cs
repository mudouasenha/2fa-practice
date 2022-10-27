using MimeKit;

namespace Doodle.Services.EmailSender.Models
{
    public class EmailMessage
    {
        public EmailMessage(IEnumerable<string> destinataries, string subject, string content)
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
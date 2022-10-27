namespace Doodle.Services.EmailSender.Abstractions
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(IEnumerable<string> destinataries, string subject, string htmlMessage);
    };
}
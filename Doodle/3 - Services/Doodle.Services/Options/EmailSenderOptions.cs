namespace Doodle.Services.Options
{
    public class EmailSenderOptions
    {
        public string FromName { get; set; }
        public string From { get; set; }

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }
    }
}
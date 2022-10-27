namespace Doodle.Services.Users.Models
{
    public class UserVerifyInput
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Code { get; set; }

        public string MfaExternalId { get; set; }
    }
}
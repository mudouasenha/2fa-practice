namespace Doodle.Services.Users.Models
{
    public class UserSignInInput
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string TotpCode { get; set; }
    }
}
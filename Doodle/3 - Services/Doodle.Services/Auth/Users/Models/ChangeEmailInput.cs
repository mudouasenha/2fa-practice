namespace Doodle.Services.Users.Models
{
    public class ChangeEmailInput
    {
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }
    }
}
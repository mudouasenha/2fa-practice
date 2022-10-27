namespace Doodle.Services.Users.Models
{
    public class AccountActivationInput
    {
        public string UserId { get; set; }

        public string Code { get; set; }
    }
}
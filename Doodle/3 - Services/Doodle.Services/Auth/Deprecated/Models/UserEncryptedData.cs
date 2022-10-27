namespace Doodle.Services.Security.Models
{
    public class UserEncryptedData
    {
        public string EncryptedUsername { get; set; }

        public string EncryptedPassword { get; set; }

        public string Salt { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Doodle.Services.Users.Models
{
    public class PasswordResetInput
    {
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
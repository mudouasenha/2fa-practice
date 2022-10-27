using Doodle.Services.Users.Models;

namespace Doodle.Api.Auth.Controllers.Auth.Models
{
    public class UserVerifyInputModel
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Code { get; set; }

        public static UserVerifyInput ToInput(UserVerifyInputModel model) => new()
        {
            Username = model.Username,
            Password = model.Password,
            Code = model.Code
        };
    }
}
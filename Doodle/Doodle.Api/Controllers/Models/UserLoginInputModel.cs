using Doodle.Services.Users.Models;

namespace Doodle.Api.Controllers.Models
{
    public class UserSignInInputModel : UserSignInInput
    {
        public static UserSignInInput ToInput(UserSignInInputModel inputModel) => new()
        {
            Username = inputModel.Username,
            Password = inputModel.Password
        };
    }
}
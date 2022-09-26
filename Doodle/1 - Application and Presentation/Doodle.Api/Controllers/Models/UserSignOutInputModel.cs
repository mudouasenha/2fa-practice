using Doodle.Services.Users.Models;

namespace Doodle.Api.Controllers.Models
{
    public class UserSignOutInputModel : UserSignOutInput
    {
        public static UserSignOutInput ToInput(UserSignOutInputModel inputModel) => new()
        {
            Username = inputModel.Username,
            Password = inputModel.Password
        };
    }
}
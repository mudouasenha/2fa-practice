using Doodle.Services.Users.Models;

namespace Doodle.Api.Auth.Controllers.Auth.Models
{
    public class UserSignInInputModel : UserSignInInput
    {
        public static UserSignInInput ToInput(UserSignInInputModel inputModel) => new()
        {
            Username = inputModel.Username,
            Password = inputModel.Password,
            TotpCode = inputModel.TotpCode
        };
    }
}
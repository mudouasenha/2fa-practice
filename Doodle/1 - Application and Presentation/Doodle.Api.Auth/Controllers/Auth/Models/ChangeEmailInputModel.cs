using Doodle.Services.Users.Models;
using Doodle.Api.Auth.Controllers.Auth.Models;

namespace Doodle.Api.Auth.Controllers.Auth.Models
{
    public class ChangeEmailInputModel : ChangeEmailInput
    {
        public static ChangeEmailInput ToInput(ChangeEmailInputModel inputModel) => new()
        {
            UserId = inputModel.UserId,
            Email = inputModel.Email,
            Code = inputModel.Code
        };
    }
}

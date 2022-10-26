using Doodle.Services.Users.Models;

namespace Doodle.Api.Auth.Controllers.Models
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
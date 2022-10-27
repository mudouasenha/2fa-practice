using Doodle.Services.Users.Models;

namespace Doodle.Api.Auth.Controllers.Auth.Models
{
    public class AccountActivationInputModel : AccountActivationInput
    {
        public static AccountActivationInput ToInput(AccountActivationInputModel inputModel) => new()
        {
            UserId = inputModel.UserId,
            Code = inputModel.Code
        };
    }
}
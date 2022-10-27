using Doodle.Services.Users.Models;

namespace Doodle.Api.Auth.Controllers.Auth.Models
{
    public class UserRegisterInputModel : UserRegisterInput
    {
        public static UserRegisterInput ToInput(UserRegisterInputModel inputModel) => new()
        {
            Name = inputModel.Name,
            Username = inputModel.Username,
            Address = inputModel.Address,
            Email = inputModel.Email,
            Password = inputModel.Password,
            ConfirmPassword = inputModel.ConfirmPassword,
            PhoneNumber = inputModel.PhoneNumber
        };
    }
}
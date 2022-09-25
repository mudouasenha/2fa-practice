using Doodle.Api.Services.Users.Models;

namespace Doodle.Api.Controllers.Models
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
            PhoneNumber = inputModel.PhoneNumber
        };
    }
}
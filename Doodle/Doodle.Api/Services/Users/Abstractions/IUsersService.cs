using Doodle.Api.Services.Common;
using Doodle.Api.Services.Users.Models;
using Doodle.Domain.Entities;

namespace Doodle.Api.Services.Users.Abstractions
{
    public interface IUsersService
    {
        Task<Result<User>> Register(UserRegisterInput input);

        Task<Result<User>> SignIn(UserSignInInput input);

        Task<Result<User>> SignOut(UserSignOutInput input);

        Task<Result<User>> DeleteUser(UserFilterDTO input);

        Task<Result<User>> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword);
    }
}
using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Auth.Deprecated.Abstractions
{
    public interface IUsersService
    {
        Task<Result<ApplicationUser>> Register(UserRegisterInput input);

        Task<Result<ApplicationUser>> SignIn(UserSignInInput input);

        Task<Result<ApplicationUser>> SignOut(UserSignOutInput input);

        Task<ApplicationUser> VerifyUser(UserVerifyInput input);

        Task<ApplicationUser> UnverifyUser(string username, string password);

        Task<ApplicationUser> UpdateMfa(UserVerifyInput input);

        Task<ApplicationUser> GetByCredentials(string username, string password);

        Task<Result<ApplicationUser>> DeleteUser(UserFilterDTO input);

        Task<Result<ApplicationUser>> UpdatePassword(UserFilterDTO input, string currentPassWord, string newPassword);
    }
}
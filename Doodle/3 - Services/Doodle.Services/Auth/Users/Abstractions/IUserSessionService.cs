using Doodle.Services.Common;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Users
{
    public interface IUserSessionService
    {
        public Task<Result<Token>> SignIn(UserSignInInput input);

        public Task<Result<bool>> SignOut(UserSignOutInput input);

        public Task<Result<bool>> ForgotPassword(string email);

        public Task<Result<bool>> ResetPassword(PasswordResetInput input);

        public Task<Result<bool>> ChangeEmail(ChangeEmailInput input);
    }
}
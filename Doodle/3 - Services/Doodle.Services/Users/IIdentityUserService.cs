using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Users
{
    public interface IIdentityUserService
    {
        public Task<Result<User>> Register(UserRegisterInput input);

        public Task<Result<Token>> SignIn(UserSignInInput input);

        public Task<Result<bool>> SignOut(UserSignOutInput input);

        public Task<Result<bool>> ForgotPassword(string email);

        public Task<Result<bool>> ResetPassword(PasswordResetInput input);

        public Task<Result<bool>> ActivateAccount(AccountActivationInput input);

        public Task<Result<bool>> ChangeEmail(ChangeEmailInput input);
    }
}
using Doodle.Domain.Entities;
using Doodle.Services.Common;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Auth.Users.Abstractions
{
    public interface IUserRegistrationService
    {
        public Task<Result<User>> Register(UserRegisterInput input);

        public Task<Result<bool>> ActivateAccount(AccountActivationInput input);
    }
}
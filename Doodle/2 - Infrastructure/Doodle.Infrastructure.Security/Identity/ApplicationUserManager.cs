using Doodle.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doodle.Infrastructure.Security.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>

    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store,
        IOptions<IdentityOptions> optionsAccesor, IPasswordHasher<ApplicationUser> passwordHasher,
        IEnumerable<IUserValidator<ApplicationUser>> userValidators,
        IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer
        keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services,
        ILogger<UserManager<ApplicationUser>> logger) :
            base(store, optionsAccesor, passwordHasher, userValidators, passwordValidators,
                keyNormalizer, errors, services, logger)
        {
        }
    }

    public class CustomPasswordHasher : IPasswordHasher<ApplicationUser>
    {
        public string HashPassword(ApplicationUser user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
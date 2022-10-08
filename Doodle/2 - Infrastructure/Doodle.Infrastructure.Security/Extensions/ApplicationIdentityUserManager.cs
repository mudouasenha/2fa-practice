using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Doodle.Infrastructure.Security.Extensions
{
    public class ApplicationIdentityUserManager : UserManager<IdentityUser>

    {
        public ApplicationIdentityUserManager(IUserStore<IdentityUser> store,
        IOptions<IdentityOptions> optionsAccesor, IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators,
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer
        keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services,
        ILogger<UserManager<IdentityUser>> logger) :
            base(store, optionsAccesor, passwordHasher, userValidators, passwordValidators,
                keyNormalizer, errors, services, logger)
        {
        }
    }

    public class CustomPasswordHasher : IPasswordHasher<IdentityUser>
    {
        public string HashPassword(IdentityUser user, string password)
        {
            throw new NotImplementedException();
        }

        public PasswordVerificationResult VerifyHashedPassword(IdentityUser user, string hashedPassword, string providedPassword)
        {
            throw new NotImplementedException();
        }
    }
}
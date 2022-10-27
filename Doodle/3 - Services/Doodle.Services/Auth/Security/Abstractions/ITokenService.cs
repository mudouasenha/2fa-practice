using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Identity;

namespace Doodle.Services.Auth.Security.Abstractions
{
    public interface ITokenService
    {
        public Token GenerateToken(IdentityUser user, IEnumerable<string> roles);
    }
}
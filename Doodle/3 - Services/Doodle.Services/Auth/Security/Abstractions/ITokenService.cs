using Doodle.Domain.Entities;
using Doodle.Services.Users.Models;

namespace Doodle.Services.Auth.Security.Abstractions
{
    public interface ITokenService
    {
        public Token GenerateToken(ApplicationUser user, IEnumerable<string> roles);
    }
}
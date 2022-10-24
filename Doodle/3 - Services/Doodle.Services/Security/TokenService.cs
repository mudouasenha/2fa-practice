using Doodle.Services.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Doodle.Services.Security
{
    public class TokenService : ITokenService
    {
        public Token GenerateToken(IdentityUser user)
        {
            Claim[] userClaims = new Claim[]
            {
                new Claim("username", user.UserName),
                new Claim("id", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dfds"));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims: userClaims,
                signingCredentials: credentials,
                expires: DateTime.UtcNow.AddHours(1)
                );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            return new Token(tokenStr);
        }
    }

    public interface ITokenService
    {
        public Token GenerateToken(IdentityUser user);
    }
}
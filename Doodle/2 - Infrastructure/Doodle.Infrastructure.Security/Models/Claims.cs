using System.Security.Claims;

namespace Doodle.Infrastructure.Security.Models
{
    public static class Claims
    {
        public static Claim TwoFactorAuthenticationClaim => new("amr", "mfa");

        public static Claim AdminClaim => new("Role", "Admin");

        public static Claim ReaderClaim => new("Role", "Reader");
    }
}
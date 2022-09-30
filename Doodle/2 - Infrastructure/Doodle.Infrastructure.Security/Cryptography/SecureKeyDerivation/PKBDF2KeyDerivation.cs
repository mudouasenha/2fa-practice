using Doodle.Domain.Extensions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Doodle.Infrastructure.Security.Cryptography.SecureKeyDerivation
{
    public class PKBDF2KeyDerivation
    {
        public static string DeriveKey(string passwd, byte[] salt)
        {
            const int HmacSha256NumBytesOutput = 32;

            var derivedKey = KeyDerivation.Pbkdf2(passwd, salt, KeyDerivationPrf.HMACSHA256, 1000, HmacSha256NumBytesOutput);

            return derivedKey.AsString();
        }
    }
}
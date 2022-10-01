using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Doodle.Infrastructure.Security.Cryptography.SecureKeyDerivation
{
    public class PKBDF2KeyDerivation
    {
        private const int HmacSha256NumBytesOutput = 32;
        private const int IterationCount = 1000;

        public static bool HasMatchedDerivedKey(string passwd, byte[] salt, string derivedKeyToMatch)
        {
            var derivedKey = DeriveKey(passwd, salt);

            for (int i = 0; i < derivedKeyToMatch.Length; i++)
                if (derivedKey[i] != derivedKeyToMatch[i])
                    return false;

            return true;
        }

        public static byte[] DeriveKey(string passwd, byte[] salt)
        {
            var derivedKey = KeyDerivation.Pbkdf2(passwd, salt, KeyDerivationPrf.HMACSHA256, IterationCount, HmacSha256NumBytesOutput);

            return derivedKey;
        }
    }
}
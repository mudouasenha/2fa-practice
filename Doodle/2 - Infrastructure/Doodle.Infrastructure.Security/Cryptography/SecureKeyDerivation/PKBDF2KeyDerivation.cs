using Doodle.Domain.Extensions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Doodle.Infrastructure.Security.Cryptography.SecureKeyDerivation
{
    public class PKBDF2KeyDerivation
    {
        private const int HmacSha256NumBytesOutput = 32;
        private const int IVNumBytesOutput = 12;
        private const int IterationCount = 1000;

        public static bool HasMatchedDerivedKey(string passwd, byte[] salt, string derivedKeyToMatch, bool isIV = false)
        {
            var byteArrderivedKeyToMatch = derivedKeyToMatch.FromHexStringToByteArray();
            var derivedKey = DeriveKey(passwd, salt, isIV);

            return byteArrderivedKeyToMatch.CompareWith(derivedKey);
        }

        public static byte[] DeriveKey(string passwd, byte[] salt, bool isIV = false)
        {
            var numBytesOutput = isIV ? IVNumBytesOutput : HmacSha256NumBytesOutput;

            var derivedKey = KeyDerivation.Pbkdf2(passwd, salt, KeyDerivationPrf.HMACSHA256, IterationCount, numBytesOutput);

            return derivedKey;
        }
    }
}
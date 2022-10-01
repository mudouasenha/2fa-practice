using System.Security.Cryptography;

namespace Doodle.Infrastructure.Security.Cryptography
{
    public static class SaltGenerator
    {
        private const int DefaultSaltByteLength = 16;

        public static byte[] GenerateSalt(int outputBytes = DefaultSaltByteLength)
        {
            var salt = new byte[outputBytes];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            return salt;
        }
    }
}
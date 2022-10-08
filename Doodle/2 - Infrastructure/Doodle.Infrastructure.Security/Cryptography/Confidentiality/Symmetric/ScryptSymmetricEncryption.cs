using Doodle.Domain.Extensions;
using Replicon.Cryptography.SCrypt;

namespace Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric
{
    public class ScryptSymmetricEncryption
    {
        public static string Encrypt(string plain, byte[] salt)
        {
            var derivedKey = SCrypt.DeriveKey(plain.ToByteArray(), salt, SCrypt.Default_N, SCrypt.Default_r, SCrypt.Default_p, SCrypt.DefaultHashLengthBytes);

            return derivedKey.AsHexadecimalString();
        }

        public static bool VerifyEncrypt(string dataToCompare, byte[] salt, string hashToVerify)
        {
            var storedHash = hashToVerify.FromHexStringToByteArray();
            var computedHash = SCrypt.DeriveKey(dataToCompare.ToByteArray(), salt, SCrypt.Default_N, SCrypt.Default_r, SCrypt.Default_p, SCrypt.DefaultHashLengthBytes);

            return storedHash.CompareWith(computedHash);
        }
    }
}
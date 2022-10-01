using Doodle.Domain.Extensions;
using System.Security.Cryptography;
using System.Text;

namespace Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric
{
    public class AesGcmSymmetricEncryption
    {
        public void Main(string data)
        {
            using Aes aes = Aes.Create();
            var salt = SaltGenerator.GenerateSalt(16);

            var gcm = new AesGcm(salt);

            var cyphertext = new byte[Encoding.Default.GetByteCount(data)];
            var tag = new byte[16];
            var associatedData = new byte[16];

            var decypherText = new byte[Encoding.Default.GetByteCount(data)];

            gcm.Encrypt(aes.IV, data.ToByteArray(), cyphertext, tag, associatedData);
            gcm.Decrypt(aes.IV, cyphertext, tag, decypherText, associatedData);
        }

        public void Encrypt(string plainText, byte[] Key, byte[] IV)
        { }

        public void Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        { }
    }
}
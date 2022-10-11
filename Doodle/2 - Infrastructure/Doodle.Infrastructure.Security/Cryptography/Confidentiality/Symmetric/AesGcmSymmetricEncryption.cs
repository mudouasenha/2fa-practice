using Doodle.Domain.Extensions;
using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric
{
    // TODO: CHANGE FROM UTF-8 TO HEXADECIMAL
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

        public static string Encrypt(string plain, byte[] Key, byte[] nonce)
        {
            //IV DEVE TER 12 BYTES
            // Get bytes of plaintext string
            byte[] plainBytes = Encoding.UTF8.GetBytes(plain);

            // Get parameter sizes
            int nonceSize = AesGcm.NonceByteSizes.MaxSize;
            int tagSize = AesGcm.TagByteSizes.MaxSize;
            int cipherSize = plainBytes.Length;

            // We write everything into one big array for easier encoding
            int encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
            Span<byte> encryptedData = encryptedDataLength < 1024
                                     ? stackalloc byte[encryptedDataLength]
                                     : new byte[encryptedDataLength].AsSpan();

            // Copy parameters
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData[..4], nonceSize);
            BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);

            //var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Generate secure nonce
            //RandomNumberGenerator.Fill(nonce);

            using var aes = new AesGcm(Key);
            aes.Encrypt(nonce.AsSpan(), plainBytes.AsSpan(), cipherBytes, tag);

            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string cipher, byte[] Key, byte[] IV)
        {
            // Decode
            Span<byte> encryptedData = Convert.FromBase64String(cipher).AsSpan();

            // Extract parameter sizes
            int nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(0, 4));
            int tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
            int cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

            // Extract parameters
            var nonce = encryptedData.Slice(4, nonceSize);
            var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
            var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

            // Decrypt
            Span<byte> plainBytes = cipherSize < 1024
                                  ? stackalloc byte[cipherSize]
                                  : new byte[cipherSize];
            using var aes = new AesGcm(Key);
            aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

            // Convert plain bytes back into string
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
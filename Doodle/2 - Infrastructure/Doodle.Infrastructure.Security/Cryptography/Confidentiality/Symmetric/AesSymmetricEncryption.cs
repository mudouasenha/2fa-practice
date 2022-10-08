using System.Security.Cryptography;

namespace Doodle.Infrastructure.Security.Cryptography.Confidentiality.Symmetric
{
    // TODO: CHANGE FROM UTF-8 TO HEXADECIMAL
    public class AesSymmetricEncryption
    {
        public static void Main(string data)
        {
            using Aes myAes = Aes.Create();

            byte[] encrypted = Encrypt(data, myAes.Key, myAes.IV);

            string roundtrip = Decrypt(encrypted, myAes.Key, myAes.IV);

            Console.WriteLine("Original:   {0}", data);
            Console.WriteLine("Round Trip: {0}", roundtrip);
        }

        private static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            CheckArguments(Key, IV);

            byte[] encrypted;

            Aes aesAlg = Aes.Create();
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using StreamWriter swEncrypt = new(csEncrypt);

            //Write all data to the stream.
            swEncrypt.Write(plainText);
            encrypted = msEncrypt.ToArray();

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            CheckArguments(Key, IV);

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using MemoryStream msDecrypt = new(cipherText);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }

            return plaintext;
        }

        private static void CheckArguments(byte[] Key, byte[] IV)
        {
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
        }
    }
}
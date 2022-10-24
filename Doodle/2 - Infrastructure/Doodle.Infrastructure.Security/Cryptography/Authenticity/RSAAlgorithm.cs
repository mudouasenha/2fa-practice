using System.Security.Cryptography;
using System.Text;

namespace Doodle.Infrastructure.Security.Cryptography.Authenticity
{
    public class RSAAlgorithm
    {
        private static int numBits = 2048;

        private void Main()
        {
            var keySize = 2048;
            var rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize);

            var cipherText = Encrypt("hello world", rsaCryptoServiceProvider.ExportParameters(false));
            var plainText = Decrypt(cipherText, rsaCryptoServiceProvider.ExportParameters(true));
            Console.WriteLine(plainText);
        }

        public string Encrypt(string data, RSAParameters key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(key);
                var byteData = Encoding.UTF8.GetBytes(data);
                var encryptData = rsa.Encrypt(byteData, false);
                return Convert.ToBase64String(encryptData);
            }
        }

        public string Decrypt(string cipherText, RSAParameters key)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                var cipherByteData = Convert.FromBase64String(cipherText);
                rsa.ImportParameters(key);

                var encryptData = rsa.Decrypt(cipherByteData, false);
                return Encoding.UTF8.GetString(encryptData);
            }
        }
    }
}
using Doodle.Domain.Extensions;
using System.Security.Cryptography;

namespace Doodle.Infrastructure.Security.Cryptography.Integrity
{
    public class HMACSHA256Algorithm
    {
        public static void Main(string data)
        {
            byte[] secretkey = new byte[64];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(secretkey);

            HashData(secretkey, data.ToByteArray());

            VerifyHash(secretkey, data.ToByteArray());
        }

        public static string HashData(byte[] key, byte[] sourceFile)
        {
            using HMACSHA256 hmac = new(key);
            byte[] hashValue = hmac.ComputeHash(sourceFile);

            return hashValue.AsString();
        }

        public static bool VerifyHash(byte[] key, byte[] inputData)
        {
            bool err = false;
            using (HMACSHA256 hmac = new(key))
            {
                byte[] storedHash = new byte[hmac.HashSize / 8];

                byte[] computedHash = hmac.ComputeHash(inputData);

                for (int i = 0; i < storedHash.Length; i++)
                    if (computedHash[i] != storedHash[i])
                        err = true;
            }
            if (err)
                return false;
            else
                return true;
        }
    }
}
using Doodle.Domain.Extensions;
using System.Security.Cryptography;

namespace Doodle.Infrastructure.Security.Cryptography.Integrity
{
    public class HMACSHA512Algorithm
    {
        public static void Main(string data)
        {
            byte[] secretkey = new byte[64];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(secretkey);

            var hash = HashData(secretkey, data);

            VerifyHash(secretkey, data, hash.AsHexadecimalString());
        }

        public static byte[] HashData(byte[] key, string data)
        {
            var byteArrData = data.ToByteArray();

            using HMACSHA512 hmac = new(key);
            byte[] hashValue = hmac.ComputeHash(byteArrData);

            return hashValue;
        }

        public static bool VerifyHash(byte[] key, string data, string hashToVerify)
        {
            var byteArrData = data.ToByteArray();
            var storedHash = hashToVerify.FromHexStringToByteArray();

            HMACSHA512 hmac = new(key);
            byte[] computedHash = hmac.ComputeHash(byteArrData);

            return storedHash.CompareWith(computedHash);
        }
    }
}
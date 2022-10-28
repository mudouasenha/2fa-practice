using System.Text;

namespace Doodle.Auth.Domain.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string str) => Encoding.ASCII.GetBytes(str);

        public static byte[] FromHexStringToByteArray(this string str) => Convert.FromHexString(str);

        public static string Encode(this string input) => Convert.ToBase64String(Encoding.UTF8.GetBytes(input));

        public static string Decode(this string encondedStr) => Encoding.UTF8.GetString(Convert.FromBase64String(encondedStr));
    }
}

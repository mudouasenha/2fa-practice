using System.Text;

namespace Doodle.Domain.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string str) => Encoding.Default.GetBytes(str);
    }
}
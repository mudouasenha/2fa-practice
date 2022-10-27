using System.Text;

namespace Doodle.Domain.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string AsString(this byte[] byteArray) => Encoding.Default.GetString(byteArray);

        public static string AsHexadecimalString(this byte[] bytes) => BitConverter.ToString(bytes).Replace("-", null);

        public static bool CompareWith(this byte[] source, byte[] byteArrayToCompare)
        {
            for (int i = 0; i < source.Length; i++)
                if (byteArrayToCompare[i] != source[i])
                    return false;

            return true;
        }
    }
}
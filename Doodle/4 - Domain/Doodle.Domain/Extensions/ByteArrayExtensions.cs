using System.Text;

namespace Doodle.Domain.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string AsString(this byte[] byteArray) => Encoding.Default.GetString(byteArray);
    }
}
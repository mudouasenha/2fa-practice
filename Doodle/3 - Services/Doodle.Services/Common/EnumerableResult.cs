namespace Doodle.Services.Common
{
    public class EnumerableResult<T> : IEnumerableResult<T>, IResult<IEnumerable<T>>, IResult
    {
        public IEnumerable<T> Data { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public EnumerableResult()
        {
        }

        public EnumerableResult(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public EnumerableResult(IEnumerable<T> data, string message, bool success)
        {
            Data = data;
            Message = message;
            Success = success;
        }

        public static EnumerableResult<T> Fail(string message) => new(message, false);

        public static EnumerableResult<T> Successful(IEnumerable<T> data, string message) => new(data, message, true);
    }

    public interface IEnumerableResult<out T> : IResult<IEnumerable<T>>, IResult
    {
    }
}
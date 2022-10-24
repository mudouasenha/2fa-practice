namespace Doodle.Services.Common
{
    public class Result<T> : IResult<T>, IResult
    {
        public T Data { get; set; }

        public string Message { get; set; }

        public bool Success { get; set; }

        public Result()
        {
        }

        public Result(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public Result(T data, string message, bool success)
        {
            Data = data;
            Message = message;
            Success = success;
        }

        public static Result<T> Fail(string message) => new(message, false);

        public static Result<T> Successful(T data, string message) => new(data, message, true);
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }

    public interface IResult
    {
        string Message { get; set; }

        bool Success { get; set; }
    }
}
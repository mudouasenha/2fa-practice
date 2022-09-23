using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
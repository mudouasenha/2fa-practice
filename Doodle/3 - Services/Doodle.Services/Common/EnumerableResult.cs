using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public interface IEnumerableResult<out T> : IResult<IEnumerable<T>>, IResult
    {
    }
}
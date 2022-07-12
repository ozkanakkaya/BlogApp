using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogApp.Core.Utilities.Responses
{
    public class CustomResponse<T>
    {
        public T Data { get; set; }

        [JsonIgnore]//clientlar göremeyecek
        public int StatusCode { get; set; }

        public List<String> Errors { get; set; }


        public static CustomResponse<T> Success(int statusCode, T data)
        {
            return new CustomResponse<T> { Data = data, StatusCode = statusCode };
        }
        public static CustomResponse<T> Success(int statusCode)
        {
            return new CustomResponse<T> { StatusCode = statusCode };
        }

        public static CustomResponse<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponse<T> { StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponse<T> Fail(int statusCode, string error)
        {
            return new CustomResponse<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}

using System.Text.Json.Serialization;

namespace BlogApp.Core.Response
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }

        [JsonIgnore]//clientlar göremeyecek
        public int StatusCode { get; set; }

        public List<String> Errors { get; set; } = new List<string>();
        //public string Message { get; set; }


        public static CustomResponseDto<T> Success(int statusCode, T data)
        {
            return new CustomResponseDto<T> { Data = data, StatusCode = statusCode };
        }
        public static CustomResponseDto<T> Success(int statusCode)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode };
        }
        //public static CustomResponse<T> Success(int statusCode, T data, string message)
        //{
        //    return new CustomResponse<T> { Data = data, StatusCode = statusCode, Message = message };
        //}

        public static CustomResponseDto<T> Fail(int statusCode, List<string> errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = errors };
        }

        public static CustomResponseDto<T> Fail(int statusCode, string error)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { error } };
        }
    }
}

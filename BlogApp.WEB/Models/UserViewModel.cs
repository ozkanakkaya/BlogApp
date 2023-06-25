using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Models
{
    public class UserViewModel
    {
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
        public UserDto UserDto { get; set; }
    }
}

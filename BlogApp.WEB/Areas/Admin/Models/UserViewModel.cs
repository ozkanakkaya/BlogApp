using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class UserViewModel
    {
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
        public UserDto UserDto { get; set; }
        public IList<UserDto> UserListDto { get; set; }
    }
}

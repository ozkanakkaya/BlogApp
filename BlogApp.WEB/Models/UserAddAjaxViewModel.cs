using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Models
{
    public class UserAddAjaxViewModel
    {
        public UserViewModel UserViewModel { get; set; }
        public UserRegisterDto UserRegisterDto { get; set; }
        public string UserAddPartial { get; set; }
    }
}

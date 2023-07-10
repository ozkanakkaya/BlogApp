using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class UserUpdateAjaxViewModel
    {
        public UserUpdateDto UserUpdateDto { get; set; }
        public string UserUpdatePartial { get; set; }
        public UserViewModel UserViewModel { get; set; }
    }
}

using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.AppUserDtos
{
    public class AppUserLoginDto : IDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

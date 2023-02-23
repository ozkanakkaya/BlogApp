using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class AppRoleDto : IDto
    {
        public int Id { get; set; }

        public string Definition { get; set; }
    }
}

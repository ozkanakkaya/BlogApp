using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class AppRoleListDto : IDto
    {
        public int Id { get; set; }

        public string Definition { get; set; }
    }
}

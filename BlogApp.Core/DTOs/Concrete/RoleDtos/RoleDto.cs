using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class RoleDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

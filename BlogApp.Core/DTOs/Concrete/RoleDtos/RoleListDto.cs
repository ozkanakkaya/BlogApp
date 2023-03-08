using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class RoleListDto : IDto
    {
        public IList<RoleDto> Roles { get; set; }
    }
}

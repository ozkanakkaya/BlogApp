using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.AppUserDtos
{
    public class CheckUserResponseDto : IDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        //public List<AppRoleListDto> AppUserRoles { get; set; }
        //public bool IsExist { get; set; } = false;
    }
}

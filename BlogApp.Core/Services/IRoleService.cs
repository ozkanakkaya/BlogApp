using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IRoleService : IService<Role>
    {
        Task<CustomResponseDto<RoleListDto>> GetAllRolesAsync();
        Task<CustomResponseDto<RoleListDto>> GetAllByUserIdAsync(int userId);
        Task<CustomResponseDto<UserRoleAssignDto>> GetUserRoleAssignDtoAsync(int userId);
        Task<CustomResponseDto<UserDto>> AssignAsync(UserRoleAssignDto userRoleAssignDto);
    }
}

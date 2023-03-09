using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IRoleService : IService<Role>
    {
        Task<CustomResponse<RoleListDto>> GetAllRolesAsync();
        Task<CustomResponse<RoleListDto>> GetAllByUserIdAsync(int userId);
        Task<CustomResponse<UserRoleAssignDto>> GetUserRoleAssignDtoAsync(int userId);
        Task<CustomResponse<UserRoleAssignDto>> AssignAsync(UserRoleAssignDto userRoleAssignDto);
    }
}

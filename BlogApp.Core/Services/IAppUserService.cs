using BlogApp.Core.DTOs.Concrete.AppRoleDtos;
using BlogApp.Core.DTOs.Concrete.AppUserDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IAppUserService : IService<AppUser>
    {
        Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId);

        CustomResponse<CheckUserResponseDto> CheckUser(AppUserLoginDto dto);

        CustomResponse<List<AppRoleListDto>> GetRolesByUserId(int userId);
    }
}

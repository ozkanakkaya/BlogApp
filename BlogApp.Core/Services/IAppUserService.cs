using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IAppUserService : IService<AppUser>
    {
        Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId);

        CustomResponse<CheckUserResponseDto> CheckUser(AppUserLoginDto dto);

        Task<CustomResponse<List<AppRoleDto>>> GetRolesByUserId(int userId);
        Task<CustomResponse<List<AppUserListDto>>> GetAllByActiveAsync();
        Task<CustomResponse<AppUserListDto>> GetUserByIdAsync(int userId);
        Task<CustomResponse<NoContent>> DeleteAsync(int userId);
        Task<CustomResponse<NoContent>> UndoDeleteAsync(int userId);
        Task<CustomResponse<NoContent>> HardDeleteAsync(int userId);
        Task<CustomResponse<List<AppUserListDto>>> GetAllByDeletedAsync();
        Task<CustomResponse<List<AppUserListDto>>> GetAllByInactiveAsync();
        Task<CustomResponse<NoContent>> UpdateUserAsync(AppUserUpdateDto appUserUpdateDto);
        Task<CustomResponse<NoContent>> PasswordChangeAsync(AppUserPasswordChangeDto appUserPasswordChangeDto);
        Task<CustomResponse<NoContent>> ActivateUserAsync(int userId);
    }
}

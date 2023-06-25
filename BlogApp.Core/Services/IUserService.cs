using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IUserService : IService<User>
    {
        Task<CustomResponseDto<UserDto>> RegisterWithRoleAsync(UserRegisterDto dto, int roleId);
        Task<CustomResponseDto<CheckUserResponseDto>> CheckUserAsync(UserLoginDto dto);
        Task<CustomResponseDto<List<UserListDto>>> GetAllByActiveAsync();
        Task<CustomResponseDto<List<UserListDto>>> GetAllUsersAsync();
        Task<CustomResponseDto<UserListDto>> GetUserByIdAsync(int userId);
        Task<CustomResponseDto<NoContent>> DeleteAsync(int userId);
        Task<CustomResponseDto<NoContent>> UndoDeleteAsync(int userId);
        Task<CustomResponseDto<NoContent>> HardDeleteAsync(int userId);
        Task<CustomResponseDto<List<UserListDto>>> GetAllByDeletedAsync();
        Task<CustomResponseDto<List<UserListDto>>> GetAllByInactiveAsync();
        Task<CustomResponseDto<NoContent>> UpdateUserAsync(UserUpdateDto appUserUpdateDto);
        Task<CustomResponseDto<NoContent>> PasswordChangeAsync(UserPasswordChangeDto appUserPasswordChangeDto, string userId);
        Task<CustomResponseDto<NoContent>> ActivateUserAsync(int userId);
        Task<CustomResponseDto<NoContent>> DeleteUserImageAsync(int userId);
        Task<CustomResponseDto<int>> CountTotalAsync();
        Task<CustomResponseDto<int>> CountByNonDeletedAsync();
    }
}

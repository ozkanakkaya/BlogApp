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
        Task<CustomResponseDto<UserDto>> DeleteAsync(int userId);
        Task<CustomResponseDto<UserDto>> UndoDeleteAsync(int userId);
        Task<CustomResponseDto<UserDto>> HardDeleteAsync(int userId);
        Task<CustomResponseDto<List<UserDto>>> GetAllByDeletedAsync();
        Task<CustomResponseDto<List<UserListDto>>> GetAllByInactiveAsync();
        Task<CustomResponseDto<UserDto>> UpdateUserAsync(UserUpdateDto appUserUpdateDto);
        Task<CustomResponseDto<NoContent>> PasswordChangeAsync(UserPasswordChangeDto appUserPasswordChangeDto, int userId);
        Task<CustomResponseDto<UserDto>> ActivateUserAsync(int userId);
        Task<CustomResponseDto<NoContent>> DeleteUserImageAsync(int userId);
        Task<CustomResponseDto<int>> CountTotalAsync();
        Task<CustomResponseDto<int>> CountByNonDeletedAsync();
    }
}

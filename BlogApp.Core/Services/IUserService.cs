using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IUserService : IService<User>
    {
        Task<CustomResponse<UserRegisterDto>> RegisterWithRoleAsync(UserRegisterDto dto, int roleId);
        Task<CustomResponse<CheckUserResponseDto>> CheckUserAsync(UserLoginDto dto);
        Task<CustomResponse<List<UserListDto>>> GetAllByActiveAsync();
        Task<CustomResponse<UserListDto>> GetUserByIdAsync(int userId);
        Task<CustomResponse<NoContent>> DeleteAsync(int userId);
        Task<CustomResponse<NoContent>> UndoDeleteAsync(int userId);
        Task<CustomResponse<NoContent>> HardDeleteAsync(int userId);
        Task<CustomResponse<List<UserListDto>>> GetAllByDeletedAsync();
        Task<CustomResponse<List<UserListDto>>> GetAllByInactiveAsync();
        Task<CustomResponse<NoContent>> UpdateUserAsync(UserUpdateDto appUserUpdateDto);
        Task<CustomResponse<NoContent>> PasswordChangeAsync(UserPasswordChangeDto appUserPasswordChangeDto, string userId);
        Task<CustomResponse<NoContent>> ActivateUserAsync(int userId);
        Task<CustomResponse<NoContent>> DeleteUserImageAsync(int userId);
        Task<CustomResponse<int>> CountTotalAsync();
        Task<CustomResponse<int>> CountByNonDeletedAsync();
    }
}

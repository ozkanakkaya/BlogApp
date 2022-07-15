using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IAppUserService : IService<AppUser>
    {
        Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId);
    }
}

using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Utilities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Services
{
    public interface IAppUserService :IService<AppUser>
    {
        Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId);
    }
}

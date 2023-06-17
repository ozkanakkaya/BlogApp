using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlogApp.WEB.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserApiService _userApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminMenuViewComponent(UserApiService userApiService, IHttpContextAccessor contextAccessor)
        {
            _userApiService = userApiService;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userApiService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
                return Content("Kullanıcı bulunamadı");

            return View(new UserWithRolesViewModel
            {
                UserDto = user.Data,
            });

        }
    }
}

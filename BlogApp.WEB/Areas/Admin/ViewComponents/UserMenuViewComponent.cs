using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.WEB.Areas.Admin.ViewComponents
{
    public class UserMenuViewComponent : ViewComponent
    {
        private readonly UserApiService _userApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public UserMenuViewComponent(UserApiService userApiService, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _userApiService = userApiService;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userApiService.GetUserByIdAsync(int.Parse(userId));

            if (user == null)
                return Content("Kullanıcı bulunamadı!");

            return View(new UserProfileViewModel
            {
                User = user.Data
            });

        }
    }
}



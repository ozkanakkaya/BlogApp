using BlogApp.WEB.Services;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly BlogApiService _blogApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogController(BlogApiService blogApiService, IHttpContextAccessor httpContextAccessor)
        {
            _blogApiService = blogApiService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Blog.Create")]
        public async Task<IActionResult> Index()
        {
            var httpContext = _httpContextAccessor.HttpContext.User.Claims;
            var userRoles = httpContext.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin"))
            {
                var result = await _blogApiService.GetAllAsync();

                if (!result.Errors.Any() && result.Data != null)
                {
                    ViewBag.TableTitle = "Tüm Blog Yazıları";
                    return View(result.Data);
                }

                else
                {
                    ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                    return View();
                }
            }
            else
            {
                var userId = int.Parse(httpContext.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var result = await _blogApiService.GetAllByUserIdAsync(userId);

                if (!result.Errors.Any() && result.Data != null)
                {
                    ViewBag.TableTitle = "Blog Yazılarınız";
                    return View(result.Data);
                }
                else
                {
                    ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                    return View();
                }
            }


        }
    }
}

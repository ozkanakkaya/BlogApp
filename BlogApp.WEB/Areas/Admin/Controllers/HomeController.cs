using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin, Member")]
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly BlogApiService _blogApiService;
        private readonly CategoryApiService _categoryApiService;
        private readonly CommentApiService _commentApiService;
        private readonly UserApiService _userApiService;

        public HomeController(BlogApiService blogApiService, CategoryApiService categoryApiService, CommentApiService commentApiService, UserApiService userApiService)
        {
            _blogApiService = blogApiService;
            _categoryApiService = categoryApiService;
            _commentApiService = commentApiService;
            _userApiService = userApiService;
        }
        
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Index()
        {
            var blogsCountResult = await _blogApiService.CountByNonDeletedBlogsAsync();
            var categoriesCountResult = await _categoryApiService.CountByNonDeletedAsync();
            var commentsCountResult = await _commentApiService.CountByNonDeletedAsync();
            var usersCountResult = await _userApiService.CountByNonDeletedAsync();
            var blogsResult = await _blogApiService.GetAllAsync();

            if (!blogsCountResult.Errors.Any() && !categoriesCountResult.Errors.Any() && !commentsCountResult.Errors.Any() && !usersCountResult.Errors.Any() && !blogsCountResult.Errors.Any())
            {
                return View(new DashboardViewModel
                {
                    BlogsCount = blogsCountResult.Data,
                    CategoriesCount = categoriesCountResult.Data,
                    CommentsCount = commentsCountResult.Data,
                    UsersCount = usersCountResult.Data,
                    Blogs = blogsResult.Data
                });
            }
            return NotFound();
        }
    }
}

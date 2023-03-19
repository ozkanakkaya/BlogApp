using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogApiService _blogApiService;

		public HomeController(BlogApiService blogApiService)
		{
			_blogApiService = blogApiService;
		}

        [HttpGet]
		public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false)
        {
            var blogsResult = await (categoryId == null
                ? _blogApiService.GetAllByPagingAsync(null, currentPage, pageSize, isAscending)
                : _blogApiService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending));

			return View(blogsResult);
        }

    }
}
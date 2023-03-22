using BlogApp.WEB.Configurations;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace BlogApp.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlogApiService _blogApiService;
		private readonly AboutUsPageInfo _aboutUsPageInfo;

		public HomeController(BlogApiService blogApiService, IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo)
		{
			_blogApiService = blogApiService;
			_aboutUsPageInfo = aboutUsPageInfo.Value;
		}

		[HttpGet]
		public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 6, bool isAscending = false)
        {
            var blogsResult = await (categoryId == null
                ? _blogApiService.GetAllByPagingAsync(null, currentPage, pageSize, isAscending)
                : _blogApiService.GetAllByPagingAsync(categoryId, currentPage, pageSize, isAscending));

			return View(blogsResult);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View(_aboutUsPageInfo);
        }
    }
}
using BlogApp.WEB.Configurations;
using BlogApp.WEB.Models;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlogApp.WEB.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogApiService _blogApiService;
        private readonly BlogRightSideBarWidgetOptions _blogRightSideBarWidgetOptions;

        public BlogController(BlogApiService blogApiService, IOptionsSnapshot<BlogRightSideBarWidgetOptions> blogRightSideBarWidgetOptions)
        {
            _blogApiService = blogApiService;
            _blogRightSideBarWidgetOptions = blogRightSideBarWidgetOptions.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 6, bool isAscending = false)
        {
            var searchResult = await _blogApiService.SearchAsync(keyword, currentPage, pageSize, isAscending);

            if (!searchResult.Errors.Any())
            {
                if (searchResult.Data.TotalCount <= 0)
                {
                    TempData["Message"] = "Aramanıza ait bir sonuç bulunamamıştır.";
                }
                return View(new BlogSearchViewModel
                {
                    BlogListResultDto = searchResult.Data,
                    Keyword = keyword
                });
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int blogId)
        {
            var blogResult = await _blogApiService.GetByBlogIdAsync(blogId);

            if (!blogResult.Errors.Any())
            {
                var userBlogs = await _blogApiService.GetAllByUserIdOnFilterAsync(blogResult.Data.UserId, _blogRightSideBarWidgetOptions.FilterBy, _blogRightSideBarWidgetOptions.OrderBy,
                    _blogRightSideBarWidgetOptions.IsAscending, _blogRightSideBarWidgetOptions.TakeSize, _blogRightSideBarWidgetOptions.CategoryId,
                    _blogRightSideBarWidgetOptions.StartAt, _blogRightSideBarWidgetOptions.EndAt,
                    _blogRightSideBarWidgetOptions.MinViewCount, _blogRightSideBarWidgetOptions.MaxViewCount,
                    _blogRightSideBarWidgetOptions.MinCommentCount, _blogRightSideBarWidgetOptions.MaxCommentCount
                    );

                ViewData["Id"] = blogResult.Data.Id;

                await _blogApiService.IncreaseViewCountAsync(blogId);

                return View(new BlogDetailViewModel
                {
                    BlogListDto = blogResult.Data,
                    BlogDetailRightSideBarViewModel = new BlogDetailRightSideBarViewModel
                    {
                        BlogListDto = userBlogs.Data,
                        Header = _blogRightSideBarWidgetOptions.Header,
                        User = blogResult.Data.User
                    }
                });
            }
            return NotFound();
        }
    }
}

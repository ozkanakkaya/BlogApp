using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.WEB.Models;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WEB.ViewComponents
{
    public class RightSideBarViewComponent : ViewComponent
    {
        private readonly BlogApiService _blogApiService;
        private readonly CategoryApiService _categoryApiService;
        public RightSideBarViewComponent(BlogApiService blogApiService, CategoryApiService categoryApiService)
        {
            _blogApiService = blogApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoriesResult = await _categoryApiService.GetAllByActiveAsync();
            var mostReadBlogs = await _blogApiService.GetAllByViewCountAsync(false, 5);
            var latestPosts = await _blogApiService.GetAllFilteredAsync(null, null, true, false, 1, 3, OrderByGeneral.CreatedDate, false, false, false, false, false);


            return View(new RightSideBarViewModel
            {
                MostReadBlogs = mostReadBlogs.Data,
                CategoryListDto = categoriesResult.Data,
                LatestPosts = latestPosts.Data.BlogListDto
            });
        }
    }
}

using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryApiService _categoryApiService;

        public CategoryController(CategoryApiService categoryApiService)
        {
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryApiService.GetAllByNonDeletedAsync();
            if (!result.Errors.Any() && result.Data != null)
                return View(new CategoryListDto
                {
                    Categories = result.Data.Categories
                });
            else
            {
                ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                return View();
            }
        }
    }
}

using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [Authorize(Roles = "SuperAdmin,Category.Read")]
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

        [Authorize(Roles = "SuperAdmin,Category.Create")]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [Authorize(Roles = "SuperAdmin,Category.Create")]
        [HttpPost]
        public async Task<IActionResult> Add(CategoryCreateDto categoryCreateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryApiService.AddAsync(categoryCreateDto);

                if (!result.Errors.Any())
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryViewModel = new CategoryViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            CategoryDto = result.Data,
                            Message = $"{result.Data.Name} isimli kategori başarıyla eklendi."
                        },
                        CategoryAddPartial=await this.RenderViewToStringAsync("_CategoryAddPartial", categoryCreateDto)
                    });
                    return Json(categoryAddAjaxModel);
                }
            }

            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryCreateDto)
            });

            return Json(categoryAddAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryApiService.GetAllByNonDeletedAsync();
            if (!result.Errors.Any() && result.Data != null)
            {
                var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(categories);
            }

            string errorMessages = String.Empty;
            foreach (var error in result.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            return Json(JsonSerializer.Serialize(new { error = errorMessages }));
        }

        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        [HttpPost]
        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryApiService.DeleteAsync(categoryId);

            if (!result.Errors.Any())
            {
                var deletedCategoryModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Name}' adlı kategori başarıyla silindi. \nTamamen silmek için veya geri almak için : \nÇöp Kutusu/Kategoriler menüsüne gidiniz.",
                    CategoryDto = result.Data
                });
                return Json(deletedCategoryModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var deletedCategoryErrorModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(deletedCategoryErrorModel);
            }
        }


    }
}

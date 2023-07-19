using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Enums.ComplexTypes;
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

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> Update(int categoryId)
        {
            var result = await _categoryApiService.GetCategoryUpdateDtoAsync(categoryId);

            return PartialView("_CategoryUpdatePartial", result.Data);
        }

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        [HttpPost]
        public async Task<JsonResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryApiService.UpdateAsync(categoryUpdateDto);

                if (!result.Errors.Any() && result.Data != null)
                {
                    var categoryUpdateAjaxModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        CategoryViewModel = new CategoryViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"'{result.Data.Name}' adlı kategorinin bilgileri başarılı bir şekilde güncellenmiştir.",
                            CategoryDto = result.Data
                        },
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                    });
                    return Json(categoryUpdateAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    var categoryUpdateAjaxErrorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        CategoryUpdateDto = categoryUpdateDto,
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                    });
                    return Json(categoryUpdateAjaxErrorModel);
                }
            }
            else
            {
                var categoryUpdateAjaxModelStateErrorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                {
                     CategoryUpdateDto= categoryUpdateDto,
                    CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial", categoryUpdateDto)
                });
                return Json(categoryUpdateAjaxModelStateErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> DeletedCategories()
        {
            var users = await _categoryApiService.GetAllByDeletedAsync();

            if (!users.Errors.Any())
                return View(users.Data);
            else
            {
                ViewBag.ErrorMessage = users.Errors.FirstOrDefault();
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<JsonResult> GetAllDeletedCategories()
        {
            var result = await _categoryApiService.GetAllByDeletedAsync();
            if (result.Data != null || !result.Errors.Any())
            {
                var categoryListModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    CategoryListDto = result.Data
                });

                return Json(categoryListModel);
            }

            string errorMessages = String.Empty;
            foreach (var error in result.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            var categoryListModelError = JsonSerializer.Serialize(new CategoryViewModel
            {
                ResultStatus = ResultStatus.Error,
                Message = $"{errorMessages}\n",
            });

            return Json(categoryListModelError);
        }

        [Authorize(Roles = "SuperAdmin,Category.Update")]
        [HttpPut]
        public async Task<JsonResult> UndoDelete(int categoryId)
        {
            var result = await _categoryApiService.UndoDeleteAsync(categoryId);

            if (!result.Errors.Any())
            {
                var undoDeletedCategoryModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Name}' adlı kategori başarıyla arşivden geri alındı.",
                    CategoryDto = result.Data
                });
                return Json(undoDeletedCategoryModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undoDeletedCategoryErrorModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeletedCategoryErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        [HttpDelete]
        public async Task<JsonResult> HardDelete(int categoryId)
        {
            var result = await _categoryApiService.HardDeleteAsync(categoryId);

            if (!result.Errors.Any() && result.Data != null)
            {
                var hardDeletedCommentModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Name}' adlı kullanıcı başarıyla tamamen silindi.",
                });

                return Json(hardDeletedCommentModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undoDeletedCommentErrorModel = JsonSerializer.Serialize(new CategoryViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeletedCommentErrorModel);
            }
        }
    }
}

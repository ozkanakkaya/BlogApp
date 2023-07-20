using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly TagApiService _tagApiService;

        public TagController(TagApiService tagApiService)
        {
            _tagApiService = tagApiService;
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        public async Task<IActionResult> Index()
        {
            var result = await _tagApiService.GetAllByNonDeletedAsync();
            if (!result.Errors.Any() && result.Data != null)
                return View(new TagListDto
                {
                    Tags = result.Data.Tags
                });
            else
            {
                ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        public async Task<JsonResult> GetAllTags()
        {
            var result = await _tagApiService.GetAllByNonDeletedAsync();
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

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpPut]
        public async Task<JsonResult> Delete(int tagId)
        {
            var result = await _tagApiService.DeleteAsync(tagId);

            if (!result.Errors.Any())
            {
                var deletedTagModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Id}' nolu etiket başarıyla silindi. \nTamamen silmek için veya geri almak için : \nÇöp Kutusu/Etiketler menüsüne gidiniz.",
                    TagDto = result.Data
                });
                return Json(deletedTagModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var deletedCommentErrorModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(deletedCommentErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        public async Task<IActionResult> Update(int tagId)
        {
            var result = await _tagApiService.GetTagUpdateDtoAsync(tagId);

            if (!result.Errors.Any() && result.Data != null)
            {
                return PartialView("_TagUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        [HttpPut]
        public async Task<JsonResult> Update(TagUpdateDto tagUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _tagApiService.UpdateAsync(tagUpdateDto);

                if (!result.Errors.Any() && result.Data != null)
                {
                    var tagUpdateAjaxModel = JsonSerializer.Serialize(new TagUpdateAjaxViewModel
                    {
                        TagViewModel = new TagViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"'{result.Data.Id}' nolu etiket başarılı bir şekilde güncellenmiştir.",
                            TagDto = result.Data
                        },
                        TagUpdatePartial = await this.RenderViewToStringAsync("_TagUpdatePartial", tagUpdateDto)
                    });
                    return Json(tagUpdateAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            var tagUpdateAjaxErrorModel = JsonSerializer.Serialize(new TagUpdateAjaxViewModel
            {
                TagUpdateDto = tagUpdateDto,
                TagUpdatePartial = await this.RenderViewToStringAsync("_TagUpdatePartial", tagUpdateDto)
            });
            return Json(tagUpdateAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        public async Task<IActionResult> DeletedTags()
        {
            var result = await _tagApiService.GetAllByDeletedAsync();

            if (!result.Errors.Any())
                return View(result.Data);
            else
            {
                ViewBag.ErrorMessage = result.Errors.FirstOrDefault();
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,Tag.Read")]
        public async Task<JsonResult> GetAllDeletedTags()
        {
            var result = await _tagApiService.GetAllByDeletedAsync();
            if (result.Data != null || !result.Errors.Any())
            {
                var tagListModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    TagListDto = result.Data
                });

                return Json(tagListModel);
            }

            string errorMessages = String.Empty;
            foreach (var error in result.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            var tagListModelError = JsonSerializer.Serialize(new TagViewModel
            {
                ResultStatus = ResultStatus.Error,
                Message = $"{errorMessages}\n",
            });

            return Json(tagListModelError);
        }

        [Authorize(Roles = "SuperAdmin,Tag.Update")]
        [HttpPut]
        public async Task<JsonResult> UndoDelete(int tagId)
        {
            var result = await _tagApiService.UndoDeleteAsync(tagId);

            if (!result.Errors.Any())
            {
                var undoDeletedTagModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Id}' nolu etiket başarıyla arşivden geri alındı.",
                    TagDto = result.Data
                });
                return Json(undoDeletedTagModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undoDeletedTagErrorModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeletedTagErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Tag.Delete")]
        [HttpDelete]
        public async Task<JsonResult> HardDelete(int tagId)
        {
            var result = await _tagApiService.HardDeleteAsync(tagId);

            if (!result.Errors.Any())
            {
                var hardDeletedTagModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{tagId}' nolu etiket başarıyla kalıcı olarak silindi.",
                });

                return Json(hardDeletedTagModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undoDeletedTagErrorModel = JsonSerializer.Serialize(new TagViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeletedTagErrorModel);
            }
        }
    }
}

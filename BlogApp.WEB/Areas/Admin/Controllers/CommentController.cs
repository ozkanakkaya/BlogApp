using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlogApp.Core.Enums;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Utilities.Extensions;
using System.Security.Claims;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly CommentApiService _commentApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentController(CommentApiService categoryApiService, IHttpContextAccessor httpContextAccessor)
        {
            _commentApiService = categoryApiService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Comment.Read")]
        public async Task<IActionResult> Index()
        {
            var httpContext = _httpContextAccessor.HttpContext.User.Claims;
            var userRoles = httpContext.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var isAdmin = userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin");

            var result = isAdmin
                ? await _commentApiService.GetAllAsync()
                : await GetCommentsByUserIdAsync(httpContext);

            if (!result.Errors.Any() && result.Data != null)
            {
                ViewBag.TableTitle = isAdmin ? "Tüm Kullanıcıların Yorumları" : "Yorumlarınız";
                return View(new CommentListDto
                {
                    Comments = result.Data.Comments
                });
            }

            ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
            return View();
        }

        private async Task<CustomResponseDto<CommentListDto>> GetCommentsByUserIdAsync(IEnumerable<Claim> claims)
        {
            var userId = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return await _commentApiService.GetAllCommentsByUserIdAsync(userId);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Comment.Read")]
        public async Task<JsonResult> GetAllComments()
        {
            var httpContext = _httpContextAccessor.HttpContext.User.Claims;
            var userRoles = httpContext.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var result = (userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin")) 
                ? await _commentApiService.GetAllAsync() 
                : await GetCommentsByUserIdAsync(httpContext);

            if (!result.Errors.Any() && result.Data != null)
            {
                var comments = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(comments);
            }

            string errorMessages = String.Empty;
            foreach (var error in result.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            return Json(JsonSerializer.Serialize(new { error = errorMessages }));
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        public async Task<IActionResult> GetDetail(int commentId)
        {
            var result = await _commentApiService.GetDetailAsync(commentId);

            if (!result.Errors.Any() && result.Data != null)
            {
                return PartialView("_CommentDetailPartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Delete")]
        [HttpPut]
        public async Task<JsonResult> Delete(int commentId)
        {
            var result = await _commentApiService.DeleteAsync(commentId);

            if (!result.Errors.Any())
            {
                var deletedCommentModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Id}' nolu yorum başarıyla silindi. \nTamamen silmek için veya geri almak için : \nÇöp Kutusu/Yorumlar menüsüne gidiniz.",
                    CommentDto = result.Data
                });
                return Json(deletedCommentModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var deletedCommentErrorModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(deletedCommentErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Delete")]
        [HttpPut]
        public async Task<JsonResult> Approve(int commentId)
        {
            var result = await _commentApiService.ApproveAsync(commentId);

            if (!result.Errors.Any())
            {
                var approvedCommentModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Id}' nolu yorum başarıyla onaylandı.",
                    CommentDto = result.Data
                });
                return Json(approvedCommentModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var approvedCommentErrorModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(approvedCommentErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        public async Task<IActionResult> Update(int commentId)
        {
            var result = await _commentApiService.GetCommentUpdateDtoAsync(commentId);

            if (!result.Errors.Any() && result.Data != null)
            {
                return PartialView("_CommentUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpPut]
        public async Task<JsonResult> Update(CommentUpdateDto commentUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentApiService.UpdateAsync(commentUpdateDto);

                if (!result.Errors.Any() && result.Data != null)
                {
                    var commentUpdateAjaxModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
                    {
                        CommentViewModel = new CommentViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"'{result.Data.Id}' nolu yorum başarılı bir şekilde güncellenmiştir.",
                            CommentDto = result.Data
                        },
                        CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
                    });
                    return Json(commentUpdateAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            var commentUpdateAjaxErrorModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
            {
                CommentUpdateDto = commentUpdateDto,
                CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
            });
            return Json(commentUpdateAjaxErrorModel);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        public async Task<IActionResult> DeletedComments()
        {
            var result = await _commentApiService.GetAllByDeletedAsync();

            if (!result.Errors.Any())
                return View(result.Data);
            else
            {
                ViewBag.ErrorMessage = result.Errors.FirstOrDefault();
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Read")]
        public async Task<JsonResult> GetAllDeletedComments()
        {
            var result = await _commentApiService.GetAllByDeletedAsync();
            if (result.Data != null || !result.Errors.Any())
            {
                var commentListModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    CommentListDto = result.Data
                });

                return Json(commentListModel);
            }

            string errorMessages = String.Empty;
            foreach (var error in result.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            var commentListModelError = JsonSerializer.Serialize(new CommentViewModel
            {
                ResultStatus = ResultStatus.Error,
                Message = $"{errorMessages}\n",
            });

            return Json(commentListModelError);
        }

        [Authorize(Roles = "SuperAdmin,Comment.Update")]
        [HttpPut]
        public async Task<JsonResult> UndoDelete(int commentId)
        {
            var result = await _commentApiService.UndoDeleteAsync(commentId);

            if (!result.Errors.Any())
            {
                var undoDeletedCommentModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Id}' nolu yorum başarıyla arşivden geri alındı.",
                    CommentDto = result.Data
                });
                return Json(undoDeletedCommentModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undoDeletedCommentErrorModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeletedCommentErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Comment.Delete")]
        [HttpDelete]
        public async Task<JsonResult> HardDelete(int commentId)
        {
            var result = await _commentApiService.HardDeleteAsync(commentId);

            if (!result.Errors.Any())
            {
                var hardDeletedCommentModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{commentId}' nolu yorum başarıyla kalıcı olarak silindi.",
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

                var undoDeletedCommentErrorModel = JsonSerializer.Serialize(new CommentViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeletedCommentErrorModel);
            }
        }
    }
}

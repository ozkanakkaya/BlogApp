using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlogApp.WEB.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentApiService _commentApiService;

        public CommentController(CommentApiService commentApiService)
        {
            _commentApiService = commentApiService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CommentCreateDto commentCreateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentApiService.AddAsync(commentCreateDto);
                if (!result.Errors.Any())
                {
                    var commentCreateAjaxViewModel = JsonSerializer.Serialize(new CommentCreateAjaxViewModel
                    {
                        CommentDto = result.Data,
                        CommentCreatePartial = await this.RenderViewToStringAsync("_CommentCreatePartial", commentCreateDto)

                    }, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                    return Json(commentCreateAjaxViewModel);
                }
                ModelState.AddModelError("", result.Errors.FirstOrDefault());
            }

            var commentCreateAjaxErrorModel = JsonSerializer.Serialize(new CommentCreateAjaxViewModel
            {
                CommentCreateDto = commentCreateDto,
                CommentCreatePartial = await this.RenderViewToStringAsync("_CommentCreatePartial", commentCreateDto)
            });
            return Json(commentCreateAjaxErrorModel);
        }
    }
}

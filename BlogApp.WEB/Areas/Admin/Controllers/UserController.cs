using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserApiService _userApiService;

        public UserController(UserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userApiService.GetAllUsersAsync();

            if (!users.Errors.Any())
                return View(users.Data);
            else
            {
                ViewBag.ErrorMessage = users.Errors.FirstOrDefault();
                return View();
            }
        }

        public async Task<PartialViewResult> GetDetail(int userId)
        {
            var user = await _userApiService.GetUserByIdAsync(userId);
            return PartialView("_GetDetailPartial", user.Data);
        }

        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userApiService.GetAllUsersAsync();
            if (users.Errors != null || !users.Errors.Any())
            {
                var userListDto = JsonSerializer.Serialize(users.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(userListDto);
            }
            ViewBag.ErrorMessage = users.Errors.FirstOrDefault();
            return Json(new { error = ViewBag.ErrorMessage });
        }


    }
}

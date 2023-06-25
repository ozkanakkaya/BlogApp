using BlogApp.Core.DTOs.Abstract;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.WEB.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserApiService _userApiService;
        private readonly IImageHelper _imageHelper;

        public UserController(UserApiService userApiService, IImageHelper imageHelper)
        {
            _userApiService = userApiService;
            _imageHelper = imageHelper;
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
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

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<PartialViewResult> GetDetail(int userId)
        {
            var user = await _userApiService.GetUserByIdAsync(userId);
            return PartialView("_GetDetailPartial", user.Data);
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
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

        [Authorize(Roles = "SuperAdmin,User.Create")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [Authorize(Roles = "SuperAdmin,User.Create")]
        [HttpPost]
        public async Task<IActionResult> Add(UserRegisterDto registerDto)
        {
            if (ModelState.IsValid)
            {
                var uploadedImageDtoResult = await _imageHelper.UploadAsync(registerDto.Username, registerDto.ImageFile, ImageType.User);
                registerDto.ImageUrl = uploadedImageDtoResult.StatusCode == 200 ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                var userDto = await _userApiService.RegisterAsync(registerDto);

                if (!userDto.Errors.Any())
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserViewModel = new UserViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"'{userDto.Data.Username}' adlı kullanıcı başarılı bir şekilde eklendi.",
                            UserDto = userDto.Data
                        },

                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", registerDto)
                    });
                    return Json(userAddAjaxModel);
                }
                else
                {
                    if (registerDto.ImageUrl != "userImages/defaultUser.png")
                        await _imageHelper.DeleteAsync(registerDto.ImageUrl);

                    foreach (var error in userDto.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserRegisterDto = registerDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", registerDto)
                    });
                    return Json(userAddAjaxErrorModel);
                }
            }
            else
            {
                var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                {
                    UserRegisterDto = registerDto,
                    UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", registerDto)
                });
                return Json(userAddAjaxModelStateErrorModel);
            }
        }

















    }
}

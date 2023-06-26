using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.WEB.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserApiService _userApiService;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;

        public UserController(UserApiService userApiService, IImageHelper imageHelper, IMapper mapper)
        {
            _userApiService = userApiService;
            _imageHelper = imageHelper;
            _mapper = mapper;
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<IActionResult> Index()
        {
            var users = await _userApiService.GetAllByActiveAsync();

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

        [Authorize(Roles = "SuperAdmin,User.Delete")]
        [HttpPost]
        public async Task<JsonResult> Delete(int userId)
        {
            var result = await _userApiService.DeleteAsync(userId);

            if (!result.Errors.Any())
            {
                var deletedUserModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Username}' adlı kullanıcı başarıyla silindi. \nTamamen silmek için veya geri almak için : \nÇöp Kutusu/Kullanıcılar menüsüne gidiniz.",
                    UserDto = result.Data
                });
                return Json(deletedUserModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var deletedUserErrorModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(deletedUserErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Update(int userId)
        {
            var user = await _userApiService.GetUserByIdAsync(userId);

            return PartialView("_UserUpdatePartial", _mapper.Map<UserUpdateDto>(user.Data));
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPost]
        public async Task<JsonResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewImageUploaded = false;

                var oldUserImage = userUpdateDto.ImageUrl;

                if (userUpdateDto.ImageFile != null)
                {
                    var uploadedImageDtoResult = await _imageHelper.UploadAsync(userUpdateDto.Username, userUpdateDto.ImageFile, ImageType.User);

                    if (uploadedImageDtoResult.StatusCode == 200)
                        userUpdateDto.ImageUrl = uploadedImageDtoResult.Data.FullName;

                    if (oldUserImage != "userImages/defaultUser.png")
                        isNewImageUploaded = true;
                }

                var result = await _userApiService.UpdateAsync(userUpdateDto);

                if (isNewImageUploaded)
                {
                    await _imageHelper.DeleteAsync(oldUserImage);
                }

                if (!result.Errors.Any())
                {
                    var userUpdateAjaxModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserViewModel = new UserViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"'{result.Data.Username}' adlı kullanıcı bilgileri başarılı bir şekilde güncellenmiştir.",
                            UserDto = result.Data
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateAjaxModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    var userUpdateAjaxErrorModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateAjaxErrorModel);
                }
            }
            else
            {
                var userUpdateAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(userUpdateAjaxModelStateErrorModel);
            }
        }










    }
}

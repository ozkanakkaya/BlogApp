using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using BlogApp.Business.Jwt;
using System.IdentityModel.Tokens.Jwt;
using BlogApp.WEB.Areas.Admin.Models;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserApiService _userApiService;
        private readonly TokenGenerator _tokenGenerator;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public UserController(UserApiService userApiService, IImageHelper imageHelper, IMapper mapper, IToastNotification toastNotification, TokenGenerator tokenGenerator)
        {
            _userApiService = userApiService;
            _imageHelper = imageHelper;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _tokenGenerator = tokenGenerator;
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
            if (!users.Errors.Any())
            {
                var userListDto = JsonSerializer.Serialize(users.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(userListDto);
            }

            string errorMessages = String.Empty;
            foreach (var error in users.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            return Json(JsonSerializer.Serialize(new { error = errorMessages }));
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<JsonResult> GetAllByActive()
        {
            var users = await _userApiService.GetAllByActiveAsync();
            if (!users.Errors.Any())
            {
                var userListDto = JsonSerializer.Serialize(users.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(userListDto);
            }

            string errorMessages = String.Empty;
            foreach (var error in users.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            return Json(JsonSerializer.Serialize(new { error = errorMessages }));
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<JsonResult> GetAllByInactive()
        {
            var users = await _userApiService.GetAllByInactiveAsync();
            if (!users.Errors.Any())
            {
                var userListDto = JsonSerializer.Serialize(users.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(userListDto);
            }

            string errorMessages = String.Empty;
            foreach (var error in users.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            return Json(JsonSerializer.Serialize(new { error = errorMessages }));
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

                if (!result.Errors.Any())
                {
                    if (isNewImageUploaded)
                    {
                        await _imageHelper.DeleteAsync(oldUserImage);
                    }

                    var userUpdateAjaxModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserViewModel = new UserViewModel
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"'{result.Data.Username}' adlı kullanıcının bilgileri başarılı bir şekilde güncellenmiştir.",
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

        [Authorize]
        public async Task<IActionResult> ProfileUpdate()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = await _userApiService.GetUserByIdAsync(int.Parse(userId));

            return View(_mapper.Map<UserUpdateDto>(user.Data));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProfileUpdate(UserUpdateDto userUpdateDto)
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

                if (!result.Errors.Any())
                {
                    if (isNewImageUploaded)
                    {
                        await _imageHelper.DeleteAsync(oldUserImage);
                    }

                    _toastNotification.AddSuccessToastMessage("Bilgileriniz başarılı bir şekilde güncellenmiştir.", new ToastrOptions
                    {
                        Title = "İşlem Başarılı"
                    });
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    return View(userUpdateDto);
                }
            }
            else
            {
                return View(userUpdateDto);
            }
        }

        [Authorize]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var result = await _userApiService.PasswordChangeAsync(userPasswordChangeDto, userId);

                if (!result.Errors.Any())
                {
                    await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

                    //LogOut olduktan sonra token oluşturarak yeniden giriş yapılıyor
                    var user = await _userApiService.GetUserByIdAsync(userId);
                    var tokenResponse = _tokenGenerator.GenerateToken(new CheckUserResponseDto { Id=user.Data.Id,Username= user.Data.Username},new RoleListDto { Roles = user.Data.Roles });

                    JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(tokenResponse.Token);

                    if (token != null)
                    {
                        var claims = token.Claims.ToList();
                        claims.Add(new Claim("accessToken", tokenResponse.Token == null ? "" : tokenResponse.Token));

                        ClaimsIdentity identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                        var authProps = new AuthenticationProperties
                        {
                            AllowRefresh = false,
                            ExpiresUtc = tokenResponse.ExpireDate,
                            IsPersistent = true,
                        };

                        await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProps);

                        _toastNotification.AddSuccessToastMessage("Şefreniz başarılı bir şekilde güncellenmiştir.", new ToastrOptions
                        {
                            Title = "İşlem Başarılı"
                        });

                        return View(userPasswordChangeDto);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Yeni şifre ile otomatik olarak giriş yapılamadı!");

                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                return View(userPasswordChangeDto);
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPut]
        public async Task<JsonResult> UndoDelete(int userId)
        {
            var result = await _userApiService.UndoDeleteAsync(userId);
            //var undoDeletedUser = JsonSerializer.Serialize(result.Data);
            //return Json(undoDeletedUser);
            if (!result.Errors.Any() && result.Data != null)
            {
                var undoDeletedUserModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Username}' adlı kullanıcı başarıyla arşivden geri alındı.",
                    UserDto = result.Data
                });
                return Json(undoDeletedUserModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undonDeletedUserErrorModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undonDeletedUserErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Delete")]
        [HttpPost]
        public async Task<JsonResult> HardDelete(int userId)
        {
            var result = await _userApiService.HardDeleteAsync(userId);
            //var undoDeletedUser = JsonSerializer.Serialize(result.Data);
            //return Json(undoDeletedUser);
            if (!result.Errors.Any() && result.Data != null)
            {
                var hardDeletedUserModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Username}' adlı kullanıcı başarıyla tamamen silindi.",
                    UserDto = result.Data
                });

                if (result.Data.ImageUrl != "userImages/defaultUser.png")
                    await _imageHelper.DeleteAsync(result.Data.ImageUrl);

                return Json(hardDeletedUserModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undonDeletedUserErrorModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undonDeletedUserErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<IActionResult> DeletedUsers()
        {
            var users = await _userApiService.GetAllByDeletedAsync();

            if (!users.Errors.Any())
                return View(users.Data);
            else
            {
                ViewBag.ErrorMessage = users.Errors.FirstOrDefault();
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<JsonResult> GetAllDeletedUsers()
        {
            var users = await _userApiService.GetAllByDeletedAsync();
            if (users.Data != null && !users.Errors.Any())
            {
                var userListModel = JsonSerializer.Serialize(new UserViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    UserListDto = users.Data
                });

                return Json(userListModel);
            }

            string errorMessages = String.Empty;
            foreach (var error in users.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            var userListModelError = JsonSerializer.Serialize(new UserViewModel
            {
                ResultStatus = ResultStatus.Error,
                Message = $"{errorMessages}\n",
            });

            return Json(userListModelError);
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPut]
        public async Task<JsonResult> ActivateUser(int userId)
        {
            var result = await _userApiService.ActivateUserAsync(userId);

            if (!result.Errors.Any())
            {
                var userJson = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(userJson);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                return Json(JsonSerializer.Serialize(new { error = errorMessages }));
            }
        }


    }
}

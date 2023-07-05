using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Enums;
using BlogApp.WEB.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BlogApp.Core.Utilities.Abstract;
using System.Text.Json;

namespace BlogApp.WEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthApiService _authApiService;
        private readonly UserApiService _userApiService;
        private readonly IImageHelper _imageHelper;

        public AuthController(AuthApiService authApiService, IImageHelper imageHelper, UserApiService userApiService)
        {
            _authApiService = authApiService;
            _imageHelper = imageHelper;
            _userApiService = userApiService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var uploadedImageDtoResult = await _imageHelper.UploadAsync(registerDto.Username, registerDto.ImageFile, ImageType.User);
            registerDto.ImageUrl = uploadedImageDtoResult.StatusCode == 200 ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

            var userDto = await _userApiService.RegisterAsync(registerDto);

            if (!userDto.Errors.Any())
            {
                return RedirectToAction("LogIn", "Auth");
            }
            else
            {
                if (registerDto.ImageUrl != "userImages/defaultUser.png")
                    await _imageHelper.DeleteAsync(registerDto.ImageUrl);

                foreach (var error in userDto.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View();
            }
        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(UserLoginDto loginDto)
        {
            var tokenModel = await _authApiService.LogIn(loginDto);

            if (!tokenModel.Errors.Any())
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenModel.Data?.Token);

                if (token != null)
                {
                    var claims = token.Claims.ToList();
                    claims.Add(new Claim("accessToken", tokenModel.Data?.Token == null ? "" : tokenModel.Data.Token));

                    ClaimsIdentity identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);

                    var authProps = new AuthenticationProperties
                    {
                        AllowRefresh = false,
                        ExpiresUtc = tokenModel.Data?.ExpireDate,
                        IsPersistent = true,
                    };

                    await HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProps);

                    return RedirectToAction("Index", "Home"/*, new { Area = "Admin" }*/);
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalıdır.");

                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", tokenModel.Errors.FirstOrDefault());

                return View();
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
        }
    }
}

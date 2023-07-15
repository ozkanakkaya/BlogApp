using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Enums;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogApp.Core.Utilities.Abstract;
using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using NToastNotify;
using Microsoft.AspNetCore.Http;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly BlogApiService _blogApiService;
        private readonly CategoryApiService _categoryApiService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageHelper _imageHelper;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;

        public BlogController(BlogApiService blogApiService, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper, IMapper mapper, IToastNotification toastNotification, CategoryApiService categoryApiService)
        {
            _blogApiService = blogApiService;
            _httpContextAccessor = httpContextAccessor;
            _imageHelper = imageHelper;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _categoryApiService = categoryApiService;
        }

        [Authorize(Roles = "SuperAdmin,Admin,Blog.Create")]
        public async Task<IActionResult> Index()
        {
            var httpContext = _httpContextAccessor.HttpContext.User.Claims;
            var userRoles = httpContext.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            if (userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin"))
            {
                var result = await _blogApiService.GetAllAsync();

                if (!result.Errors.Any() && result.Data != null)
                {
                    ViewBag.TableTitle = "Tüm Blog Yazıları";
                    return View(result.Data);
                }

                else
                {
                    ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                    return View();
                }
            }
            else
            {
                var userId = int.Parse(httpContext.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                var result = await _blogApiService.GetAllByUserIdAsync(userId);

                if (!result.Errors.Any() && result.Data != null)
                {
                    ViewBag.TableTitle = "Blog Yazılarınız";
                    return View(result.Data);
                }
                else
                {
                    ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                    return View();
                }
            }
        }

        [Authorize(Roles = "SuperAdmin,Blog.Create")]
        public async Task<IActionResult> Add()
        {
            var result = await _categoryApiService.GetAllByActiveAsync();

            if (!result.Errors.Any())
            {
                return View(new BlogAddViewModel
                {
                    Categories = result.Data.Categories
                });
            }
            return NotFound();
        }

        [Authorize(Roles = "SuperAdmin,Blog.Create")]
        [HttpPost]
        public async Task<IActionResult> Add(BlogAddViewModel blogAddViewModel)
        {
            if (ModelState.IsValid)
            {
                var httpContext = _httpContextAccessor.HttpContext.User.Claims;
                var userId = int.Parse(httpContext.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

                var blogCreateDto = _mapper.Map<BlogCreateDto>(blogAddViewModel);

                var imageResult = await _imageHelper.UploadAsync(blogAddViewModel.Title, blogAddViewModel.ImageFile, ImageType.Post);

                blogCreateDto.ImageUrl = imageResult.StatusCode == (int)ResultStatus.Success ? imageResult.Data.FullName : "postImages/defaultImage.png";
                blogCreateDto.UserId = userId;

                var userDto = await _blogApiService.AddAsync(blogCreateDto);

                if (!userDto.Errors.Any())
                {

                    _toastNotification.AddSuccessToastMessage("Blog yazınız başarılı bir şekilde eklenmiştir.", new ToastrOptions
                    {
                        Title = "İşlem Başarılı"
                    });
                    return RedirectToAction("Index", "Blog");
                }
                else
                {
                    if (blogCreateDto.ImageUrl != "postImages/defaultImage.png")
                        await _imageHelper.DeleteAsync(blogCreateDto.ImageUrl);

                    foreach (var error in userDto.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            var categories = await _categoryApiService.GetAllByActiveAsync();
            blogAddViewModel.Categories = categories.Data.Categories;
            return View(blogAddViewModel);

        }
    }
}

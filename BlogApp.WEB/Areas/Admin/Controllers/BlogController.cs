using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Enums;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BlogApp.Core.Utilities.Abstract;
using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using NToastNotify;
using System.Text.Json.Serialization;
using System.Text.Json;
using BlogApp.Core.Response;

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
            var isAdmin = userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin");

            var result = isAdmin
                ? await _blogApiService.GetAllAsync()
                : await GetBlogPostsByUserIdAsync(httpContext);

            if (!result.Errors.Any() && result.Data != null)
            {
                ViewBag.TableTitle = isAdmin ? "Tüm Blog Yazıları" : "Blog Yazılarınız";
                return View(result.Data);
            }

            ViewBag.ErrorMessage = result.Errors.Any() ? result.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
            return View();
        }

        private async Task<CustomResponseDto<List<BlogListDto>>> GetBlogPostsByUserIdAsync(IEnumerable<Claim> claims)
        {
            var userId = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            return await _blogApiService.GetAllByUserIdAsync(userId);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Blog.Read")]
        public async Task<JsonResult> GetAllBlogs()
        {
            var httpContext = _httpContextAccessor.HttpContext.User.Claims;
            var userRoles = httpContext.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            var blogPosts = (userRoles.Contains("SuperAdmin") || userRoles.Contains("Admin"))
                ? await _blogApiService.GetAllAsync()
                : await GetBlogPostsByUserIdAsync(httpContext);


            if (!blogPosts.Errors.Any())
            {
                var blogListDto = JsonSerializer.Serialize(blogPosts.Data, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });

                return Json(blogListDto);
            }

            string errorMessages = String.Empty;
            foreach (var error in blogPosts.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            return Json(JsonSerializer.Serialize(new { error = errorMessages }));
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

        [Authorize(Roles = "SuperAdmin,Blog.Update")]
        public async Task<IActionResult> Update(int blogId)
        {
            var blogPostResult = await _blogApiService.GetByBlogIdAsync(blogId);
            var categoriesResult = await _categoryApiService.GetAllByActiveAsync();

            if (!blogPostResult.Errors.Any() && !categoriesResult.Errors.Any())
            {
                var blogUpdateViewModel = _mapper.Map<BlogUpdateViewModel>(blogPostResult.Data);
                blogUpdateViewModel.CategoryIds = blogPostResult.Data.Categories.Select(tag => tag.Id).ToList();
                blogUpdateViewModel.Tags = string.Join(", ", blogPostResult.Data.Tags.Select(tag => tag.Name));

                blogUpdateViewModel.Categories = categoriesResult.Data.Categories;
                return View(blogUpdateViewModel);
            }
            return NotFound();
        }

        [Authorize(Roles = "SuperAdmin,Blog.Update")]
        [HttpPost]
        public async Task<IActionResult> Update(BlogUpdateViewModel blogUpdateViewModel)
        {
            if (ModelState.IsValid)
            {
                bool isNewImageUploaded = false;

                var oldUserImage = blogUpdateViewModel.ImageUrl;

                if (blogUpdateViewModel.ImageFile != null)
                {
                    var uploadedImageDtoResult = await _imageHelper.UploadAsync(blogUpdateViewModel.Title, blogUpdateViewModel.ImageFile, ImageType.Post);

                    if (uploadedImageDtoResult.StatusCode == 200)
                        blogUpdateViewModel.ImageUrl = uploadedImageDtoResult.Data.FullName;

                    if (oldUserImage != "postImages/defaultImage.png")
                        isNewImageUploaded = true;
                }

                var blogUpdateDto = _mapper.Map<BlogUpdateDto>(blogUpdateViewModel);
                var result = await _blogApiService.UpdateAsync(blogUpdateDto);

                if (!result.Errors.Any())
                {
                    if (isNewImageUploaded)
                    {
                        await _imageHelper.DeleteAsync(oldUserImage);
                    }

                    _toastNotification.AddSuccessToastMessage("Blog yazınız başarılı bir şekilde güncellenmiştir.", new ToastrOptions
                    {
                        Title = "İşlem Başarılı"
                    });
                    return RedirectToAction("Index", "Blog");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            var categories = await _categoryApiService.GetAllByActiveAsync();
            blogUpdateViewModel.Categories = categories.Data.Categories;
            return View(blogUpdateViewModel);
        }

        [Authorize(Roles = "SuperAdmin,Blog.Delete")]
        [HttpPut]
        public async Task<JsonResult> Delete(int blogId)
        {
            var result = await _blogApiService.DeleteAsync(blogId);

            if (!result.Errors.Any())
            {
                var deletedBlogModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Title}' başlıklı blog yazısı başarıyla silindi. \nTamamen silmek için veya geri almak için : \nÇöp Kutusu/Blog Yazıları menüsüne gidiniz.",
                    BlogDto = result.Data
                });
                return Json(deletedBlogModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var deletedBlogErrorModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(deletedBlogErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Blog.Read")]
        public async Task<IActionResult> DeletedBlogPosts()
        {
            var users = await _blogApiService.GetAllByDeletedAsync();

            if (!users.Errors.Any())
                return View(users.Data);
            else
            {
                ViewBag.ErrorMessage = users.Errors.FirstOrDefault();
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<JsonResult> GetAllDeletedBlogPosts()
        {
            var blogPosts = await _blogApiService.GetAllByDeletedAsync();
            if (blogPosts.Data != null && !blogPosts.Errors.Any())
            {
                var blogPostListModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    BlogListDto = blogPosts.Data
                }
                , new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });

                return Json(blogPostListModel);
            }

            string errorMessages = String.Empty;
            foreach (var error in blogPosts.Errors)
            {
                errorMessages = $"*{error}\n";
            }

            var blogPostListModelError = JsonSerializer.Serialize(new BlogViewModel
            {
                ResultStatus = ResultStatus.Error,
                Message = $"{errorMessages}\n",
            });

            return Json(blogPostListModelError);
        }

        [Authorize(Roles = "SuperAdmin,Blog.Update")]
        [HttpPut]
        public async Task<JsonResult> UndoDelete(int blogId)
        {
            var result = await _blogApiService.UndoDeleteAsync(blogId);

            if (!result.Errors.Any() && result.Data != null)
            {
                var undoDeletedBlogPostModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Title}' başlıklı blog yazısı başarıyla arşivden geri alındı.",
                    BlogDto = result.Data
                });
                return Json(undoDeletedBlogPostModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undonDeletedBlogPostErrorModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undonDeletedBlogPostErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Blog.Delete")]
        [HttpDelete]
        public async Task<JsonResult> HardDelete(int blogId)
        {
            var result = await _blogApiService.HardDeleteAsync(blogId);

            if (!result.Errors.Any() && result.Data != null)
            {
                var hardDeleteBlogPostModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"'{result.Data.Title}' başlıklı blog yazısızı başarıyla tamamen silindi.",
                    BlogDto = result.Data
                });

                if (result.Data.ImageUrl != "postImages/defaultImage.png")
                    await _imageHelper.DeleteAsync(result.Data.ImageUrl);

                return Json(hardDeleteBlogPostModel);
            }
            else
            {
                string errorMessages = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error}\n";
                }

                var undoDeleteBlogPostErrorModel = JsonSerializer.Serialize(new BlogViewModel
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}\n",
                });
                return Json(undoDeleteBlogPostErrorModel);
            }
        }

        [Authorize(Roles = "SuperAdmin,Blog.Read")]
        public async Task<JsonResult> GetAllByViewCount(bool isAscending, int takeSize)
        {
            var result = await _blogApiService.GetAllByViewCountAsync(isAscending, takeSize);
            var blogPosts = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(blogPosts);
        }
    }
}

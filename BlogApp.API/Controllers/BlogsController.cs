using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlogApp.API.Controllers
{
    public class BlogsController : CustomBaseController
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        private readonly IValidator<BlogCreateDto> _blogCreateDtoValidator;
        private readonly IValidator<BlogUpdateDto> _blogUpdateDtoValidotor;

        public BlogsController(IBlogService blogService, IMapper mapper, IValidator<BlogCreateDto> blogCreateDtoValidator, IValidator<BlogUpdateDto> blogUpdateDtoValidotor)
        {
            _blogService = blogService;
            _mapper = mapper;
            _blogCreateDtoValidator = blogCreateDtoValidator;
            _blogUpdateDtoValidotor = blogUpdateDtoValidotor;
        }

        //[Authorize(Roles = "Admin,Author")]
        [HttpPost]
        public async Task<IActionResult> Add(BlogCreateDto blogDto)
        {
            var result = _blogCreateDtoValidator.Validate(blogDto);

            if (result.IsValid)
            {
                //var createDto = _mapper.Map<BlogCreateDto>(blogDto);

                var addResult = await _blogService.AddBlogWithTagsAndCategoriesAsync(blogDto);
                //if (!addResult.Errors.Any())
                return CreateActionResult(CustomResponse<BlogCreateDto>.Success(201, addResult.Data));
                //return CreateActionResult(CustomResponse<BlogCreateDto>.Fail(addResult.StatusCode, addResult.Errors));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        [HttpPut]
        public async Task<IActionResult> Update(BlogUpdateDto blogUpdateDto)
        {
            var result = _blogUpdateDtoValidotor.Validate(blogUpdateDto);

            if (result.IsValid)
            {
                var result1 = await _blogService.UpdateBlogAsync(blogUpdateDto);
                if (result1.StatusCode == 204)
                {
                    return CreateActionResult(CustomResponse<NoContent>.Success(204));
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        [HttpDelete("{id}")]//api/blogs/13
        public IActionResult Delete(int blogId)
        {
            var result = _blogService.DeleteAsync(blogId);

            if (!result.Result.Errors.Any())
                return CreateActionResult(CustomResponse<NoContent>.Success(result.Result.StatusCode));

            return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Result.Errors));
        }

        [HttpGet]//api/blogs
        public IActionResult GetAllByNonDeletedAndActiveBlogs()
        {
            var blogs = _blogService.GetAllByNonDeletedAndActive();
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public IActionResult GetAllByDeletedBlogs()
        {
            var deletedBlogs = _blogService.GetAllByDeleted();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogDto>>.Success(200, deletedBlogs.Data));
        }

        [HttpGet("[action]/{userId}")]
        public IActionResult GetBlogsByUserId(int userId)
        {
            var blogs = _blogService.GetByUserId(userId);
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponse<PersonalBlogDto>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var deletedBlogs = _blogService.GetAll();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogDto>>.Success(200, deletedBlogs.Data));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> HardDelete(int blogId)
        {
            var result = await _blogService.HardDeleteAsync(blogId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
            }
            return CreateActionResult(CustomResponse<NoContent>.Success(result.StatusCode));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UndoDelete(int blogId)
        {
            var result = await _blogService.UndoDeleteAsync(blogId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
            }
            return CreateActionResult(CustomResponse<NoContent>.Success(result.StatusCode));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var searchResult = await _blogService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            if (searchResult.StatusCode == 200 && searchResult.Data.Count > 0)
            {
                return CreateActionResult(CustomResponse<BlogSearchModel>.Success(200, new BlogSearchModel
                {
                    BlogListDto = searchResult.Data,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = searchResult.Data.Count,
                    IsAscending = isAscending,
                    Keyword = keyword
                }));
            }
            return CreateActionResult(CustomResponse<NoContent>.Fail(404, "Anahtar kelime bulunamadı!"));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByViewCount(bool isAscending,int takeSize)
        {
            var result = await _blogService.GetAllByViewCountAsync(isAscending, takeSize);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(result.StatusCode, result.Data));

        }
    }
}

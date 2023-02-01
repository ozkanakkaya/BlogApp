﻿using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllByNonDeletedAndActiveBlogs()
        {
            var blogs = await _blogService.GetAllByNonDeletedAndActive();
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeletedBlogs()
        {
            var deletedBlogs = await _blogService.GetAllByDeletedAsync();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, deletedBlogs.Data));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetBlogsByUserId(int userId)
        {
            var blogs = await _blogService.GetAllByUserIdAsync(userId);
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponse<PersonalBlogDto>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var deletedBlogs = await _blogService.GetAllBlogsAsync();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, deletedBlogs.Data));
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

        [HttpPost("[action]")]
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByPaging(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var result = await (categoryId == null
                ? _blogService.GetAllByPagingAsync(null, currentPage, pageSize, isAscending)
                : _blogService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize, isAscending)
                );
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
            }

            return CreateActionResult(CustomResponse<BlogSearchModel>.Success(result.StatusCode, new BlogSearchModel
            {
                BlogListDto = result.Data,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = result.Data.Count,
                IsAscending = isAscending,
            }));

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotalBlogs()
        {
            var result = await _blogService.CountTotalBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountActiveBlogs()
        {
            var result = await _blogService.CountActiveBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountInActiveBlogs()
        {
            var result = await _blogService.CountInActiveBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByDeletedBlogs()
        {
            var result = await _blogService.CountByDeletedBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeletedBlogs()
        {
            var result = await _blogService.CountByNonDeletedBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public IActionResult IncreaseViewCount(int blogId)
        {
            var result = _blogService.IncreaseViewCountAsync(blogId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<string>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByCategory(int categoryId)
        {
            var result = await _blogService.GetAllByCategoryAsync(categoryId);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(result.StatusCode, result.Data));
        }

    }
}

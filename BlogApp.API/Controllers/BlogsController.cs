using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
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
                var resultUpdate = await _blogService.UpdateBlogAsync(blogUpdateDto);
                if (resultUpdate.StatusCode == 204)
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
        public async Task<IActionResult> GetAllByActive()
        {
            var blogs = await _blogService.GetAllByActiveAsync();
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByNonDeleted()
        {
            var blogs = await _blogService.GetAllByNonDeletedAsync();
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var deletedBlogs = await _blogService.GetAllByDeletedAsync();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(200, deletedBlogs.Data));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetAllByUserId(int userId)
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

            if (searchResult.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(searchResult.StatusCode, searchResult.Errors));
            }

            return CreateActionResult(CustomResponse<BlogViewModel>.Success(200, new BlogViewModel
            {
                BlogListDto = searchResult.Data,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchResult.Data.Count,
                IsAscending = isAscending,
                Keyword = keyword
            }));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByViewCount(bool isAscending, int takeSize)
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

            return CreateActionResult(CustomResponse<BlogViewModel>.Success(result.StatusCode, new BlogViewModel
            {
                BlogListDto = result.Data,
                CategoryId = categoryId.HasValue ? categoryId.Value : null,
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
        public async Task<IActionResult> CountInactiveBlogs()
        {
            var result = await _blogService.CountInactiveBlogsAsync();
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
        public async Task<IActionResult> IncreaseViewCount(int blogId)
        {
            var result = await _blogService.IncreaseViewCountAsync(blogId);
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var result = await _blogService.GetAllByUserIdOnFilterAsync(userId, filterBy, orderBy, isAscending, takeSize, categoryId, startAt, endAt, minViewCount, maxViewCount, minCommentCount, maxCommentCount);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            var result = await _blogService.GetAllBlogsFilteredAsync(categoryId, userId, isActive, isDeleted, currentPage, pageSize, orderBy, isAscending, includeCategory, includeTag, includeComments, includeUser);
            if (!result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<BlogViewModel>.Success(200, new BlogViewModel
                {
                    BlogListDto = result.Data,
                    CategoryId = categoryId.HasValue ? categoryId.Value : null,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = result.Data.Count,
                    IsAscending = isAscending,
                }));
            }
            return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetByBlogId(int blogId)
        {
            var result = await _blogService.GetByBlogIdAsync(blogId);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<BlogListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFilteredByBlogId(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser)
        {
            var result = await _blogService.GetFilteredByBlogIdAsync(blogId, includeCategory, includeTag, includeComment, includeUser);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<BlogListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByTag(int tagId)
        {
            var result = await _blogService.GetAllByTagAsync(tagId);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<List<BlogListDto>>.Success(result.StatusCode, result.Data));
        }
    }
}

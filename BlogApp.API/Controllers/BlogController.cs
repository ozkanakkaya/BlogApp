using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class BlogController : CustomControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        private readonly IValidator<BlogCreateDto> _blogCreateDtoValidator;
        private readonly IValidator<BlogUpdateDto> _blogUpdateDtoValidotor;

        public BlogController(IBlogService blogService, IMapper mapper, IValidator<BlogCreateDto> blogCreateDtoValidator, IValidator<BlogUpdateDto> blogUpdateDtoValidotor)
        {
            _blogService = blogService;
            _mapper = mapper;
            _blogCreateDtoValidator = blogCreateDtoValidator;
            _blogUpdateDtoValidotor = blogUpdateDtoValidotor;
        }

        //[Authorize(Roles = "Admin,Author")]
        [HttpPost]
        public async Task<IActionResult> Add(BlogCreateDto blogCreateDto)
        {
            var result = _blogCreateDtoValidator.Validate(blogCreateDto);

            if (result.IsValid)
            {
                //var createDto = _mapper.Map<BlogCreateDto>(blogDto);

                var addResult = await _blogService.AddBlogWithTagsAndCategoriesAsync(blogCreateDto);
                //if (!addResult.Errors.Any())
                return CreateActionResult(CustomResponseDto<BlogCreateDto>.Success(200, addResult.Data));
                //return CreateActionResult(CustomResponse<BlogCreateDto>.Fail(addResult.StatusCode, addResult.Errors));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<BlogCreateDto>.Fail(200, errors));
        }

        [HttpPut]
        public async Task<IActionResult> Update(BlogUpdateDto blogUpdateDto)
        {
            var result = _blogUpdateDtoValidotor.Validate(blogUpdateDto);

            if (result.IsValid)
            {
                var resultUpdate = await _blogService.UpdateBlogAsync(blogUpdateDto);
                if (resultUpdate.StatusCode == 200)
                {
                    return CreateActionResult(CustomResponseDto<NoContent>.Success(200));
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(200, errors));
        }

        [HttpPut("[action]/{blogId}")]
        public async Task<IActionResult> Delete(int blogId)
        {
            var result = await _blogService.DeleteAsync(blogId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<BlogListDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<BlogListDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpGet]//api/blogs
        public async Task<IActionResult> GetAllByActive()
        {
            var blogs = await _blogService.GetAllByActiveAsync();
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByNonDeleted()
        {
            var blogs = await _blogService.GetAllByNonDeletedAsync();
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(404, blogs.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(200, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var deletedBlogs = await _blogService.GetAllByDeletedAsync();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Fail(deletedBlogs.StatusCode, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(deletedBlogs.StatusCode, deletedBlogs.Data));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            var blogs = await _blogService.GetAllByUserIdAsync(userId);
            if (blogs.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Fail(blogs.StatusCode, blogs.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(blogs.StatusCode, blogs.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var deletedBlogs = await _blogService.GetAllBlogsAsync();
            if (deletedBlogs.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(404, deletedBlogs.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(200, deletedBlogs.Data));
        }

        [HttpDelete("{blogId}")]
        public async Task<IActionResult> HardDelete(int blogId)
        {
            var result = await _blogService.HardDeleteAsync(blogId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<BlogListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<BlogListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpPut("[action]/{blogId}")]
        public async Task<IActionResult> UndoDelete(int blogId)
        {
            var result = await _blogService.UndoDeleteAsync(blogId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<BlogListDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<BlogListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            var searchResult = await _blogService.SearchAsync(keyword, currentPage, pageSize, isAscending);

            if (searchResult.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<BlogListResultDto>.Fail(searchResult.StatusCode, searchResult.Errors));
            }

            return CreateActionResult(CustomResponseDto<BlogListResultDto>.Success(searchResult.StatusCode, searchResult.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByViewCount(bool isAscending, int takeSize)
        {
            var result = await _blogService.GetAllByViewCountAsync(isAscending, takeSize);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(result.StatusCode, result.Data));

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
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(404, result.Errors));
            }

            return CreateActionResult(CustomResponseDto<BlogListResultDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotalBlogs()
        {
            var result = await _blogService.CountTotalBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountActiveBlogs()
        {
            var result = await _blogService.CountActiveBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountInactiveBlogs()
        {
            var result = await _blogService.CountInactiveBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByDeletedBlogs()
        {
            var result = await _blogService.CountByDeletedBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeletedBlogs()
        {
            var result = await _blogService.CountByNonDeletedBlogsAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{blogId}")]
        public async Task<IActionResult> IncreaseViewCount(int blogId)
        {
            var result = await _blogService.IncreaseViewCountAsync(blogId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<string>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetAllByCategory(int categoryId)
        {
            var result = await _blogService.GetAllByCategoryAsync(categoryId);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var result = await _blogService.GetAllByUserIdOnFilterAsync(userId, filterBy, orderBy, isAscending, takeSize, categoryId, startAt, endAt, minViewCount, maxViewCount, minCommentCount, maxCommentCount);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllFiltered(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser)
        {
            var result = await _blogService.GetAllBlogsFilteredAsync(categoryId, userId, isActive, isDeleted, currentPage, pageSize, orderBy, isAscending, includeCategory, includeTag, includeComments, includeUser);
            if (!result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<BlogListResultDto>.Success(200, result.Data));
            }
            return CreateActionResult(CustomResponseDto<NoContent>.Fail(404, result.Errors));
        }

        [HttpGet("[action]/{blogId}")]
        public async Task<IActionResult> GetByBlogId(int blogId)
        {
            var result = await _blogService.GetByBlogIdAsync(blogId);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<BlogListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFilteredByBlogId(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser)
        {
            var result = await _blogService.GetFilteredByBlogIdAsync(blogId, includeCategory, includeTag, includeComment, includeUser);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<BlogListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{tagId}")]
        public async Task<IActionResult> GetAllByTag(int tagId)
        {
            var result = await _blogService.GetAllByTagAsync(tagId);

            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<BlogListDto>>.Success(result.StatusCode, result.Data));
        }
    }
}

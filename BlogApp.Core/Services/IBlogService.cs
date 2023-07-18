using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IBlogService : IService<Blog>
    {
        Task<CustomResponseDto<BlogCreateDto>> AddBlogWithTagsAndCategoriesAsync(BlogCreateDto blogDto);
        Task<CustomResponseDto<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto);
        Task<CustomResponseDto<BlogListDto>> DeleteAsync(int blogId);
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByActiveAsync();
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByNonDeletedAsync();
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByDeletedAsync();
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByUserIdAsync(int userId);
        Task<CustomResponseDto<List<BlogListDto>>> GetAllBlogsAsync();
        Task<CustomResponseDto<BlogListDto>> HardDeleteAsync(int blogId);
        Task<CustomResponseDto<BlogListDto>> UndoDeleteAsync(int blogId);
        Task<CustomResponseDto<BlogListResultDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
        Task<CustomResponseDto<BlogListResultDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<CustomResponseDto<int>> CountTotalBlogsAsync();
        Task<CustomResponseDto<int>> CountActiveBlogsAsync();
        Task<CustomResponseDto<int>> CountInactiveBlogsAsync();
        Task<CustomResponseDto<int>> CountByDeletedBlogsAsync();
        Task<CustomResponseDto<int>> CountByNonDeletedBlogsAsync();
        Task<CustomResponseDto<string>> IncreaseViewCountAsync(int blogId);
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByCategoryAsync(int categoryId);
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount);
        Task<CustomResponseDto<BlogListResultDto>> GetAllBlogsFilteredAsync(int? categoryId, int? userId, bool? isActive, bool? isDeleted, int currentPage, int pageSize, OrderByGeneral orderBy, bool isAscending, bool includeCategory, bool includeTag, bool includeComments, bool includeUser);
        Task<CustomResponseDto<BlogListDto>> GetByBlogIdAsync(int blogId);
        Task<CustomResponseDto<BlogListDto>> GetFilteredByBlogIdAsync(int blogId, bool includeCategory, bool includeTag, bool includeComment, bool includeUser);
        Task<CustomResponseDto<List<BlogListDto>>> GetAllByTagAsync(int tagId);
    }
}

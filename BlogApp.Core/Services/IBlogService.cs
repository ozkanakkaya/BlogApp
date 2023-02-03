using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IBlogService : IService<Blog>
    {
        Task<CustomResponse<BlogCreateDto>> AddBlogWithTagsAndCategoriesAsync(BlogCreateDto blogDto);
        Task<CustomResponse<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto);
        Task<CustomResponse<List<BlogListDto>>> GetAllByNonDeletedAndActive();
        Task<CustomResponse<NoContent>> DeleteAsync(int blogId);
        Task<CustomResponse<List<BlogListDto>>> GetAllByDeletedAsync();
        Task<CustomResponse<PersonalBlogDto>> GetAllByUserIdAsync(int userId);
        Task<CustomResponse<List<BlogListDto>>> GetAllBlogsAsync();
        Task<CustomResponse<NoContent>> HardDeleteAsync(int blogId);
        Task<CustomResponse<NoContent>> UndoDeleteAsync(int blogId);
        Task<CustomResponse<List<BlogListDto>>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<CustomResponse<List<BlogListDto>>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
        Task<CustomResponse<List<BlogListDto>>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<CustomResponse<int>> CountTotalBlogsAsync();
        Task<CustomResponse<int>> CountActiveBlogsAsync();
        Task<CustomResponse<int>> CountInActiveBlogsAsync();
        Task<CustomResponse<int>> CountByDeletedBlogsAsync();
        Task<CustomResponse<int>> CountByNonDeletedBlogsAsync();
        CustomResponse<string> IncreaseViewCountAsync(int blogId);
        Task<CustomResponse<List<BlogListDto>>> GetAllByCategoryAsync(int categoryId);
        Task<CustomResponse<List<BlogListDto>>> GetAllByUserIdOnFilterAsync(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount);
    }
}

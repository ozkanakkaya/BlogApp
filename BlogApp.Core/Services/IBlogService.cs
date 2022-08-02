using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IBlogService : IService<Blog>
    {
        Task<CustomResponse<BlogCreateDto>> AddBlogWithTagsAndCategoriesAsync(BlogCreateDto blogDto);
        Task<CustomResponse<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto);
        CustomResponse<List<BlogListDto>> GetAllByNonDeletedAndActive();
        Task<CustomResponse<NoContent>> DeleteAsync(int blogId);
        CustomResponse<List<BlogDto>> GetAllByDeleted();
        CustomResponse<PersonalBlogDto> GetByUserId(int userId);
    }
}

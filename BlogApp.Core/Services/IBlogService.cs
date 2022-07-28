using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IBlogService : IService<Blog>
    {
        Task<CustomResponse<BlogCreateDto>> AddBlogWithTagsAsync(BlogCreateDto blogDto);
        Task<CustomResponse<NoContent>> UpdateBlogAsync(BlogUpdateDto blogUpdateDto);
    }
}

using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IBlogService : IService<Blog>
    {//BlogCreateDto createDto, string tags, List<int> categories
        Task<CustomResponse<BlogDto>> AddBlogWithTagsAsync(BlogDto blogDto);
    }
}

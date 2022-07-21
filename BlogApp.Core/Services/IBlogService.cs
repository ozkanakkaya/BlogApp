using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.DTOs.Concrete.TagDtos;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Services
{
    public interface IBlogService : IService<Blog>
    {
        Task<CustomResponse<BlogCreateDto>> AddBlogWithTags(BlogCreateDto createDto, List<TagListDto> tagList);
    }
}

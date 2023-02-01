using AutoMapper;
using BlogApp.Core.DTOs.Concrete.BlogDtos;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogCreateDto>().ReverseMap();
            CreateMap<Blog, BlogUpdateDto>().ReverseMap();
            CreateMap<Blog, BlogListDto>().ReverseMap();
            CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}

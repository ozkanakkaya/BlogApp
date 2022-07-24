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
            CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<Blog, BlogCreateDto>().ReverseMap();
            CreateMap<Blog, BlogUpdateDto>().ReverseMap();
            CreateMap<Blog, BlogListDto>()
                .ForPath(dest => dest.Categories, opt => opt.MapFrom(src => src.BlogCategories.Select(x => x.Category)))
                .ForPath(dest => dest.Tags, opt => opt.MapFrom(src => src.BlogTags.Select(x => x.Tag)))
                .ReverseMap();
            CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}

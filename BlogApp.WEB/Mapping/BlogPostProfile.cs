using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Areas.Admin.Models;

namespace BlogApp.WEB.Mapping
{
    public class BlogPostProfile : Profile
    {
        public BlogPostProfile()
        {
            CreateMap<BlogCreateDto, BlogAddViewModel>().ReverseMap();
            CreateMap<BlogListDto, BlogUpdateViewModel>().ReverseMap();
            CreateMap<BlogUpdateDto, BlogUpdateViewModel>().ReverseMap();
        }
    }
}

using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Configurations;

namespace BlogApp.WEB.Mapping
{
    public class BlogPostProfile : Profile
    {
        public BlogPostProfile()
        {
            CreateMap<BlogCreateDto, BlogAddViewModel>().ReverseMap();
            CreateMap<BlogListDto, BlogUpdateViewModel>().ReverseMap();
            CreateMap<BlogUpdateDto, BlogUpdateViewModel>().ReverseMap();
            CreateMap<BlogRightSideBarWidgetOptions, BlogRightSideBarWidgetOptionsViewModel>().ReverseMap();

        }
    }
}

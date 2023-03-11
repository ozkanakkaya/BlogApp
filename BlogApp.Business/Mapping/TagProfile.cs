using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>().ReverseMap();
            CreateMap<Tag, TagListDto>().ReverseMap();
            CreateMap<Tag, TagUpdateDto>().ReverseMap();

        }
    }
}

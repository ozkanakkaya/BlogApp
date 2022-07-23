using AutoMapper;
using BlogApp.Core.DTOs.Concrete.TagDtos;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagListDto>().ReverseMap();
        }
    }
}

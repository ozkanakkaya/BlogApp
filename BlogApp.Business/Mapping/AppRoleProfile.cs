using AutoMapper;
using BlogApp.Core.DTOs.Concrete.AppRoleDtos;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class AppRoleProfile : Profile
    {
        public AppRoleProfile()
        {
            CreateMap<AppRole, AppRoleListDto>().ReverseMap();
        }
    }
}

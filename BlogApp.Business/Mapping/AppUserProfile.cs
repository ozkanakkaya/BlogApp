using AutoMapper;
using BlogApp.Core.DTOs.Concrete.AppUserDtos;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, AppUserRegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserListDto>().ReverseMap();
            CreateMap<AppUser, CheckUserResponseDto>().ReverseMap();
        }
    }
}

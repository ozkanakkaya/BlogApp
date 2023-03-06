using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, AppUserRegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserUpdateDto>().ReverseMap();
            CreateMap<AppUser, AppUserListDto>()
                .ForPath(dest => dest.Roles, opt => opt.MapFrom(src => src.AppUserRoles.Select(x => x.AppRole)))
                .ReverseMap();
            CreateMap<AppUser, CheckUserResponseDto>().ReverseMap();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUser, AppUserPasswordChangeDto>()
                .ForPath(dest => dest.NewPassword, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();
        }
    }
}

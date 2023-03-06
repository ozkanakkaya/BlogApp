using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();
            CreateMap<AppUser, UserListDto>()
                .ForPath(dest => dest.Roles, opt => opt.MapFrom(src => src.AppUserRoles.Select(x => x.AppRole)))
                .ReverseMap();
            CreateMap<AppUser, CheckUserResponseDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserPasswordChangeDto>()
                .ForPath(dest => dest.NewPassword, opt => opt.MapFrom(src => src.Password))
                .ReverseMap();
        }
    }
}

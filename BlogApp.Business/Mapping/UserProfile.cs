using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Business.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserListDto>()
                .ForPath(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(x => x.Role)))
                .ReverseMap();
            CreateMap<User, CheckUserResponseDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserPasswordChangeDto>()
                .ForPath(dest => dest.NewPassword, opt => opt.MapFrom(src => src.PasswordHash))
                .ReverseMap();
            CreateMap<UserListDto, UserUpdateDto>().ReverseMap();
        }
    }
}

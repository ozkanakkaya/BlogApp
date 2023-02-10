using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using FluentValidation;

namespace BlogApp.Business.Services
{
    public class AppUserService : Service<AppUser>, IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AppUserLoginDto> _loginValidator;

        public AppUserService(IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper, IAppUserRepository appUserRepository, IAppRoleRepository appRoleRepository, IValidator<AppUserLoginDto> loginValidator) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _unitOfWork = unitOfWork;
            _appRoleRepository = appRoleRepository;
            _loginValidator = loginValidator;
        }

        public CustomResponse<List<AppRoleListDto>> GetRolesByUserId(int userId)
        {
            var userRoles = _appRoleRepository.GetRolesByUserId(userId);

            if (userRoles == null)
            {
                return CustomResponse<List<AppRoleListDto>>.Fail(404, $"Id: {userId} kullanıcısının rolleri bulunamadı!");
            }
            var rolesDto = _mapper.Map<List<AppRoleListDto>>(userRoles);
            return CustomResponse<List<AppRoleListDto>>.Success(200, rolesDto);
        }

        public CustomResponse<CheckUserResponseDto> CheckUser(AppUserLoginDto dto)
        {
            var result = _loginValidator.Validate(dto);
            if (result.IsValid)
            {
                var user = _appUserRepository.GetAppUserWithLoginInfo(dto.Username, dto.Password);

                if (user != null)
                {
                    var userDto = _mapper.Map<CheckUserResponseDto>(user);
                    return CustomResponse<CheckUserResponseDto>.Success(200, userDto);
                }
                return CustomResponse<CheckUserResponseDto>.Fail(404, "Kullanıcı adı veya parola hatalıdır.");
            }
            return CustomResponse<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola boş geçilemez.");
        }

        public async Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId)
        {
            var hasUser = await _appUserRepository.AnyAsync(x => x.Username == dto.Username);

            if (!hasUser)
            {
                var user = _mapper.Map<AppUser>(dto);

                user.AppUserRoles = new List<AppUserRole>();
                user.AppUserRoles.Add(new AppUserRole
                {
                    AppUser = user,
                    AppRoleId = roleId
                });

                await _appUserRepository.AddAsync(user);
                await _unitOfWork.CommitAsync();

                return CustomResponse<AppUserRegisterDto>.Success(201, dto);
            }
            return CustomResponse<AppUserRegisterDto>.Fail(400, $"'{dto.Username}' kullanıcı adı zaten kayıtlı!");
        }
    }
}

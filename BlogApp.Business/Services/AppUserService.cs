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

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IValidator<AppUserRegisterDto> _registerValidator;

        public AppUserService(IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper, IAppUserRepository appUserRepository, IValidator<AppUserRegisterDto> registerValidator) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _unitOfWork = unitOfWork;
            _registerValidator = registerValidator;
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

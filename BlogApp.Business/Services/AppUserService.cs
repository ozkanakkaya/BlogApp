using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Core.Utilities.Responses;

namespace BlogApp.Business.Services
{
    public class AppUserService : Service<AppUser>, IAppUserService
    {
        private readonly IAppUserRepository _appUserRepository;

        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public AppUserService(IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper, IAppUserRepository appUserRepository) : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _appUserRepository = appUserRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId)
        {
            //validate
            var user = _mapper.Map<AppUser>(dto);

            user.AppUserRoles = new List<AppUserRole>();
            user.AppUserRoles.Add(new AppUserRole
            {
                AppUser = user,
                AppRoleId = roleId
            });

            await _appUserRepository.AddAsync(user);

            await _unitOfWork.CommitAsync();

            return CustomResponse<AppUserRegisterDto>.Success(200, dto);
        }
    }
}

using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Core.Utilities.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Business.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IValidator<UserLoginDto> _loginValidator;
        private readonly IImageHelper _imageHelper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(IGenericRepository<User> repository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<UserLoginDto> loginValidator, IImageHelper imageHelper, IPasswordHasher<User> passwordHasher) : base(repository, unitOfWork, mapper)
        {
            _loginValidator = loginValidator;
            _imageHelper = imageHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<CustomResponseDto<CheckUserResponseDto>> CheckUserAsync(UserLoginDto loginDto)
        {
            var result = _loginValidator.Validate(loginDto);
            if (result.IsValid)
            {
                //var user = UnitOfWork.Users.GetAppUserWithLoginInfo(dto.Username, dto.Password);
                var user = await UnitOfWork.Users.GetAsync(x => x.Username == loginDto.Username);
                if (user != null)
                {
                    var resultPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

                    if (resultPassword == PasswordVerificationResult.Success)
                    {
                        var userDto = Mapper.Map<CheckUserResponseDto>(user);
                        return CustomResponseDto<CheckUserResponseDto>.Success(200, userDto);
                    }
                }
                return CustomResponseDto<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola hatalıdır.");
            }
            return CustomResponseDto<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola boş geçilemez.");
        }

        public async Task<CustomResponseDto<UserDto>> RegisterWithRoleAsync(UserRegisterDto dto, int roleId)
        {
            var hasUser = await UnitOfWork.Users.AnyAsync(x => x.Username == dto.Username);

            if (!hasUser)
            {
                var user = Mapper.Map<User>(dto);
                user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
                user.ImageUrl = dto.ImageUrl;
                user.IsActive = true;//Mail onayı özelliği eklendiğinde burada pasif olarak değer alacak.

                user.UserRoles = new List<UserRole>();
                user.UserRoles.Add(new UserRole
                {
                    User = user,
                    RoleId = roleId
                });

                await UnitOfWork.Users.AddAsync(user);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<UserDto>.Success(201, Mapper.Map<UserDto>(user));
            }
            return CustomResponseDto<UserDto>.Fail(400, $"'{dto.Username}' kullanıcı adı zaten kayıtlı!");
        }

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllUsersAsync()
        {
            var users = await UnitOfWork.Users.GetAllAsync(null, x => x.UserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserListDto>(user)).ToList();

                return CustomResponseDto<List<UserListDto>>.Success(200, usersDto);
            }
            return CustomResponseDto<List<UserListDto>>.Fail(200, "Herhangi bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllByActiveAsync()
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.UserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserListDto>(user)).ToList();

                return CustomResponseDto<List<UserListDto>>.Success(200, usersDto);
            }
            return CustomResponseDto<List<UserListDto>>.Fail(200, "Herhangi bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<UserListDto>> GetUserByIdAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId, x => x.UserRoles);

            if (user != null)
            {
                var userDto = Mapper.Map<UserListDto>(user);

                return CustomResponseDto<UserListDto>.Success(200, userDto);
            }
            return CustomResponseDto<UserListDto>.Fail(200, "Gösterilecek bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<UserDto>> DeleteAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
            if (user != null)
            {
                user.IsDeleted = true;
                user.IsActive = false;

                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();
                return CustomResponseDto<UserDto>.Success(200, Mapper.Map<UserDto>(user));
            }
            return CustomResponseDto<UserDto>.Fail(200, $"{userId} numaralı kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<UserDto>> UndoDeleteAsync(int userId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Users.AnyAsync(x => x.Id == userId);
            if (result)
            {
                var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
                user.IsDeleted = false;
                user.IsActive = true;
                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();
                return CustomResponseDto<UserDto>.Success(200, Mapper.Map<UserDto>(user));
            }
            return CustomResponseDto<UserDto>.Fail(200, "Bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<UserDto>> HardDeleteAsync(int userId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Users.AnyAsync(x => x.Id == userId);
            if (result)
            {
                var user = UnitOfWork.Users.Where(x => x.Id == userId);
                var userDto = await user.Select(u => Mapper.Map<UserDto>(u)).FirstOrDefaultAsync();

                UnitOfWork.Users.RemoveRange(user);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<UserDto>.Success(200, userDto);
            }
            return CustomResponseDto<UserDto>.Fail(200, "Bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<List<UserDto>>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => x.IsDeleted, x => x.UserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserDto>(user)).ToList();

                return CustomResponseDto<List<UserDto>>.Success(200, usersDto);
            }
            return CustomResponseDto<List<UserDto>>.Fail(200, "Silinmiş bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<List<UserListDto>>> GetAllByInactiveAsync()//Admin-Arşiv
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => !x.IsActive & !x.IsDeleted, x => x.UserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserListDto>(user)).ToList();

                return CustomResponseDto<List<UserListDto>>.Success(200, usersDto);
            }
            return CustomResponseDto<List<UserListDto>>.Fail(200, "Aktif olmayan bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponseDto<UserDto>> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var oldUser = UnitOfWork.Users.Where(x => x.Id == userUpdateDto.Id).SingleOrDefault();

            var updateUser = Mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);

            UnitOfWork.Users.Update(updateUser);

            await UnitOfWork.CommitAsync();

            return CustomResponseDto<UserDto>.Success(200, Mapper.Map<UserDto>(updateUser));
        }

        public async Task<CustomResponseDto<NoContent>> PasswordChangeAsync(UserPasswordChangeDto userPasswordChangeDto, int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);

            var resultPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userPasswordChangeDto.CurrentPassword);

            if (resultPassword == PasswordVerificationResult.Success)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, userPasswordChangeDto.NewPassword);

                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();

                return CustomResponseDto<NoContent>.Success(200);
            }
            else
            {
                return CustomResponseDto<NoContent>.Fail(400, "Mevcut şifrenizi kontrol ediniz!");
            }
        }

        public async Task<CustomResponseDto<UserDto>> ActivateUserAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
            if (user.IsActive)
                return CustomResponseDto<UserDto>.Fail(200, "Kullanıcı zaten aktif!");
            user.IsActive = true;
            UnitOfWork.Users.Update(user);
            await UnitOfWork.CommitAsync();
            return CustomResponseDto<UserDto>.Success(200, Mapper.Map<UserDto>(user));
        }

        public async Task<CustomResponseDto<NoContent>> DeleteUserImageAsync(int userId)
        {
            var user = UnitOfWork.Users.Where(x => x.Id == userId).SingleOrDefault();

            if (user.ImageUrl != "userImages/defaultUser.png")
            {
                user.ImageUrl = "userImages/defaultUser.png";
                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();
                await _imageHelper.DeleteAsync(user.ImageUrl);

                return CustomResponseDto<NoContent>.Success(204);
            }

            return CustomResponseDto<NoContent>.Fail(400, "Silinecek bir profil fotoğrafı bulunamadı!");
        }

        public async Task<CustomResponseDto<int>> CountTotalAsync()
        {
            var categoriesCount = await UnitOfWork.Users.CountAsync();

            return categoriesCount > -1 ? CustomResponseDto<int>.Success(200, categoriesCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }

        public async Task<CustomResponseDto<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Users.CountAsync(x => !x.IsDeleted);

            return categoriesCount > -1 ? CustomResponseDto<int>.Success(200, categoriesCount) : CustomResponseDto<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }
    }
}

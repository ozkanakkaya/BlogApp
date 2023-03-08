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
    public class UserService : Service<AppUser>, IUserService
    {
        private readonly IValidator<UserLoginDto> _loginValidator;
        private readonly IImageHelper _imageHelper;
        private readonly IPasswordHasher<AppUser> _passwordHasher;

        public UserService(IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<UserLoginDto> loginValidator, IImageHelper imageHelper, IPasswordHasher<AppUser> passwordHasher) : base(repository, unitOfWork, mapper)
        {
            _loginValidator = loginValidator;
            _imageHelper = imageHelper;
            _passwordHasher = passwordHasher;
        }

        public async Task<CustomResponse<CheckUserResponseDto>> CheckUserAsync(UserLoginDto loginDto)
        {
            var result = _loginValidator.Validate(loginDto);
            if (result.IsValid)
            {
                //var user = UnitOfWork.Users.GetAppUserWithLoginInfo(dto.Username, dto.Password);
                var user = await UnitOfWork.Users.GetAsync(x => x.Username == loginDto.Username);
                if (user != null)
                {
                    var resultPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);

                    if (resultPassword == PasswordVerificationResult.Success)
                    {
                        var userDto = Mapper.Map<CheckUserResponseDto>(user);
                        return CustomResponse<CheckUserResponseDto>.Success(200, userDto);
                    }
                    return CustomResponse<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola hatalıdır.");
                }
                return CustomResponse<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola hatalıdır.");
            }
            return CustomResponse<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola boş geçilemez.");
        }

        public async Task<CustomResponse<UserRegisterDto>> RegisterWithRoleAsync(UserRegisterDto dto, int roleId)
        {
            var hasUser = await UnitOfWork.Users.AnyAsync(x => x.Username == dto.Username);

            if (!hasUser)
            {
                var uploadedImageDtoResult = await _imageHelper.UploadAsync(dto.Username, dto.ImageFile, ImageType.User);

                var user = Mapper.Map<AppUser>(dto);
                user.Password = _passwordHasher.HashPassword(user, dto.Password);
                user.ImageUrl = uploadedImageDtoResult.StatusCode == 200 ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                user.AppUserRoles = new List<AppUserRole>();
                user.AppUserRoles.Add(new AppUserRole
                {
                    AppUser = user,
                    AppRoleId = roleId
                });

                await UnitOfWork.Users.AddAsync(user);
                await UnitOfWork.CommitAsync();

                return CustomResponse<UserRegisterDto>.Success(201, dto);
            }
            return CustomResponse<UserRegisterDto>.Fail(400, $"'{dto.Username}' kullanıcı adı zaten kayıtlı!");
        }

        public async Task<CustomResponse<List<UserListDto>>> GetAllByActiveAsync()
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.AppUserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserListDto>(user)).ToList();

                return CustomResponse<List<UserListDto>>.Success(200, usersDto);
            }
            return CustomResponse<List<UserListDto>>.Fail(404, "Herhangi bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<UserListDto>> GetUserByIdAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId && x.IsActive && !x.IsDeleted, x => x.AppUserRoles);

            if (user != null)
            {
                var userDto = Mapper.Map<UserListDto>(user);

                return CustomResponse<UserListDto>.Success(200, userDto);
            }
            return CustomResponse<UserListDto>.Fail(404, "Gösterilecek bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> DeleteAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
            if (user != null)
            {
                user.IsDeleted = true;
                user.IsActive = false;

                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, $"{userId} numaralı kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UndoDeleteAsync(int userId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Users.AnyAsync(x => x.Id == userId);
            if (result)
            {
                var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
                user.IsDeleted = false;
                user.IsActive = true;
                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();
                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> HardDeleteAsync(int userId)//Admin-Arşiv-Users
        {
            var result = await UnitOfWork.Users.AnyAsync(x => x.Id == userId);
            if (result)
            {
                var user = UnitOfWork.Users.Where(x => x.Id == userId);

                var imageUrl = await user.Select(x => x.ImageUrl).FirstOrDefaultAsync();

                UnitOfWork.Users.RemoveRange(user);
                await UnitOfWork.CommitAsync();

                if (imageUrl != "userImages/defaultUser.png")
                    await _imageHelper.DeleteAsync(imageUrl);

                return CustomResponse<NoContent>.Success(204);
            }
            return CustomResponse<NoContent>.Fail(404, "Bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<List<UserListDto>>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => x.IsDeleted, x => x.AppUserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserListDto>(user)).ToList();

                return CustomResponse<List<UserListDto>>.Success(200, usersDto);
            }
            return CustomResponse<List<UserListDto>>.Fail(404, "Silinmiş bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<List<UserListDto>>> GetAllByInactiveAsync()//Admin-Arşiv
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => !x.IsActive & !x.IsDeleted, x => x.AppUserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<UserListDto>(user)).ToList();

                return CustomResponse<List<UserListDto>>.Success(200, usersDto);
            }
            return CustomResponse<List<UserListDto>>.Fail(404, "Aktif olmayan bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UpdateUserAsync(UserUpdateDto appUserUpdateDto)
        {
            bool isNewImageUploaded = false;

            var oldUser = UnitOfWork.Users.Where(x => x.Id == appUserUpdateDto.Id).SingleOrDefault();

            var oldUserImage = oldUser.ImageUrl;

            if (appUserUpdateDto.ImageFile != null)
            {
                var uploadedImageDtoResult = await _imageHelper.UploadAsync(oldUser.Username, appUserUpdateDto.ImageFile, ImageType.User);
                if (uploadedImageDtoResult.StatusCode == 200)
                    appUserUpdateDto.ImageUrl = uploadedImageDtoResult.Data.FullName;

                if (oldUserImage != "userImages/defaultUser.png")
                    isNewImageUploaded = true;
            }

            var updateUser = Mapper.Map<UserUpdateDto, AppUser>(appUserUpdateDto, oldUser);

            UnitOfWork.Users.Update(updateUser);

            await UnitOfWork.CommitAsync();

            if (isNewImageUploaded)
            {
                await _imageHelper.DeleteAsync(oldUserImage);
            }

            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<NoContent>> PasswordChangeAsync(UserPasswordChangeDto userPasswordChangeDto, string userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == int.Parse(userId));

            var resultPassword = _passwordHasher.VerifyHashedPassword(user, user.Password, userPasswordChangeDto.CurrentPassword);

            if (resultPassword == PasswordVerificationResult.Success)
            {
                user.Password = _passwordHasher.HashPassword(user, userPasswordChangeDto.NewPassword);

                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();

                return CustomResponse<NoContent>.Success(204);
            }
            else
            {
                return CustomResponse<NoContent>.Fail(400, "Mevcut şifrenizi kontrol ediniz!");
            }
        }

        public async Task<CustomResponse<NoContent>> ActivateUserAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
            if (user.IsActive)
                return CustomResponse<NoContent>.Fail(400, "Kullanıcı zaten aktif!");
            user.IsActive = true;
            UnitOfWork.Users.Update(user);
            await UnitOfWork.CommitAsync();
            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<NoContent>> DeleteUserImageAsync(int userId)
        {
            var user = UnitOfWork.Users.Where(x => x.Id == userId).SingleOrDefault();

            if (user.ImageUrl != "userImages/defaultUser.png")
            {
                user.ImageUrl = "userImages/defaultUser.png";
                UnitOfWork.Users.Update(user);
                await UnitOfWork.CommitAsync();
                await _imageHelper.DeleteAsync(user.ImageUrl);

                return CustomResponse<NoContent>.Success(204);
            }

            return CustomResponse<NoContent>.Fail(400, "Silinecek bir profil fotoğrafı bulunamadı!");
        }

        public async Task<CustomResponse<int>> CountTotalAsync()
        {
            var categoriesCount = await UnitOfWork.Users.CountAsync();

            return categoriesCount > -1 ? CustomResponse<int>.Success(200, categoriesCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }

        public async Task<CustomResponse<int>> CountByNonDeletedAsync()
        {
            var categoriesCount = await UnitOfWork.Users.CountAsync(x => !x.IsDeleted);

            return categoriesCount > -1 ? CustomResponse<int>.Success(200, categoriesCount) : CustomResponse<int>.Fail(400, $"Hata ile karşılaşıldı! Dönen sayı: {categoriesCount}");
        }
    }
}

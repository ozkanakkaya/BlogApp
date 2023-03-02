﻿using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using BlogApp.Core.Utilities.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Business.Services
{
    public class AppUserService : Service<AppUser>, IAppUserService
    {
        private readonly IValidator<AppUserLoginDto> _loginValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageHelper _imageHelper;

        public AppUserService(IGenericRepository<AppUser> repository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<AppUserLoginDto> loginValidator, IHttpContextAccessor httpContextAccessor, IImageHelper imageHelper) : base(repository, unitOfWork, mapper)
        {
            _loginValidator = loginValidator;
            _httpContextAccessor = httpContextAccessor;
            _imageHelper = imageHelper;
        }

        public async Task<CustomResponse<List<AppRoleDto>>> GetRolesByUserId(int userId)
        {
            var userRoles = await UnitOfWork.Roles.GetAllAsync(x => x.AppUserRoles.Any(x => x.AppUserId == userId));

            if (userRoles == null)
            {
                return CustomResponse<List<AppRoleDto>>.Fail(404, $"Id: {userId} kullanıcısının rolleri bulunamadı!");
            }
            var rolesDto = Mapper.Map<List<AppRoleDto>>(userRoles);
            return CustomResponse<List<AppRoleDto>>.Success(200, rolesDto);
        }

        public CustomResponse<CheckUserResponseDto> CheckUser(AppUserLoginDto dto)
        {
            var result = _loginValidator.Validate(dto);
            if (result.IsValid)
            {
                var user = UnitOfWork.Users.GetAppUserWithLoginInfo(dto.Username, dto.Password);

                if (user != null)
                {
                    var userDto = Mapper.Map<CheckUserResponseDto>(user);
                    return CustomResponse<CheckUserResponseDto>.Success(200, userDto);
                }
                return CustomResponse<CheckUserResponseDto>.Fail(404, "Kullanıcı adı veya parola hatalıdır.");
            }
            return CustomResponse<CheckUserResponseDto>.Fail(400, "Kullanıcı adı veya parola boş geçilemez.");
        }

        public async Task<CustomResponse<AppUserRegisterDto>> RegisterWithRoleAsync(AppUserRegisterDto dto, int roleId)
        {
            var hasUser = await UnitOfWork.Users.AnyAsync(x => x.Username == dto.Username);

            if (!hasUser)
            {
                var uploadedImageDtoResult = await _imageHelper.UploadAsync(dto.Username, dto.ImageFile, ImageType.User);
                dto.ImageUrl = uploadedImageDtoResult.StatusCode == 200 ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                var user = Mapper.Map<AppUser>(dto);

                user.AppUserRoles = new List<AppUserRole>();
                user.AppUserRoles.Add(new AppUserRole
                {
                    AppUser = user,
                    AppRoleId = roleId
                });

                await UnitOfWork.Users.AddAsync(user);
                await UnitOfWork.CommitAsync();

                return CustomResponse<AppUserRegisterDto>.Success(201, dto);
            }
            return CustomResponse<AppUserRegisterDto>.Fail(400, $"'{dto.Username}' kullanıcı adı zaten kayıtlı!");
        }

        public async Task<CustomResponse<List<AppUserListDto>>> GetAllByActiveAsync()
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.AppUserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<AppUserListDto>(user)).ToList();

                return CustomResponse<List<AppUserListDto>>.Success(200, usersDto);
            }
            return CustomResponse<List<AppUserListDto>>.Fail(404, "Herhangi bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<AppUserListDto>> GetUserByIdAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId && x.IsActive && !x.IsDeleted, x => x.AppUserRoles);

            if (user != null)
            {
                var userDto = Mapper.Map<AppUserListDto>(user);

                return CustomResponse<AppUserListDto>.Success(200, userDto);
            }
            return CustomResponse<AppUserListDto>.Fail(404, "Gösterilecek bir kullanıcı bulunamadı!");
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

        public async Task<CustomResponse<List<AppUserListDto>>> GetAllByDeletedAsync()//Admin-Arşiv
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => x.IsDeleted, x => x.AppUserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<AppUserListDto>(user)).ToList();

                return CustomResponse<List<AppUserListDto>>.Success(200, usersDto);
            }
            return CustomResponse<List<AppUserListDto>>.Fail(404, "Silinmiş bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<List<AppUserListDto>>> GetAllByInactiveAsync()//Admin-Arşiv
        {
            var users = await UnitOfWork.Users.GetAllAsync(x => !x.IsActive & !x.IsDeleted, x => x.AppUserRoles);

            if (users.Any())
            {
                var usersDto = users.Select(user => Mapper.Map<AppUserListDto>(user)).ToList();

                return CustomResponse<List<AppUserListDto>>.Success(200, usersDto);
            }
            return CustomResponse<List<AppUserListDto>>.Fail(404, "Aktif olmayan bir kullanıcı bulunamadı!");
        }

        public async Task<CustomResponse<NoContent>> UpdateUserAsync(AppUserUpdateDto appUserUpdateDto)
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

            var updateUser = Mapper.Map<AppUserUpdateDto, AppUser>(appUserUpdateDto, oldUser);

            UnitOfWork.Users.Update(updateUser);

            await UnitOfWork.CommitAsync();

            if (isNewImageUploaded)
            {
                await _imageHelper.DeleteAsync(oldUserImage);
            }

            return CustomResponse<NoContent>.Success(204);
        }

        public async Task<CustomResponse<NoContent>> PasswordChangeAsync(AppUserPasswordChangeDto appUserPasswordChangeDto)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await UnitOfWork.Users.GetAsync(x => x.Id == int.Parse(userId));

            var isVerified = UnitOfWork.Users.CheckPasswordAsync(user, appUserPasswordChangeDto.CurrentPassword);

            if (isVerified)
            {
                var updatePassword = Mapper.Map<AppUserPasswordChangeDto, AppUser>(appUserPasswordChangeDto, user);
                UnitOfWork.Users.Update(updatePassword);
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

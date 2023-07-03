using AutoMapper;
using BlogApp.API.Filter;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class UserController : CustomControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserUpdateDto> _userUpdateDtoValidator;
        private readonly IValidator<UserPasswordChangeDto> _userPasswordChangeDtoValidator;
        private readonly IValidator<UserRegisterDto> _validator;
        private readonly IMapper _mapper;


        public UserController(IUserService userService, IValidator<UserUpdateDto> userUpdateDtoValidator, IValidator<UserPasswordChangeDto> userPasswordChangeDtoValidator, IMapper mapper, IValidator<UserRegisterDto> validator)
        {
            _userService = userService;
            _userUpdateDtoValidator = userUpdateDtoValidator;
            _userPasswordChangeDtoValidator = userPasswordChangeDtoValidator;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            var result = _validator.Validate(registerDto);

            if (result.IsValid)
            {
                var user = await _userService.RegisterWithRoleAsync(registerDto, (int)RoleType.Member);

                if (user.Errors.Any())//aynı kullanıcı adı kayıtlıysa girer
                {
                    return CreateActionResult(CustomResponseDto<UserDto>.Fail(user.StatusCode, user.Errors));
                }
                return CreateActionResult(CustomResponseDto<UserDto>.Success(user.StatusCode, user.Data));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByActive()
        {
            var users = await _userService.GetAllByActiveAsync();

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(users.StatusCode, users.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<UserListDto>>.Success(users.StatusCode, users.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(users.StatusCode, users.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<UserListDto>>.Success(users.StatusCode, users.Data));
        }

        [HttpGet("{userId}")]//api/user/1
        public async Task<IActionResult> GetUserById(int userId)
        {
            var users = await _userService.GetUserByIdAsync(userId);

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(users.StatusCode, users.Errors));
            }
            return CreateActionResult(CustomResponseDto<UserListDto>.Success(users.StatusCode, users.Data));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _userService.DeleteAsync(userId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponseDto<UserDto>.Success(result.StatusCode, result.Data));

            return CreateActionResult(CustomResponseDto<UserDto>.Fail(result.StatusCode, result.Errors));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> UndoDelete(int userId)
        {
            var result = await _userService.UndoDeleteAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<UserDto>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<UserDto>.Success(result.StatusCode, result.Data));
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> HardDelete(int userId)
        {
            var user = await _userService.HardDeleteAsync(userId);
            if (user.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<UserDto>.Fail(user.StatusCode, user.Errors));
            }
            return CreateActionResult(CustomResponseDto<UserDto>.Success(user.StatusCode, user.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var deletedUsers = await _userService.GetAllByDeletedAsync();
            if (deletedUsers.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<List<UserDto>>.Fail(deletedUsers.StatusCode, deletedUsers.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<UserDto>>.Success(deletedUsers.StatusCode, deletedUsers.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByInactive()
        {
            var users = await _userService.GetAllByInactiveAsync();

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(users.StatusCode, users.Errors));
            }
            return CreateActionResult(CustomResponseDto<List<UserListDto>>.Success(users.StatusCode, users.Data));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDto appUserUpdateDto)
        {
            var result = _userUpdateDtoValidator.Validate(appUserUpdateDto);

            if (result.IsValid)
            {
                var resultUpdate = await _userService.UpdateUserAsync(appUserUpdateDto);

                if (!resultUpdate.Errors.Any())
                {
                    return CreateActionResult(CustomResponseDto<UserDto>.Success(resultUpdate.StatusCode,resultUpdate.Data));
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<UserDto>.Fail(400, errors));
        }

        [HttpPut("[action]/{userId}")]
        //[CheckUserId]
        public async Task<IActionResult> PasswordChange(int userId, UserPasswordChangeDto userPasswordChangeDto)
        {
            var result = _userPasswordChangeDtoValidator.Validate(userPasswordChangeDto);

            if (result.IsValid)
            {
                var resultUpdate = await _userService.PasswordChangeAsync(userPasswordChangeDto, userId);

                return !resultUpdate.Errors.Any()
                    ? CreateActionResult(CustomResponseDto<NoContent>.Success(resultUpdate.StatusCode))
                    : CreateActionResult(CustomResponseDto<NoContent>.Fail(resultUpdate.StatusCode, resultUpdate.Errors));
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, errors));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var activateUser = await _userService.ActivateUserAsync(userId);
            if (activateUser.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<UserDto>.Fail(activateUser.StatusCode, activateUser.Errors));
            }
            return CreateActionResult(CustomResponseDto<UserDto>.Success(activateUser.StatusCode, activateUser.Data));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> DeleteUserImage(int userId)
        {
            var result = await _userService.DeleteUserImageAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<NoContent>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotal()
        {
            var result = await _userService.CountTotalAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<int>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeleted()
        {
            var result = await _userService.CountByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<int>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<int>.Success(result.StatusCode, result.Data));
        }
    }
}

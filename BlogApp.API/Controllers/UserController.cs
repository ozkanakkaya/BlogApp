using AutoMapper;
using BlogApp.API.Filter;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class UserController : CustomControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<AppUserUpdateDto> _userUpdateDtoValidator;
        private readonly IValidator<AppUserPasswordChangeDto> _userPasswordChangeDtoValidator;
        private readonly IMapper _mapper;


        public UserController(IUserService userService, IValidator<AppUserUpdateDto> userUpdateDtoValidator, IValidator<AppUserPasswordChangeDto> userPasswordChangeDtoValidator, IMapper mapper)
        {
            _userService = userService;
            _userUpdateDtoValidator = userUpdateDtoValidator;
            _userPasswordChangeDtoValidator = userPasswordChangeDtoValidator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByActive()
        {
            var users = await _userService.GetAllByActiveAsync();

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, users.Errors));
            }
            return CreateActionResult(CustomResponse<List<AppUserListDto>>.Success(200, users.Data));
        }

        [HttpGet("{userId}")]//api/blogs/1
        public async Task<IActionResult> GetUserById(int userId)
        {
            var users = await _userService.GetUserByIdAsync(userId);

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, users.Errors));
            }
            return CreateActionResult(CustomResponse<AppUserListDto>.Success(200, users.Data));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var result = await _userService.DeleteAsync(userId);

            if (!result.Errors.Any())
                return CreateActionResult(CustomResponse<NoContent>.Success(result.StatusCode));

            return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> UndoDelete(int userId)
        {
            var result = await _userService.UndoDeleteAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, result.Errors));
            }
            return CreateActionResult(CustomResponse<NoContent>.Success(result.StatusCode));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> HardDelete(int userId)
        {
            var user = await _userService.HardDeleteAsync(userId);
            if (user.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, user.Errors));
            }
            return CreateActionResult(CustomResponse<NoContent>.Success(user.StatusCode));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByDeleted()
        {
            var deletedUsers = await _userService.GetAllByDeletedAsync();
            if (deletedUsers.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(deletedUsers.StatusCode, deletedUsers.Errors));
            }
            return CreateActionResult(CustomResponse<List<AppUserListDto>>.Success(deletedUsers.StatusCode, deletedUsers.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllByInactive()
        {
            var users = await _userService.GetAllByInactiveAsync();

            if (users.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(404, users.Errors));
            }
            return CreateActionResult(CustomResponse<List<AppUserListDto>>.Success(200, users.Data));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] AppUserUpdateDto appUserUpdateDto)
        {
            var result = _userUpdateDtoValidator.Validate(appUserUpdateDto);

            if (result.IsValid)
            {
                var resultUpdate = await _userService.UpdateUserAsync(appUserUpdateDto);

                if (resultUpdate.StatusCode == 204)
                {
                    return CreateActionResult(CustomResponse<NoContent>.Success(204));
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        [HttpPost("[action]")]
        [CheckUserId]
        public async Task<IActionResult> PasswordChange(AppUserPasswordChangeDto appUserPasswordChangeDto)
        {
            var result = _userPasswordChangeDtoValidator.Validate(appUserPasswordChangeDto);

            if (result.IsValid)
            {
                var userId = HttpContext.Items["userId"] as string;

                var resultUpdate = await _userService.PasswordChangeAsync(appUserPasswordChangeDto, userId);

                return !resultUpdate.Errors.Any()
                    ? CreateActionResult(CustomResponse<NoContent>.Success(204))
                    : CreateActionResult(CustomResponse<NoContent>.Fail(400, resultUpdate.Errors));
            }

            result.Errors.ToList().ForEach(error => ModelState.AddModelError(error.PropertyName, error.ErrorMessage));

            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var activateUser = await _userService.ActivateUserAsync(userId);
            if (activateUser.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(activateUser.StatusCode, activateUser.Errors));
            }
            return CreateActionResult(CustomResponse<NoContent>.Success(activateUser.StatusCode, activateUser.Data));
        }

        [HttpPut("[action]/{userId}")]
        public async Task<IActionResult> DeleteUserImage(int userId)
        {
            var result = await _userService.DeleteUserImageAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<NoContent>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountTotal()
        {
            var result = await _userService.CountTotalAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CountByNonDeleted()
        {
            var result = await _userService.CountByNonDeletedAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<int>.Success(result.StatusCode, result.Data));
        }
    }
}

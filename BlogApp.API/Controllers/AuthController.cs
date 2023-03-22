using AutoMapper;
using BlogApp.API.Jwt;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class AuthController : CustomControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IValidator<UserRegisterDto> _validator;
        public AuthController(IMapper mapper, IUserService userService, IValidator<UserRegisterDto> validator, IRoleService roleService)
        {
            _mapper = mapper;
            _userService = userService;
            _validator = validator;
            _roleService = roleService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto registerDto)
        {
            var result = _validator.Validate(registerDto);

            if (result.IsValid)
            {
                var user = await _userService.RegisterWithRoleAsync(registerDto, (int)RoleType.Member);

                if (user.Errors.Any())//aynı kullanıcı adı kayıtlıysa girer
                {
                    return CreateActionResult(CustomResponseDto<UserRegisterDto>.Fail(user.StatusCode, user.Errors));
                }
                return CreateActionResult(CustomResponseDto<UserRegisterDto>.Success(user.StatusCode, user.Data));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponseDto<NoContent>.Fail(400, errors));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm] UserLoginDto loginDto)
        {
            var result = await _userService.CheckUserAsync(loginDto);
            if (!result.Errors.Any())
            {
                var roleResult = await _roleService.GetAllByUserIdAsync(result.Data.Id);
                var token = TokenGenerator.GenerateToken(result.Data, roleResult.Data);
				var cookieOptions = new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.Strict,
					Expires = DateTime.UtcNow.AddDays(1),
				};

				Response.Cookies.Append("access_token", token.Token, cookieOptions);

                //return Created("", token);
                return Ok(token);
            }
            return CreateActionResult(CustomResponseDto<CheckUserResponseDto>.Fail(result.StatusCode, result.Errors));
        }
    }
}

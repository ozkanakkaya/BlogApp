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
        private readonly IUserService _UserService;
        private readonly IValidator<UserRegisterDto> _validator;
        public AuthController(IMapper mapper, IUserService appUserService, IValidator<UserRegisterDto> validator)
        {
            _mapper = mapper;
            _UserService = appUserService;
            _validator = validator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromForm] UserRegisterDto registerDto)
        {
            var result = _validator.Validate(registerDto);

            if (result.IsValid)
            {
                var user = await _UserService.RegisterWithRoleAsync(registerDto, (int)RoleType.Member);

                if (user.Errors.Any())//aynı kullanıcı adı kayıtlıysa girer
                {
                    return CreateActionResult(CustomResponse<UserRegisterDto>.Fail(user.StatusCode, user.Errors));
                }
                return CreateActionResult(CustomResponse<UserRegisterDto>.Success(user.StatusCode, user.Data));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var result = _UserService.CheckUser(loginDto);
            if (!result.Errors.Any())
            {
                var roleResult = await _UserService.GetRolesByUserId(result.Data.Id);
                var token = TokenGenerator.GenerateToken(result.Data, roleResult.Data);
                return Created("", token);
            }
            return CreateActionResult(CustomResponse<CheckUserResponseDto>.Fail(result.StatusCode, result.Errors));
        }
    }
}

using BlogApp.Business.Jwt;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class AuthController : CustomControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly TokenGenerator _tokenGenerator;

        public AuthController(IUserService userService, IRoleService roleService, TokenGenerator tokenGenerator)
        {
            _userService = userService;
            _roleService = roleService;
            _tokenGenerator = tokenGenerator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(UserLoginDto loginDto)
        {
            var result = await _userService.CheckUserAsync(loginDto);
            if (!result.Errors.Any())
            {
                var roleResult = await _roleService.GetAllByUserIdAsync(result.Data.Id);
                var token = _tokenGenerator.GenerateToken(result.Data, roleResult.Data);

                return CreateActionResult(CustomResponseDto<TokenResponse>.Success(result.StatusCode, token));

                //return Created("", token);
            }
            return CreateActionResult(CustomResponseDto<string>.Fail(result.StatusCode, result.Errors));
        }
    }
}

using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Services;
using BlogApp.Core.Utilities.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class AuthController : CustomBaseController
    {
        private readonly IMapper _mapper;

        private readonly IAppUserService _appUserService;

        public AuthController(IMapper mapper, IAppUserService appUserService)
        {
            _mapper = mapper;
            _appUserService = appUserService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(AppUserRegisterDto registerDto)
        {
            var user = await _appUserService.RegisterWithRoleAsync(registerDto, (int)RoleType.Member);
            return CreateActionResult(CustomResponse<AppUserRegisterDto>.Success(201, registerDto));
        }
    }
}

using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Services;
using BlogApp.Core.Utilities.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class AuthController : CustomBaseController
    {
        private readonly IMapper _mapper;

        private readonly IAppUserService _appUserService;

        private readonly IValidator<AppUserRegisterDto> _validator;

        public AuthController(IMapper mapper, IAppUserService appUserService, IValidator<AppUserRegisterDto> validator)
        {
            _mapper = mapper;
            _appUserService = appUserService;
            _validator = validator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(AppUserRegisterDto registerDto)
        {
            var result = _validator.Validate(registerDto);

            if (result.IsValid)
            {
                var user = await _appUserService.RegisterWithRoleAsync(registerDto, (int)RoleType.Member);
                return CreateActionResult(CustomResponse<AppUserRegisterDto>.Success(201, registerDto));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }
    }
}

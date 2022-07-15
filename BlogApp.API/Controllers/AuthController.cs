using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
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

                var a = user.Errors.Any();

                if (user.Errors.Any())//kullanıcı adı kayıtlıysa girer
                {
                    user.Errors.ForEach(x =>
                    {
                        ModelState.AddModelError(String.Empty, x);
                    });

                    var errorsResult = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                    return CreateActionResult(CustomResponse<AppUserRegisterDto>.Fail(400, errorsResult));
                }
                return CreateActionResult(user);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            var errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();

            return CreateActionResult(CustomResponse<NoContent>.Fail(400, errors));
        }

        //[HttpPost("[action]")]
        //public Task<IActionResult> Login(AppUserLoginDto registerDto)
        //{

        //}
    }
}

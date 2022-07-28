using BlogApp.Core.DTOs.Concrete.AppUserDtos;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class AppUserLoginDtoValidator : AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen parolanızı giriniz.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Lütfen kullanıcı adınızı giriniz.");
        }
    }
}

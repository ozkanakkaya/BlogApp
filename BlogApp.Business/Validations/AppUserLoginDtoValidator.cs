using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class AppUserLoginDtoValidator : AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen kullanıcı adını giriniz.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Lütfen parolanızı giriniz.");
        }
    }
}

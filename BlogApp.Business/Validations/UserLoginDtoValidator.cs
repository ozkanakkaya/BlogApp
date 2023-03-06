using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen parolanızı giriniz.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Lütfen kullanıcı adınızı giriniz.");
        }
    }
}

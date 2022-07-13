using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class AppUserRegisterValidator : AbstractValidator<AppUserRegisterDto>
    {
        public AppUserRegisterValidator()
        {
            RuleFor(x => x.Firstname).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.").NotEmpty().WithMessage("Ad alanı boş geçilemez.");
            RuleFor(x => x.Lastname).MinimumLength(2).WithMessage("Soyad alanı en az 2 karakter olmalıdır.").NotEmpty().WithMessage("Soyad alanı boş geçilemez.");
            RuleFor(x => x.Username).MinimumLength(2).WithMessage("Kullanıcı adı en az 2 karakter olmalıdır.").NotEmpty().WithMessage("Kullanıcı adı boş geçilemez.");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Parola alanı en az 6 karakter olmalıdır.").NotEmpty().MinimumLength(10).WithMessage("Parola alanı boş geçilemez.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Doğru bir e-posta adresi giriniz.").NotEmpty().WithMessage("E-posta alanı boş geçilemez.");
            RuleFor(x => x.GenderId).NotEmpty().WithMessage("Cinsiyet alanı boş geçilemez.");
        }
    }
}

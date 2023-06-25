using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterDtoValidator()
        {
            RuleFor(x => x.Firstname).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.").NotEmpty().WithMessage("Ad alanı boş geçilemez.").WithName("Ad");
            RuleFor(x => x.Lastname).MinimumLength(2).WithMessage("Soyad alanı en az 2 karakter olmalıdır.").NotEmpty().WithMessage("Soyad alanı boş geçilemez.").WithName("Soyad");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Kullanıcı adı boş geçilemez.").MinimumLength(2).WithMessage("Kullanıcı adı en az 2 karakter olmalıdır.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola alanı boş geçilemez.").MinimumLength(6).WithMessage("Parola alanı en az 6 karakter olmalıdır.");
            RuleFor(x => x.About).NotEmpty().WithMessage("Hakkında alanı boş geçilemez.").MinimumLength(10).WithMessage("Hakkında alanı en az 10 karakter olmalıdır.");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Parola tekrar alanı boş geçilemez.").Equal(x => x.Password).WithMessage("Parolalar uyuşmuyor!").MinimumLength(6).WithMessage("Parola tekrar alanı en az 6 karakter olmalıdır.");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Doğru bir e-posta adresi giriniz.").NotEmpty().WithMessage("E-posta alanı boş geçilemez.");
            RuleFor(x => x.GenderId).NotEmpty().WithMessage("Cinsiyet alanı boş geçilemez.");
        }
    }
}

using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class UserPasswordChangeDtoValidator : AbstractValidator<UserPasswordChangeDto>
    {
        public UserPasswordChangeDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                .MinimumLength(6).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(30).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Mevcut Parola")
                ;
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                .MinimumLength(6).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(30).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Yeni Parola")
                ;

            RuleFor(x => x.RepeatPassword)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                //.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$")
                //.WithMessage("Parola alanı en az 6 karakter, büyük ve küçük harf, sayı ve özel karakter içermelidir.")
                .MinimumLength(6).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(30).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .Equal(x => x.NewPassword).WithMessage("'Yeni Parola' ve '{PropertyName}' alanları uyuşmuyor!")
                .WithName("Yeni Parola Tekrar")
                ;
        }
    }
}

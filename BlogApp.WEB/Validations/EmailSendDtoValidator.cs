using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.WEB.Validations
{
    public class EmailSendDtoValidator : AbstractValidator<EmailSendDto>
    {
        public EmailSendDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .MinimumLength(5).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(60).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("İsim");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .MinimumLength(5).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(100).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .EmailAddress().WithMessage("Doğru bir e-posta adresi giriniz.")
                .WithName("E-Posta");
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .MinimumLength(5).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(120).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Konu");
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .MinimumLength(5).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(1500).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Mesaj");
        }
    }
}

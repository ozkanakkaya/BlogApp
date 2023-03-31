using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class CommentUpdateDtoValidator : AbstractValidator<CommentUpdateDto>
    {
        public CommentUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.");
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                .MinimumLength(2).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(1000).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Kategori Adı");
        }
    }
}

using BlogApp.Core.DTOs.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Validations
{
    public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.");
            RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                    .MinimumLength(3).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                    .MaximumLength(100).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                    .WithName("Etiket Adı");
            RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                    .MinimumLength(3).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                    .MaximumLength(500).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                    .WithName("Etiket Açıklaması");
        }
    }
}

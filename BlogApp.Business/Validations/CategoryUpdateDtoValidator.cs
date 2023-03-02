﻿using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                .MinimumLength(3).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(100).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Kategori Adı");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("'{PropertyName}' alanı boş geçilemez.")
                .MinimumLength(3).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(500).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Kategori Açıklaması");
        }
    }
}

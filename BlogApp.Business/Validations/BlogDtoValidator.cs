﻿using BlogApp.Core.DTOs.Concrete.BlogDtos;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class BlogDtoValidator : AbstractValidator<BlogDto>
    {
        public BlogDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen başlık giriniz.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Lütfen içerik giriniz.");
            RuleFor(x => x.Tags).NotEmpty().WithMessage("Lütfen blog içeriğini açıklayan etiket(leri) giriniz.");
            RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Lütfen bloğun ait olduğunu kategori(leri) seçiniz.");
        }
    }
}

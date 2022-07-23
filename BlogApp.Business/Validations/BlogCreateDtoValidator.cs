using BlogApp.Core.DTOs.Concrete.BlogDtos;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen başlık giriniz.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Lütfen içerik giriniz.");
        }
    }
}

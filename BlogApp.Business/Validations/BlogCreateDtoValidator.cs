using BlogApp.Core.DTOs.Concrete;
using FluentValidation;

namespace BlogApp.Business.Validations
{
    public class BlogCreateDtoValidator : AbstractValidator<BlogCreateDto>
    {
        public BlogCreateDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Lütfen başlık giriniz.");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Lütfen içerik giriniz.");
            RuleFor(x => x.Tags).NotEmpty().WithMessage("Lütfen blog içeriğini açıklayan etiket(leri) giriniz.");
            RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("Lütfen bloğun ait olduğunu kategori(leri) seçiniz.");
        }
    }
}

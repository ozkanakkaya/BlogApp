using BlogApp.WEB.Configurations;
using FluentValidation;

namespace BlogApp.WEB.Validations
{
    public class BlogRightSideBarWidgetOptionsValidator : AbstractValidator<BlogRightSideBarWidgetOptions>
    {
        public BlogRightSideBarWidgetOptionsValidator()
        {
            RuleFor(x => x.Header)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .MinimumLength(5).WithMessage("'{PropertyName}' alanı en az {MinLength} karakter olmalıdır.")
                .MaximumLength(150).WithMessage("'{PropertyName}' alanı en fazla {MaxLength} karakter olmalıdır.")
                .WithName("Widget Başlığı");

            RuleFor(x => x.TakeSize)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .InclusiveBetween(0, 20).WithMessage("'{PropertyName}' alanına {From} - {To} arası değer girilebilir.")
                .WithName("Blog Sayısı");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Kategori");

            RuleFor(x => x.FilterBy)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Filtre Türü");

            RuleFor(x => x.OrderBy)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Sıralama Türü");

            RuleFor(x => x.IsAscending)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Sıralama Ölçütü");

            RuleFor(x => x.StartAt)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .Must(BeAValidDate).WithMessage("'{PropertyName}' alanı geçerli bir tarih formatında olmalıdır.")
                .WithName("Başlangıç Tarihi");

            RuleFor(x => x.EndAt)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .Must(BeAValidDate).WithMessage("'{PropertyName}' alanı geçerli bir tarih formatında olmalıdır")
                .WithName("Bitiş Tarihi");

            RuleFor(x => x.MaxViewCount)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Maksimum Okuma Sayısı");

            RuleFor(x => x.MinViewCount)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Minimum Okuma Sayısı");

            RuleFor(x => x.MaxCommentCount)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Maksimum Yorum Sayısı");

            RuleFor(x => x.MinCommentCount)
                .NotEmpty().WithMessage("'{PropertyName}' alanı zorunludur.")
                .WithName("Minimum Yorum Sayısı");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}

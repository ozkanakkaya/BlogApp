using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
    {
        public void Configure(EntityTypeBuilder<BlogCategory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasIndex(x => new
            {
                x.BlogId,
                x.CategoryId
            }).IsUnique();

            builder.HasOne(x => x.Category).WithMany(x => x.BlogCategories).HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.Blog).WithMany(x => x.BlogCategories).HasForeignKey(x => x.BlogId);
        }
    }
}

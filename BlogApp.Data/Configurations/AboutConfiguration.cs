using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class AboutConfiguration : IEntityTypeConfiguration<About>
    {
        public void Configure(EntityTypeBuilder<About> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Content).HasMaxLength(1500).IsRequired();
        }
    }
}

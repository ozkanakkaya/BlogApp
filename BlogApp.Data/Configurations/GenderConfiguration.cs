using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Definition).HasMaxLength(20).IsRequired();
            builder.HasData(new Gender[]
            {
                new()
                {
                    Id=1,
                    Definition="Man"
                },
                new()
                {
                    Id=2,
                    Definition="Woman"
                },
                new()
                {
                    Id=3,
                    Definition="Undefined"
                }
            });
        }
    }
}

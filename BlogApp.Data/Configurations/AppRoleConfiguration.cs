using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Definition).HasMaxLength(300).IsRequired();
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(a => a.CreatedByUsername).HasMaxLength(50).IsRequired();
            builder.Property(a => a.UpdatedByUsername).HasMaxLength(50).IsRequired();
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.UpdatedDate).IsRequired();

            builder.HasData(new AppRole[]
            {
                new()
                {
                    Definition="Admin",
                    Id=1
                },
                new()
                {
                    Definition="Member",
                    Id=2
                },
                new()
                {
                    Definition="Author",
                    Id=3
                }
            });
        }
    }
}

using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Firstname).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Lastname).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(300).IsRequired();
            builder.HasIndex(x => x.Username).IsUnique();//benzersiz
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.Password).HasMaxLength(50).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(250).IsRequired();
            builder.Property(x => x.About).HasMaxLength(1000);
            builder.Property(x => x.GitHubLink).HasMaxLength(250);
            builder.Property(x => x.WebsiteLink).HasMaxLength(250);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(a => a.CreatedByUsername).HasMaxLength(50).IsRequired();
            builder.Property(a => a.UpdatedByUsername).HasMaxLength(50).IsRequired();
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.UpdatedDate).IsRequired();

            builder.HasOne(x => x.Gender).WithMany(x => x.AppUsers).HasForeignKey(x => x.GenderId);
        }
    }
}

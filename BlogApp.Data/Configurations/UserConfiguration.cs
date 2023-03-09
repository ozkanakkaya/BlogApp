using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.Firstname).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Lastname).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Username).HasMaxLength(100).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(100).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(20);
            builder.Property(x => x.PasswordHash).HasMaxLength(200).IsRequired();
            builder.Property(x => x.ImageUrl).HasMaxLength(500);
            builder.Property(x => x.About).HasMaxLength(1000);
            builder.Property(x => x.GitHubLink).HasMaxLength(250);
            builder.Property(x => x.WebsiteLink).HasMaxLength(250);
            builder.Property(x => x.IsActive).IsRequired();
            builder.Property(x => x.IsDeleted).IsRequired();
            builder.Property(a => a.CreatedByUsername).HasMaxLength(50).IsRequired();
            builder.Property(a => a.UpdatedByUsername).HasMaxLength(50).IsRequired();
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.UpdatedDate).IsRequired();

            builder.HasOne(x => x.Gender).WithMany(x => x.Users).HasForeignKey(x => x.GenderId);
        }
    }
}

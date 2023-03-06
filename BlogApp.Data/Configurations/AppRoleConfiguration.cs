using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Data;

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
                   Id = 1,
                   Definition = "Category.Create",
               },
               new()
               {
                   Id = 2,
                   Definition = "Category.Read",
               },
               new()
               {
                   Id = 3,
                   Definition = "Category.Update",
               }
                ,
               new()
               {
                   Id = 4,
                   Definition = "Category.Delete",
               },
               new()
               {
                   Id = 5,
                   Definition = "Blog.Create",
               }
                    ,
               new()
               {
                   Id = 6,
                   Definition = "Blog.Read",
               },
               new()
               {
                   Id = 7,
                   Definition = "Blog.Update",
               }
                   ,
               new()
               {
                   Id = 8,
                   Definition = "Blog.Delete",
               }
                ,
               new()
               {
                   Id = 9,
                   Definition = "User.Create",
               }
                ,
               new()
               {
                   Id = 10,
                   Definition = "User.Read",
               },
               new()
               {
                   Id = 11,
                   Definition = "User.Update",
               }
                ,
               new()
               {
                   Id = 12,
                   Definition = "User.Delete",
               }
                ,
               new()
               {
                   Id = 13,
                   Definition = "Role.Create",
               }
               ,
               new()
               {
                   Id = 14,
                   Definition = "Role.Read",
               },
               new()
               {
                   Id = 15,
                   Definition = "Role.Update",
               }
               ,
               new()
               {
                   Id = 16,
                   Definition = "Role.Delete",
               }
               ,
               new()
               {
                   Id = 17,
                   Definition = "Comment.Create",
               }
               ,
               new()
               {
                   Id = 18,
                   Definition = "Comment.Read",
               },
               new()
               {
                   Id = 19,
                   Definition = "Comment.Update",
               }
               ,
               new()
               {
                   Id = 20,
                   Definition = "Comment.Delete",
               },
               new()
               {
                   Id = 21,
                   Definition = "Member",
               },
               new()
               {
                   Id = 22,
                   Definition = "Admin",
               },
               new()
               {
                   Id = 23,
                   Definition = "SuperAdmin",
               }
            });
        }
    }
}

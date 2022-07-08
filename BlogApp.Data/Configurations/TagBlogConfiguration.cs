using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.Data.Configurations
{
    public class TagBlogConfiguration : IEntityTypeConfiguration<TagBlog>
    {
        public void Configure(EntityTypeBuilder<TagBlog> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.HasIndex(x => new
            {
                x.TagId,
                x.BlogId
            });

            builder.HasOne(x => x.Tag).WithMany(x => x.TagBlogs).HasForeignKey(x => x.TagId);
            builder.HasOne(x => x.Blog).WithMany(x => x.TagBlogs).HasForeignKey(x => x.BlogId);
        }
    }
}

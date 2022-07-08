using BlogApp.Core.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BlogApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<About> About { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagBlog> TagBlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//entity configurationlarını uygular
            base.OnModelCreating(modelBuilder);
        }
    }
}

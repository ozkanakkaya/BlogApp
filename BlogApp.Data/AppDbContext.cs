using BlogApp.Core.Entities.Abstract;
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
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagBlog> TagBlogs { get; set; }


        //public override int SaveChanges()
        //{
        //    foreach (var item in ChangeTracker.Entries())
        //    {
        //        if (item.Entity is AppUser entityReference)
        //        {
        //            switch (item.State)
        //            {
        //                case EntityState.Added:
        //                    {
        //                        entityReference.CreatedDate = DateTime.Now;
        //                        entityReference.CreatedByUsername = entityReference.Username;
        //                        break;
        //                    }
        //                case EntityState.Modified:
        //                    {
        //                        Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
        //                        Entry(entityReference).Property(x => x.CreatedByUsername).IsModified = false;

        //                        entityReference.UpdatedDate = DateTime.Now;
        //                        entityReference.CreatedByUsername = entityReference.Username;
        //                        break;
        //                    }
        //            }
        //        }
        //    }
        //    return base.SaveChanges();
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                var username = "Admin";
                if (item.Entity is AppUser entityReference1)
                {
                    username = entityReference1.Username;
                }

                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                entityReference.CreatedByUsername = username;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                Entry(entityReference).Property(x => x.CreatedByUsername).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now;
                                entityReference.UpdatedByUsername = username;
                                break;
                            }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//entity configurationlarını uygular
            base.OnModelCreating(modelBuilder);
        }
    }
}

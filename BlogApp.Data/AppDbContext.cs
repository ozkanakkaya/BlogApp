using BlogApp.Core.Entities.Abstract;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Data.Seed;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;

namespace BlogApp.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<About> About { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogTag> BlogTags { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    var username = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                    var methodName = httpContext.Request.Path.Value.Split("/").Last();

                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.CreatedDate = DateTime.Now;
                                entityReference.UpdatedDate = entityReference.CreatedDate;
                                if (username != null) entityReference.CreatedByUsername = username;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                if (methodName == "IncreaseViewCount") break;

                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                Entry(entityReference).Property(x => x.CreatedByUsername).IsModified = false;
                                entityReference.UpdatedDate = DateTime.Now;
                                if (username != null) entityReference.UpdatedByUsername = username;
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
            modelBuilder.SeedData();
        }
    }
}

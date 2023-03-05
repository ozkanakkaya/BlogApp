using BlogApp.Core.Entities.Abstract;
using BlogApp.Core.Entities.Concrete;
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
                                entityReference.CreatedDate = DateTime.UtcNow;
                                entityReference.UpdatedDate = entityReference.CreatedDate;
                                if(username != null) entityReference.CreatedByUsername = username;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                if (methodName == "IncreaseViewCount") break;

                                Entry(entityReference).Property(x => x.CreatedDate).IsModified = false;
                                Entry(entityReference).Property(x => x.CreatedByUsername).IsModified = false;
                                entityReference.UpdatedDate = DateTime.UtcNow;
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
        }
    }
}

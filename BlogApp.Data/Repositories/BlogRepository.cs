using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogApp.Data.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Blog> GetBlogById(int blogId)
        {
            return await _context.Blogs.Where(x => x.Id == blogId).Include(x => x.BlogCategories).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).FirstOrDefaultAsync();
        }

        public async Task<IList<Blog>> GetBlogsByDetailsAsync(int? categoryId = null, int? userId = null, int? blogId = null, bool? isActive = true, bool? isDeleted = false)
        {
            return await _context.Blogs.Where(
                x => isActive == true || false ? x.IsActive == isActive : true
                && x.IsDeleted == true || false ? x.IsDeleted == isDeleted : true
                && categoryId != null ? x.BlogCategories.Any(x => x.CategoryId == categoryId.Value) : true
                && userId != null ? x.AppUserId == userId : true
                && blogId != null ? x.Id == blogId : true)
                .Include(x => x.BlogCategories).ThenInclude(x => x.Category)
                .Include(x => x.TagBlogs).ThenInclude(x => x.Tag)
                .Include(x => x.AppUser)
                .ToListAsync();
        }

        public async Task<IList<Blog>> SearchAsync(IList<Expression<Func<Blog, bool>>> expressions, params Expression<Func<Blog, object>>[] includeProperties)
        {
            IQueryable<Blog> query = _context.Blogs.Where(x => x.IsActive && !x.IsDeleted);
            if (expressions.Any())
            {
                var predicateChain = PredicateBuilder.New<Blog>();

                foreach (var expression in expressions)
                {
                    predicateChain.Or(expression);
                }
                query = query.Where(predicateChain);
            }

            if (includeProperties.Any())
            {
                foreach (var includeProperty in includeProperties)
                {
                    switch (includeProperty.ToString())
                    {
                        case string x when x.Contains("BlogCategories"):
                            query = query.Include(x => x.BlogCategories).ThenInclude(x => x.Category);
                            break;
                        case string x when x.Contains("TagBlogs"):
                            query = query.Include(x => x.TagBlogs).ThenInclude(x => x.Tag);
                            break;
                        default:
                            query = query.Include(includeProperty);
                            break;
                    }
                }
            }
            return await query.AsNoTracking().ToListAsync();
        }
    }
}

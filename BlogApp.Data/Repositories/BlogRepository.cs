using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Blog> GetBlogById(int blogId)
        {
            return await _context.Blogs.Where(x => x.Id == blogId).Include(x => x.BlogCategories).Include(x => x.BlogTags).ThenInclude(x => x.Tag).FirstOrDefaultAsync();
        }
    }
}

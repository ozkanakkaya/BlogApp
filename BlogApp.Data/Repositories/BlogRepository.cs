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

        public Blog GetBlogById(int blogId)
        {
            return _context.Blogs.Where(x => x.Id == blogId).Include(x => x.BlogCategories).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).FirstOrDefault();
        }

        public List<Blog> GetAllByNonDeletedAndActive()
        {
            return _context.Blogs.Where(x => x.IsActive && !x.IsDeleted).Include(x => x.BlogCategories).ThenInclude(x => x.Category).Include(x => x.TagBlogs).ThenInclude(x => x.Tag).ToList();
        }
    }
}

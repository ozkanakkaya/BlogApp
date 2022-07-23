using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;

namespace BlogApp.Data.Repositories
{
    public class BlogRepository : GenericRepository<Blog>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }


    }
}

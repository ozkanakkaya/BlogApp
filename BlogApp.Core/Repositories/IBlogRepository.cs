using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Blog GetBlogById(int blogId);
    }
}

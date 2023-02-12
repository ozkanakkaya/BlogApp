using BlogApp.Core.Entities.Concrete;
using System.Linq.Expressions;

namespace BlogApp.Core.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Task<Blog> GetBlogById(int blogId);
    }
}

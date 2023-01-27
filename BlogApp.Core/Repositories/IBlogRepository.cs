using BlogApp.Core.Entities.Concrete;
using System.Linq.Expressions;

namespace BlogApp.Core.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Blog GetBlogById(int blogId);
        List<Blog> GetAllByNonDeletedAndActive();
        Task<IList<Blog>> SearchAsync(IList<Expression<Func<Blog, bool>>> expressions, params Expression<Func<Blog, object>>[] includeProperties);
    }
}

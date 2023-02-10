using BlogApp.Core.Entities.Concrete;
using System.Linq.Expressions;

namespace BlogApp.Core.Repositories
{
    public interface IBlogRepository : IGenericRepository<Blog>
    {
        Blog GetBlogById(int blogId);
        Task<IList<Blog>> GetBlogsByDetailsAsync(int? categoryId = null, int? userId = null, int? blogId = null, bool? isActive = true, bool? isDeleted = false);
        Task<IList<Blog>> SearchAsync(IList<Expression<Func<Blog, bool>>> expressions, params Expression<Func<Blog, object>>[] includeProperties);
    }
}

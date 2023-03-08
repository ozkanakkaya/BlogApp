using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IRoleRepository : IGenericRepository<AppRole>
    {
        Task<IList<string>> GetRolesByUserIdAsync(int userId);
    }
}

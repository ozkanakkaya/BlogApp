using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<IList<string>> GetRolesByUserIdAsync(int userId);
    }
}

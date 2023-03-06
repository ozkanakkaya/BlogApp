using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IRoleRepository : IGenericRepository<AppRole>
    {
        List<AppRole> GetRolesByUserId(int userId);
    }
}

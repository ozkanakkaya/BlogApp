using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IAppRoleRepository : IGenericRepository<AppRole>
    {
        List<AppRole> GetRolesByUserId(int userId);
    }
}

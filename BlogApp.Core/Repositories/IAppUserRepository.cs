using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IAppUserRepository : IGenericRepository<AppUser>
    {
        AppUser GetAppUserWithLoginInfo(string username, string password);
    }
}
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<AppUser>
    {
        AppUser GetAppUserWithLoginInfo(string username, string password);
        bool CheckPasswordAsync(AppUser user, string currentPassword);
    }
}
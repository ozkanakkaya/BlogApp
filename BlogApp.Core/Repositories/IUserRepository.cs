using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetAppUserWithLoginInfo(string username, string password);
        bool CheckPasswordAsync(User user, string currentPassword);
    }
}
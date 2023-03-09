using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;

namespace BlogApp.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public bool CheckPasswordAsync(User user, string currentPassword)
        {
            return _context.Users.Any(x => x.Username == user.Username && x.PasswordHash == currentPassword);
        }

        public User GetAppUserWithLoginInfo(string username, string password)
        {
            return _context.Users.Where(x => x.Username == username && x.PasswordHash == password)/*.Include(x=>x.AppUserRoles)*/.SingleOrDefault();
        }
    }
}

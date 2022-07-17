using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;

namespace BlogApp.Data.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(AppDbContext context) : base(context)
        {
        }

        public AppUser GetAppUserWithLoginInfo(string username, string password)
        {
            return _context.AppUsers.Where(x => x.Username == username && x.Password == password)/*.Include(x=>x.AppUserRoles)*/.SingleOrDefault();
        }
    }
}

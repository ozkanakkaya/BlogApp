using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;

namespace BlogApp.Data.Repositories
{
    public class RoleRepository : GenericRepository<AppRole>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
        public List<AppRole> GetRolesByUserId(int userId)
        {
            return _context.AppRoles.Where(x => x.AppUserRoles.Any(x => x.AppUserId == userId)).ToList();
        }
    }
}

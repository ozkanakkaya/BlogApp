using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class RoleRepository : GenericRepository<AppRole>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IList<string>> GetRolesByUserIdAsync(int userId)
        {
            return await _context.AppRoles
               .Where(role => role.AppUserRoles.Any(ur => ur.AppUserId == userId))
               .Select(role => role.Definition)
               .ToListAsync();
        }

    }
}

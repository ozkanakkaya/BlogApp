using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<IList<string>> GetRolesByUserIdAsync(int userId)
        {
            return await _context.Roles
               .Where(role => role.UserRoles.Any(ur => ur.UserId == userId))
               .Select(role => role.Name)
               .ToListAsync();
        }

    }
}

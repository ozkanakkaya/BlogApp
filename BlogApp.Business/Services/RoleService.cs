using AutoMapper;
using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Enums;
using BlogApp.Core.Repositories;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using BlogApp.Core.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Business.Services
{
    public class RoleService : Service<AppRole>, IRoleService
    {
        public RoleService(IGenericRepository<AppRole> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }
        public async Task<CustomResponse<RoleListDto>> GetAllRolesAsync()
        {
            var roles = await UnitOfWork.Roles.GetAllAsync(null);

            if (roles.Any())
            {
                return CustomResponse<RoleListDto>.Success(200, new RoleListDto
                {
                    Roles = Mapper.Map<IList<RoleDto>>(roles)
                });
            }
            return CustomResponse<RoleListDto>.Fail(404, "Bir role bulunamadı!");
        }

        public async Task<CustomResponse<RoleListDto>> GetAllByUserIdAsync(int userId)
        {
            var userRoles = await UnitOfWork.Roles.GetAllAsync(x => x.AppUserRoles.Any(x => x.AppUserId == userId));

            if (userRoles == null)
            {
                return CustomResponse<RoleListDto>.Fail(404, $"Id: {userId} kullanıcısının rolleri bulunamadı!");
            }
            return CustomResponse<RoleListDto>.Success(200, new RoleListDto
            {
                Roles = Mapper.Map<IList<RoleDto>>(userRoles)
            });
        }

        public async Task<CustomResponse<UserRoleAssignDto>> GetUserRoleAssignDtoAsync(int userId)
        {
            var user = await UnitOfWork.Users.GetAsync(x => x.Id == userId);
            var roles = await UnitOfWork.Roles.GetAllAsync(null);
            var userRoles = await UnitOfWork.Roles.GetRolesByUserIdAsync(userId);

            UserRoleAssignDto userRoleAssignDto = new()
            {
                UserId = user.Id,
                Username = user.Username
            };

            foreach (var role in roles)
            {
                var aea = role.Definition;
                RoleAssignDto roleAssignDto = new()
                {
                    RoleId = role.Id,
                    RoleName = role.Definition,
                    HasRole = userRoles.Contains(role.Definition)
                };
                userRoleAssignDto.RoleAssignments.Add(roleAssignDto);
            }
            return CustomResponse<UserRoleAssignDto>.Success(200, userRoleAssignDto);
        }

        public async Task<CustomResponse<UserRoleAssignDto>> AssignAsync(UserRoleAssignDto userRoleAssignDto)
        {
            var user = await UnitOfWork.Users.Where(x => x.Id == userRoleAssignDto.UserId).Include(x => x.AppUserRoles).SingleOrDefaultAsync();

            var userNewRoles = new HashSet<int>(
                userRoleAssignDto.RoleAssignments
                .Where(x => x.HasRole)
                .Select(x => x.RoleId).ToList()
            );

            if (userNewRoles.Count <= 0)
            {
                user.AppUserRoles.Clear();
                user.AppUserRoles.Add(new AppUserRole
                {
                    AppRoleId = (int)RoleType.Member//default role
                });
            }
            else
            {
                var userOldRoles = new HashSet<int>(
                await UnitOfWork.UserRoles.Where(x => x.AppUserId == user.Id)
                .Select(x => x.AppRoleId)
                .ToListAsync()
);
                if (!userNewRoles.SetEquals(userOldRoles))
                {
                    user.AppUserRoles.Clear();
                    foreach (var roleId in userNewRoles)
                    {
                        user.AppUserRoles.Add(new AppUserRole
                        {
                            AppRoleId = roleId
                        });
                    }
                }
            }
            UnitOfWork.Users.Update(user);
            await UnitOfWork.CommitAsync();
            return CustomResponse<UserRoleAssignDto>.Success(200, userRoleAssignDto);

        }
    }
}

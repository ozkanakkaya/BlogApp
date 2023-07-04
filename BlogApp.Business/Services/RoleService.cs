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
    public class RoleService : Service<Role>, IRoleService
    {
        public RoleService(IGenericRepository<Role> repository, IUnitOfWork unitOfWork, IMapper mapper) : base(repository, unitOfWork, mapper)
        {
        }
        public async Task<CustomResponseDto<RoleListDto>> GetAllRolesAsync()
        {
            var roles = await UnitOfWork.Roles.GetAllAsync(null);

            if (roles.Any())
            {
                return CustomResponseDto<RoleListDto>.Success(200, new RoleListDto
                {
                    Roles = Mapper.Map<IList<RoleDto>>(roles)
                });
            }
            return CustomResponseDto<RoleListDto>.Fail(200, "Bir role bulunamadı!");
        }

        public async Task<CustomResponseDto<RoleListDto>> GetAllByUserIdAsync(int userId)
        {
            var userRoles = await UnitOfWork.Roles.GetAllAsync(x => x.UserRoles.Any(x => x.UserId == userId));

            if (userRoles == null)
            {
                return CustomResponseDto<RoleListDto>.Fail(404, $"Id: {userId} kullanıcısının rolleri bulunamadı!");
            }
            return CustomResponseDto<RoleListDto>.Success(200, new RoleListDto
            {
                Roles = Mapper.Map<IList<RoleDto>>(userRoles)
            });
        }

        public async Task<CustomResponseDto<UserRoleAssignDto>> GetUserRoleAssignDtoAsync(int userId)
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
                RoleAssignDto roleAssignDto = new()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name)
                };
                userRoleAssignDto.RoleAssignDtos.Add(roleAssignDto);
            }
            return CustomResponseDto<UserRoleAssignDto>.Success(200, userRoleAssignDto);
        }

        public async Task<CustomResponseDto<UserDto>> AssignAsync(UserRoleAssignDto userRoleAssignDto)
        {
            var user = await UnitOfWork.Users.Where(x => x.Id == userRoleAssignDto.UserId).Include(x => x.UserRoles).SingleOrDefaultAsync();

            var userNewRoles = new HashSet<int>(
                userRoleAssignDto.RoleAssignDtos
                .Where(x => x.HasRole)
                .Select(x => x.RoleId).ToList()
            );

            if (userNewRoles.Count <= 0)
            {
                user.UserRoles.Clear();
                user.UserRoles.Add(new UserRole
                {
                    RoleId = (int)RoleType.Member//default role
                });
            }
            else
            {
                var userOldRoles = new HashSet<int>(
                await UnitOfWork.UserRoles.Where(x => x.UserId == user.Id)
                .Select(x => x.RoleId)
                .ToListAsync()
);
                if (!userNewRoles.SetEquals(userOldRoles))
                {
                    user.UserRoles.Clear();
                    foreach (var roleId in userNewRoles)
                    {
                        user.UserRoles.Add(new UserRole
                        {
                            RoleId = roleId
                        });
                    }
                }
            }
            UnitOfWork.Users.Update(user);
            await UnitOfWork.CommitAsync();
            return CustomResponseDto<UserDto>.Success(200, Mapper.Map<UserDto>(user)); ;

        }
    }
}

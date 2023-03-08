using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    public class RoleController : CustomControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<RoleListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserRoleAssignDto(int userId)
        {
            var result = await _roleService.GetUserRoleAssignDtoAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<UserRoleAssignDto>.Success(result.StatusCode, result.Data));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Assign(UserRoleAssignDto userRoleAssignDto)
        {
            var result = await _roleService.AssignAsync(userRoleAssignDto);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponse<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponse<UserRoleAssignDto>.Success(result.StatusCode, result.Data));
        }
    }
}

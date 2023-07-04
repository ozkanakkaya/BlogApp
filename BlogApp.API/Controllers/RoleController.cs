using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<RoleListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            var result = await _roleService.GetAllByUserIdAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<RoleListDto>.Success(result.StatusCode, result.Data));
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> GetUserRoleAssignDto(int userId)
        {
            var result = await _roleService.GetUserRoleAssignDtoAsync(userId);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<UserRoleAssignDto>.Success(result.StatusCode, result.Data));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Assign(UserRoleAssignDto userRoleAssignDto)
        {
            var result = await _roleService.AssignAsync(userRoleAssignDto);
            if (result.Errors.Any())
            {
                return CreateActionResult(CustomResponseDto<NoContent>.Fail(result.StatusCode, result.Errors));
            }
            return CreateActionResult(CustomResponseDto<UserDto>.Success(result.StatusCode, result.Data));
        }
    }
}

using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;
using BlogApp.WEB.Areas.Admin.Models;
using BlogApp.WEB.Services;
using BlogApp.WEB.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BlogApp.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleApiService _roleApiService;

        public RoleController(RoleApiService roleApiService)
        {
            _roleApiService = roleApiService;
        }

        [Authorize(Roles = "SuperAdmin,Role.Read")]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleApiService.GetAllRolesAsync();

            if (!roles.Errors.Any() && roles.Data != null)
                return View(new RoleListDto
                {
                    Roles = roles.Data.Roles
                });
            else
            {
                ViewBag.ErrorMessage = roles.Errors.Any() ? roles.Errors.FirstOrDefault() : "Kayıt Bulunamadı!";
                return View();
            }
        }

        [Authorize(Roles = "SuperAdmin,Role.Read")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleApiService.GetAllRolesAsync();

            var roleListDto = JsonSerializer.Serialize(new RoleListDto
            {
                Roles = roles.Data.Roles
            });

            return Json(roleListDto);
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Assign(int userId)
        {
            var result = await _roleApiService.GetUserRoleAssignDtoAsync(userId);

            return PartialView("_RoleAssignPartial", result.Data);
        }

        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPut]
        public async Task<IActionResult> Assign(UserRoleAssignDto userRoleAssignDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleApiService.AssignAsync(userRoleAssignDto);

                var userRoleAssignAjaxViewModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
                {
                    UserViewModel = new UserViewModel
                    {
                        UserDto = result.Data,
                        Message = $"{result.Data.Username} kullanıcısının rol atama işlemleri baraşıyla tamamlanmıştır.",
                        ResultStatus = ResultStatus.Success
                    },
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", userRoleAssignDto)
                });
                return Json(userRoleAssignAjaxViewModel);
            }
            else
            {
                var userRoleAssignAjaxErrorModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel
                {
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial", userRoleAssignDto),
                    UserRoleAssignDto = userRoleAssignDto
                });
                return Json(userRoleAssignAjaxErrorModel);
            }
        }
    }
}

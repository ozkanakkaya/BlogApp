﻿using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Models
{
    public class UserRoleAssignAjaxViewModel
    {
        public UserRoleAssignDto UserRoleAssignDto { get; set; }
        public string RoleAssignPartial { get; set; }
        public UserViewModel UserViewModel { get; set; }
    }
}

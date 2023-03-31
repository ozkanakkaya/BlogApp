namespace BlogApp.Core.DTOs.Concrete
{
    public class UserRoleAssignDto
    {
        public UserRoleAssignDto()
        {
            RoleAssignments = new List<RoleAssignDto>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public IList<RoleAssignDto> RoleAssignments { get; set; }

    }
}

namespace BlogApp.Core.DTOs.Concrete
{
    public class UserRoleAssignDto
    {
        public UserRoleAssignDto()
        {
            RoleAssignDtos = new List<RoleAssignDto>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public IList<RoleAssignDto> RoleAssignDtos { get; set; }

    }
}

using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class AppUser : BaseEntity, IEntity
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string GitHubLink { get; set; }
        public string WebsiteLink { get; set; }
        public int GenderId { get; set; }

        public Gender Gender { get; set; }
        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<Blog> Blogs { get; set; }

    }
}

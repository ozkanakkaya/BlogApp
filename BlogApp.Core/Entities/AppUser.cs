using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public class AppUser : BaseEntity
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

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

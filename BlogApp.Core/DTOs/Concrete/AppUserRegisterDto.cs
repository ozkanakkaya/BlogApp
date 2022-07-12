using BlogApp.Core.DTOs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete
{
    public class AppUserRegisterDto : IDto
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
    }
}

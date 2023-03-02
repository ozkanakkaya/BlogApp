using BlogApp.Core.DTOs.Abstract;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Core.DTOs.Concrete
{
    public class AppUserUpdateDto : IDto
    {
        public int Id { get; set; }
        public int GenderId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
        public string About { get; set; }
        public string GitHubLink { get; set; }
        public string WebsiteLink { get; set; }
    }
}

using BlogApp.Core.DTOs.Abstract;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace BlogApp.Core.DTOs.Concrete
{
    public class UserRegisterDto : IDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string GitHubLink { get; set; }
        public string WebsiteLink { get; set; }
        public int GenderId { get; set; }
    }
}

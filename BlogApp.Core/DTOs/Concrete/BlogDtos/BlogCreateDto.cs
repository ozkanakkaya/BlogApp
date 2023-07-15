using BlogApp.Core.DTOs.Abstract;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace BlogApp.Core.DTOs.Concrete
{
    public class BlogCreateDto : IDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public IFormFile ImageFile { get; set; }
        public int UserId { get; set; }
        public string Tags { get; set; }
        public List<int> CategoryIds { get; set; }
        public bool IsActive { get; set; }
    }
}

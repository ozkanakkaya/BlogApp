using BlogApp.Core.DTOs.Abstract;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Core.DTOs.Concrete
{
    public class BlogUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public IFormFile ThumbnailFile { get; set; }
        public string ImageUrl { get; set; }
        public int AppUserId { get; set; }
        public string Tags { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}

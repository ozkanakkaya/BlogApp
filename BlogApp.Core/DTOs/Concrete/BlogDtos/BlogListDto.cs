
using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogListDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUrl { get; set; }
        public int AppUserId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Categories { get; set; }
    }
}

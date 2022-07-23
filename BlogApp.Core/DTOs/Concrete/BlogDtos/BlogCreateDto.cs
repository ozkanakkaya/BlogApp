using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogCreateDto : IDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public int AppUserId { get; set; }

        public bool IsActive { get; set; }

        //public string Tags { get; set; }
        //public ICollection<TagBlog> TagBlogs { get; set; }
    }
}

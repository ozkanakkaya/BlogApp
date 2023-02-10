using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class PersonalBlogDto : IDto
    {
        public List<BlogListDto> Blogs { get; set; }
        public int TotalBlogCount { get; set; }
        public int TotalActiveBlogCount { get; set; }
        public int TotalInactiveBlogCount { get; set; }
        public int TotalBlogsViewedCount { get; set; }

    }
}

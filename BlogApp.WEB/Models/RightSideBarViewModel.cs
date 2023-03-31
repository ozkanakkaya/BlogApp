using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Models
{
    public class RightSideBarViewModel
    {
        public CategoryListDto CategoryListDto { get; set; }
        public IList<BlogListDto> MostReadBlogs { get; set; }
        public IList<BlogListDto> LatestPosts { get; set; }

    }
}

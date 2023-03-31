using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Models
{
    public class BlogDetailViewModel
    {
        public BlogListDto BlogListDto { get; set; }
        public BlogDetailRightSideBarViewModel BlogDetailRightSideBarViewModel { get; set; }
    }
}

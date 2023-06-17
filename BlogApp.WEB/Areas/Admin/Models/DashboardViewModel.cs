using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoriesCount { get; set; }
        public int BlogsCount { get; set; }
        public int CommentsCount { get; set; }
        public int UsersCount { get; set; }
        public IList<BlogListDto> Blogs { get; set; }
    }
}

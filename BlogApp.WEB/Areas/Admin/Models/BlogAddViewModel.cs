using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class BlogAddViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
        public int AppUserId { get; set; }
        public string Tags { get; set; }
        public List<int> CategoryIds { get; set; }
        public bool IsActive { get; set; }
        public IList<CategoryDto> Categories { get; set; }
    }
}

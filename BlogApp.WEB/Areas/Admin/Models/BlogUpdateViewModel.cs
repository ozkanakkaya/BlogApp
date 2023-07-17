using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class BlogUpdateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
        public string Tags { get; set; }
        public IList<int> CategoryIds { get; set; }
        public bool IsActive { get; set; }
        public IList<CategoryDto> Categories { get; set; }
    }
}

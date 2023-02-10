using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CategoryDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

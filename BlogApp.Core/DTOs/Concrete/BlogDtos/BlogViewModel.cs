using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class BlogViewModel : BaseDto, IDto
    {
        public List<BlogListDto> BlogListDto { get; set; }
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}

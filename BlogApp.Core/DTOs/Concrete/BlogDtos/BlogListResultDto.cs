using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class BlogListResultDto : BaseDto, IDto
    {
        public IList<BlogListDto> BlogListDto { get; set; }
        public string Keyword { get; set; }
        public int? CategoryId { get; set; }
    }
}

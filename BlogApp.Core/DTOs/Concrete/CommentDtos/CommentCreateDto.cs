using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CommentCreateDto : IDto
    {
        public string Content { get; set; }
        public int BlogId { get; set; }
    }
}

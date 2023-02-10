using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CommentDto : IDto
    {
        public string Content { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public int BlogId { get; set; }
    }
}

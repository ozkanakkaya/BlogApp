using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CommentListDto : IDto
    {
        public IList<CommentDto> Comments { get; set; }
    }
}

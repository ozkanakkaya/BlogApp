using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Models
{
    public class CommentCreateAjaxViewModel
    {
        public CommentCreateDto CommentCreateDto { get; set; }
        public string CommentCreatePartial { get; set; }
        public CommentDto CommentDto { get; set; }

    }
}

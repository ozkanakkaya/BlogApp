using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class CommentViewModel
    {
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
        public CommentDto CommentDto { get; set; }
        public CommentListDto CommentListDto { get; set; }
    }
}

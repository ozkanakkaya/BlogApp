using BlogApp.Core.DTOs.Concrete;
using BlogApp.WEB.Areas.Admin.Models;

namespace BlogApp.WEB
{
    public class CommentUpdateAjaxViewModel
    {
        public CommentUpdateDto CommentUpdateDto { get; set; }
        public CommentViewModel CommentViewModel { get; set; }
        public string CommentUpdatePartial { get; set; }
    }
}
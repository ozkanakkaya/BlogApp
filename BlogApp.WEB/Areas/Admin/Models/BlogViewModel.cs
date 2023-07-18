using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class BlogViewModel
    {
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
        public BlogListDto BlogDto { get; set; }
        public List<BlogListDto> BlogListDto { get; set; }
    }
}

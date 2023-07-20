using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class TagViewModel
    {
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
        public TagDto TagDto { get; set; }
        public TagListDto TagListDto { get; set; }
    }
}

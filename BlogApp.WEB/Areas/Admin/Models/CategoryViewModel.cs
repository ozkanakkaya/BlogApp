using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public ResultStatus ResultStatus { get; set; }
        public string Message { get; set; }
        public CategoryDto CategoryDto { get; set; }
        public CategoryListDto CategoryListDto { get; set; }
        public CategoryCreateDto CategoryCreateDto { get; set; }
    }
}

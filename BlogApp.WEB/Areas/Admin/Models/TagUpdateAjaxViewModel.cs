using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class TagUpdateAjaxViewModel
    {
        public TagUpdateDto TagUpdateDto { get; set; }
        public TagViewModel TagViewModel { get; set; }
        public string TagUpdatePartial { get; set; }
    }
}

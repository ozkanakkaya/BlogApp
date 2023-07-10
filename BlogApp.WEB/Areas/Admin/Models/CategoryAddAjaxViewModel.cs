using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Areas.Admin.Models
{
    public class CategoryAddAjaxViewModel
    {
        public CategoryViewModel CategoryViewModel { get; set; }
        
        public string CategoryAddPartial { get; set; }

    }
}

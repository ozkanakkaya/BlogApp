using BlogApp.Core.DTOs.Concrete;

namespace BlogApp.WEB.Models
{
    public class BlogSearchViewModel
    {
        public BlogListResultDto BlogListResultDto { get; set; }
        public string Keyword { get; set; }
    }
}

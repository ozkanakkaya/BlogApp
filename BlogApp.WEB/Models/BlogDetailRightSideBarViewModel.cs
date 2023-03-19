using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.WEB.Models
{
	public class BlogDetailRightSideBarViewModel
	{
		public string Header { get; set; }
        public IList<BlogListDto> BlogListDto { get; set; }
        public UserDto User { get; set; }
    }
}

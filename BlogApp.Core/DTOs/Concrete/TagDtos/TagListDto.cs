using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.DTOs.Concrete
{
    public class TagListDto
    {
        public IList<Tag> Tags { get; set; }
    }
}

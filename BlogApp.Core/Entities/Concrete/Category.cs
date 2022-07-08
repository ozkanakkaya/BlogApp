using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Category : BaseEntity, IEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}

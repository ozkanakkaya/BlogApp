using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Tag : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<BlogTag> BlogTags { get; set; }
    }
}

using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Category : BaseEntity, IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<BlogCategory> BlogCategories { get; set; }
    }
}

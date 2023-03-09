using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Gender : IEntity
    {
        public int Id { get; set; }
        public string Definition { get; set; }

        public ICollection<User> Users { get; set; }
    }
}

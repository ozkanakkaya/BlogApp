using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Comment : BaseEntity, IEntity
    {
        public string Content { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; } = 0;
        public string Firstname { get; set; } = null;
        public string Lastname { get; set; } = null;
        public string ImageUrl { get; set; } = null;
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}

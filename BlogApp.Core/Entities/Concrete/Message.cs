using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Message : IEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

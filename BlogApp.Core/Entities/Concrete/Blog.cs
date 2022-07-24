using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete
{
    public class Blog : BaseEntity, IEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUrl { get; set; }
        public int ViewsCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<TagBlog> TagBlogs { get; set; }
        public ICollection<BlogCategory> BlogCategories { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public string ImageUrl { get; set; }

        public bool Status { get; set; }

        public int ViewsCount { get; set; } = 0;

        public int CommentCount { get; set; } = 0;

        public int LikeCount { get; set; } = 0;

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<TagBlog> TagBlogs { get; set; }
    }
}

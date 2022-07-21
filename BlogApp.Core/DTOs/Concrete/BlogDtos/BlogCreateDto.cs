using BlogApp.Core.DTOs.Abstract;
using BlogApp.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogCreateDto : IDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Thumbnail { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public int AppUserId { get; set; }

        public bool IsActive { get; set; }
        public ICollection<TagBlog> TagBlogs { get; set; }
    }
}

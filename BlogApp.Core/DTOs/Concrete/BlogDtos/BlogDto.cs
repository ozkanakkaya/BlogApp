using BlogApp.Core.DTOs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogDto : IDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public int ViewsCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public string Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByUsername { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}

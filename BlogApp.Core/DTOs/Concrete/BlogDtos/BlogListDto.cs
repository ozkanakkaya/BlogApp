﻿
using BlogApp.Core.DTOs.Abstract;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogListDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUrl { get; set; }
        public int ViewsCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByUsername { get; set; }
        public int AppUserId { get; set; }



        //public IList<Blog> Blogs { get; set; }
        public int? CategoryId { get; set; }
        public List<string> Tags { get; set; }
        public List<string> Categories { get; set; }
    }
}

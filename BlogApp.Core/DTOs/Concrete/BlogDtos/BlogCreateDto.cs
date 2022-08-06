﻿using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.BlogDtos
{
    public class BlogCreateDto : IDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public string ImageUrl { get; set; }
        public int AppUserId { get; set; }
        public string Tags { get; set; }
        public List<int> CategoryIds { get; set; }
        public bool IsActive { get; set; }
    }
}
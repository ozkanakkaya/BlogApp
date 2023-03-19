using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class BlogListDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int ViewCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int LikeCount { get; set; } = 0;
        public DateTime CreatedDate { get; set; }
        public string CreatedByUsername { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedByUsername { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

        public List<TagDto> Tags { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<CommentDto> Comments { get; set; }
        public UserDto User { get; set; }

    }
}

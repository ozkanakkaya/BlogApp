using BlogApp.Core.DTOs.Abstract;
using BlogApp.Core.Entities.Concrete;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CommentDto : IDto
    {
        public int Id { get; set; }
        public string BlogTitle { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedByUsername { get; set; }
        public string UpdatedByUsername { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }

    }
}

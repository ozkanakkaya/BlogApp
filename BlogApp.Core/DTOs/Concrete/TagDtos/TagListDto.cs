using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.TagDtos
{
    public class TagListDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

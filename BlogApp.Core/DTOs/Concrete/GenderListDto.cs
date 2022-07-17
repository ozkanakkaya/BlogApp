using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class GenderListDto : IDto
    {
        public int Id { get; set; }

        public string Definition { get; set; }
    }
}

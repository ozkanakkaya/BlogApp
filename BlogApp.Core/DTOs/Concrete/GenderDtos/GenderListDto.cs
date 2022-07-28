using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete.GenderDtos
{
    public class GenderListDto : IDto
    {
        public int Id { get; set; }

        public string Definition { get; set; }
    }
}

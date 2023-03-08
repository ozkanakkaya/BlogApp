using BlogApp.Core.DTOs.Abstract;

namespace BlogApp.Core.DTOs.Concrete
{
    public class CheckUserResponseDto : IDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }
}

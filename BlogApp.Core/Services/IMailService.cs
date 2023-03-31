using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Response;

namespace BlogApp.Core.Services
{
    public interface IMailService
    {
        CustomResponseDto<NoContent> Send(EmailSendDto emailSendDto);
        CustomResponseDto<NoContent> SendContactEmail(EmailSendDto emailSendDto);

    }
}

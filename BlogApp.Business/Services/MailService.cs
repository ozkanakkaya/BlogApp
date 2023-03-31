using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Entities.Concrete;
using BlogApp.Core.Response;
using BlogApp.Core.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace BlogApp.Business.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpSettings _smtpSettings;

        public MailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public CustomResponseDto<NoContent> Send(EmailSendDto emailSendDto)
        {
            MailMessage message = new()
            {
                From = new MailAddress(_smtpSettings.Server),
                To = { new MailAddress(emailSendDto.Email) },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = emailSendDto.Message
            };

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            smtpClient.Send(message);

            return CustomResponseDto<NoContent>.Success(200);
        }

        public CustomResponseDto<NoContent> SendContactEmail(EmailSendDto emailSendDto)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.SenderEmail),
                To = { new MailAddress("ozkky@hotmail.com") },
                Subject = emailSendDto.Subject,
                IsBodyHtml = true,
                Body = $"<br/> Gönderen Kişi: {emailSendDto.Name}<br/> Gönderen E-Posta Adresi: {emailSendDto.Email}<br/> Mesaj: {emailSendDto.Message}",
            };

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Server,
                Port = _smtpSettings.Port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.SenderEmail, _smtpSettings.Password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            try
            {
                smtpClient.Send(message);
            }
            catch (Exception)
            {

                return CustomResponseDto<NoContent>.Fail(400, "Mesajınız gönderilirken bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }

            return CustomResponseDto<NoContent>.Success(200);
        }
    }
}

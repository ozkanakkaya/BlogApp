using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Response;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Core.Utilities.Abstract
{
    public interface IImageHelper
    {
        Task<CustomResponse<ImageUploadedDto>> UploadAsync(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
        Task<CustomResponse<ImageDeletedDto>> DeleteAsync(string imageName);
    }
}

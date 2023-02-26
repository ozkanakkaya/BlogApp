using BlogApp.Core.DTOs.Concrete;
using BlogApp.Core.Enums.ComplexTypes;
using BlogApp.Core.Response;
using BlogApp.Core.Utilities.Abstract;
using BlogApp.Core.Utilities.Extensions;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace BlogApp.Business.Helpers
{
    public class ImageHelper : IImageHelper
    {
        private readonly IFileAccess _fileAccess;
        private const string imgFolder = "img";
        private const string userImagesFolder = "userImages";
        private const string postImagesFolder = "postImages";

        public ImageHelper(IFileAccess fileAccess)
        {
            _fileAccess = fileAccess;
        }

        public async Task<CustomResponse<ImageUploadedDto>> UploadAsync(string name, IFormFile imageFile, ImageType imageType, string folderName = null)
        {
            /* Eğer folderName değişkeni null gelir ise, o zaman resim tipine göre (PictureType) klasör adı ataması yapılır. */
            folderName ??= imageType == ImageType.User ? userImagesFolder : postImagesFolder;

            /* Eğer folderName değişkeni ile gelen klasör adı sistemimizde mevcut değilse, yeni bir klasör oluşturulur. */
            if (!await _fileAccess.FileExistsAsync($"{imgFolder}/{folderName}"))
            {
                await _fileAccess.CreateDirectoryAsync($"{imgFolder}/{folderName}");
            }

            if (imageFile == null || imageFile.Length < 0) return CustomResponse<ImageUploadedDto>.Fail(400, "imageFile boş");
            /* Resimin yüklenme sırasındaki ilk adı oldFileName adlı değişkene atanır. */
            string oldFileName = await _fileAccess.GetFileNameWithoutExtensionAsync(imageFile.FileName);

            /* Resimin uzantısı fileExtension adlı değişkene atanır. */
            string fileExtension = await _fileAccess.GetExtensionAsync(imageFile.FileName);

            Regex regex = new Regex("[*'\",._&#^@]");
            name = regex.Replace(name, string.Empty);

            DateTime dateTime = DateTime.Now;
            /*
            // Parametre ile gelen değerler kullanılarak yeni bir resim adı oluşturulur.
            // Örn: OzkanAkkaya_587_5_38_12_23_02_2023.png
            */
            string newFileName = $"{name}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";

            /* Kendi parametrelerimiz ile sistemimize uygun yeni bir dosya yolu (path) oluşturulur. */
            var path = $"{imgFolder}/{folderName}/{newFileName}";

            /* Sistemimiz için oluşturulan yeni dosya yoluna resim kopyalanır. */
            await _fileAccess.SaveFileAsync(path, imageFile);

            /* Resim tipine göre kullanıcı için bir mesaj oluşturulur. */
            string message = imageType == ImageType.User
                ? $"{name} adlı kullanıcının profil fotoğrafı başarıyla yüklenmiştir."
                : $"{name} adlı bloğun fotoğrafı başarıyla yüklenmiştir.";

            var imageUploadedDto = new ImageUploadedDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = imageFile.Length
            };

            return CustomResponse<ImageUploadedDto>.Success(200, imageUploadedDto);
        }

        public async Task<CustomResponse<ImageDeletedDto>> DeleteAsync(string imageName)
        {
            if (await _fileAccess.FileExistsAsync($"{imgFolder}/{imageName}"))
            {
                var fileInfo = await _fileAccess.GetImageFileInfoAsync($"{imgFolder}/{imageName}");
                var imageDeleteDto = new ImageDeletedDto
                {
                    FullName = imageName,
                    Extension = fileInfo.Extension,
                    Path = fileInfo.FullName,
                    Size = fileInfo.Length
                };
                await _fileAccess.DeleteFileAsync($"{imgFolder}/{imageName}");
                return CustomResponse<ImageDeletedDto>.Success(200, imageDeleteDto);
            }
            else
            {
                return CustomResponse<ImageDeletedDto>.Fail(400, "Böyle bir resim bulunamadı!");
            }
        }
    }
}

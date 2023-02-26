using Microsoft.AspNetCore.Http;

namespace BlogApp.Core.Utilities.Abstract
{
    public interface IFileAccess
    {
        Task<bool> FileExistsAsync(string filePath);
        Task CreateDirectoryAsync(string directoryPath);
        Task SaveFileAsync(string filePath, IFormFile file);
        Task DeleteFileAsync(string filePath);
        Task<FileInfo> GetImageFileInfoAsync(string filePath);
        Task<string> GetFileNameWithoutExtensionAsync(string fileName);
        Task<string> GetExtensionAsync(string fileName);
    }
}

using BlogApp.Core.Utilities.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Helpers
{
    public class FileAccess : IFileAccess
    {
        private readonly string _wwwroot;

        public FileAccess(IWebHostEnvironment env)
        {
            _wwwroot = env.WebRootPath;
        }

        public async Task<bool> FileExistsAsync(string filePath)
        {
            string fullPath = Path.Combine(_wwwroot, filePath);
            return await Task.FromResult(File.Exists(fullPath));
        }

        public async Task CreateDirectoryAsync(string directoryPath)
        {
            string fullPath = Path.Combine(_wwwroot, directoryPath);
            await Task.Run(() => Directory.CreateDirectory(fullPath));
        }

        public async Task SaveFileAsync(string filePath, IFormFile file)
        {
            string fullPath = Path.Combine(_wwwroot, filePath);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            string fullPath = Path.Combine(_wwwroot, filePath);
            if (await FileExistsAsync(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
        }

        public async Task<FileInfo> GetImageFileInfoAsync(string filePath)
        {
            string fullPath = Path.Combine(_wwwroot, filePath);
            var fileInfo = new FileInfo(fullPath);
            return await Task.FromResult(new FileInfo(fullPath));
        }

        public async Task<string> GetFileNameWithoutExtensionAsync(string fileName)
        {
            return await Task.Run(() =>
            {
                return Path.GetFileNameWithoutExtension(fileName);
            });
        }

        public async Task<string> GetExtensionAsync(string fileName)
        {
            return await Task.Run(() =>
            {
                return Path.GetExtension(fileName);
            });
        }
    }
}

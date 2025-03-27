using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PRM392.Services.Interfaces;
using PRM392.Utils;

namespace PRM392.Services
{
    public class StorageService : IStorageService
    {
        private readonly string _storagePath;
        private const string folderName = "web";

        public StorageService(IWebHostEnvironment env)
        {
            _storagePath = Path.Combine(env.WebRootPath, folderName);
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string fileName = Utilities.GenerateSlug(Path.GetFileNameWithoutExtension(file.FileName), true) + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_storagePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public bool DeleteFile(string filePath)
        {
            string fullPath = Path.Combine(_storagePath, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
    }

}

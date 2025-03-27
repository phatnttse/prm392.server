using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRM392.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFileAsync(IFormFile file);
        bool DeleteFile(string filePath);
    }
}

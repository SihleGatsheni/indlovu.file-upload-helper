using FileUploadHelper.IFileUploadHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FileUploadHelper.Strategy
{
    public class LocalFileUploadHelper : IUploadHelper
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public LocalFileUploadHelper(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> PutAsync(string dirPath, IFormFile image, CancellationToken cancellationToken = default)
        {
            if (image is null) return string.Empty;
            var uploadDir = Path.Combine(_hostEnvironment.WebRootPath, dirPath);
            var fileName = Guid.NewGuid() + "-" + image.FileName;
            var filePath = Path.Combine(uploadDir, fileName);
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(fileStream, cancellationToken);
            return fileName;
        }

        public Task<bool> RemoveAsync(string dirPath, string filename)
        {
            filename = Path.Combine(_hostEnvironment.WebRootPath, dirPath, filename);
            var file = new FileInfo(filename);
            try
            {
                File.Delete(file.Name);
                file.Delete();
                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }
    }
}

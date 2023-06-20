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
            string fileName = null;
            try
            {
                if (image is not null)
                {
                    var uploadDir = Path.Combine(_hostEnvironment.WebRootPath, dirPath);
                    fileName = Guid.NewGuid() + "-" + image.FileName;
                    var filePath = Path.Combine(uploadDir, fileName);
                    await using var fileStream = new FileStream(filePath, FileMode.Create);
                    await image.CopyToAsync(fileStream, cancellationToken);
                }
            }
            catch (Exception si)
            {
                Debug.WriteLine(si.Message);
            }
            return fileName;
        }

        public bool Remove(string dirPath, string filename)
        {
            filename = Path.Combine(_hostEnvironment.WebRootPath, dirPath, filename);
            var file = new FileInfo(filename);
            File.Delete(file.Name);
            file.Delete();
            return true;
        }

        public string Put(string dirPath, IFormFile fileToUpload)
        {
            string fileName = null;
            try
            {
                if (fileToUpload != null)
                {
                    var uploadDir = Path.Combine(_hostEnvironment.WebRootPath, dirPath);
                    fileName = Guid.NewGuid() + "-" + fileToUpload.FileName;
                    var filePath = Path.Combine(uploadDir, fileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    fileToUpload.CopyToAsync(fileStream);
                }
            }
            catch (Exception si)
            {
                Debug.WriteLine(si.Message);
            }
            return fileName;
        }
    }
}

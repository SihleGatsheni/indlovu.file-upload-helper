using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileUploadHelper.IFileUploadHelper
{
   public interface IUploadHelper
    {
        public string Put(string dirPath, IFormFile image);
        public Task<string> PutAsync(string dirPath, IFormFile image, CancellationToken cancellationToken = default);
        public bool Remove(string dirPath, string filename);
    }
}

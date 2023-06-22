using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileUploadHelper.IFileUploadHelper
{
   public interface IUploadHelper
    {
        public Task<string> PutAsync(string dirPath, IFormFile image, CancellationToken cancellationToken = default);
        public Task<bool> RemoveAsync(string dirPath, string filename);
    }
}

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileUploadHelper.Strategy
{
    public interface IUploadFileStrategy
    {
        Task<bool> RemoveAsync(string path, string filename);
        Task<string> UploadAsync(string path, IFormFile file, CancellationToken cancellationToken = default);
    }
}
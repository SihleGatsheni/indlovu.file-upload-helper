using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileUploadHelper.Strategy
{
    public interface IUploadFileStrategy
    {
        bool Remove(string path, string filename);
        string Upload(string path, IFormFile file);
        Task<string> UploadAsync(string path, IFormFile file, CancellationToken cancellationToken = default);
    }
}
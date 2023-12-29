using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FileUploadHelper.IFileUploadHelper;
 public interface IUploadHelper
{
    public Task<string> PutAsync(IFormFile image,string dirPath = "", CancellationToken cancellationToken = default);
    public Task<bool> RemoveAsync(string filename,string dirPath = "");
}


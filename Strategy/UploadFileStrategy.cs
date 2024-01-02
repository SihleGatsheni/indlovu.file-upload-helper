using System.Threading;
using System.Threading.Tasks;
using FileUploadHelper.FileUploadHelperStrategy;
using Microsoft.AspNetCore.Http;


namespace FileUploadHelper.Strategy
{
    internal class UploadFileStrategy
    {
        private readonly IUploadHelperStrategy _uploadHelper;

        public UploadFileStrategy(IUploadHelperStrategy uploadHelper)
        {
            _uploadHelper = uploadHelper;
        }
        
        public async Task<bool> RemoveAsync(string filename, string path = "")
        {
            return await _uploadHelper.RemoveAsync(filename,path);
        }

        public async Task<string> PutAsync (IFormFile file,string path = "", CancellationToken cancellationToken = default)
        {
          return await _uploadHelper.PutAsync(file,path, cancellationToken);
        } 
    }
}

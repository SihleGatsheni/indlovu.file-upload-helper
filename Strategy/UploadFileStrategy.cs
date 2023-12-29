using System.Threading;
using System.Threading.Tasks;
using FileUploadHelper.IFileUploadHelper;
using Microsoft.AspNetCore.Http;


namespace FileUploadHelper.Strategy
{
    public class UploadFileStrategy
    {
        private readonly IUploadHelper _uploadHelper;

        public UploadFileStrategy(IUploadHelper uploadHelper)
        {
            _uploadHelper = uploadHelper;
        }
        
        public async Task<bool> RemoveAsync(string filename, string path = "")
        {
            return await _uploadHelper.RemoveAsync(filename,path);
        }

        public async Task<string> UploadAsync (IFormFile file,string path = "", CancellationToken cancellationToken = default)
        {
          return await _uploadHelper.PutAsync(file,path, cancellationToken);
        } 
    }
}

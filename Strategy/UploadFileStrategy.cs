using System.Threading;
using System.Threading.Tasks;
using FileUploadHelper.IFileUploadHelper;
using Microsoft.AspNetCore.Http;


namespace FileUploadHelper.Strategy
{
    public class UploadFileStrategy : IUploadFileStrategy
    {
        private readonly IUploadHelper _uploadHelper;

        public UploadFileStrategy(IUploadHelper uploadHelper)
        {
            _uploadHelper = uploadHelper;
        }
        
        public async Task<bool> RemoveAsync(string path, string filename)
        {
            return await _uploadHelper.RemoveAsync(path, filename);
        }

        public async Task<string> UploadAsync (string path,IFormFile file, CancellationToken cancellationToken = default)
        {
          return await _uploadHelper.PutAsync(path,file, cancellationToken);
        } 
    }
}

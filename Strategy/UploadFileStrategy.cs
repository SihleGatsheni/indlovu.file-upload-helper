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

        public string Upload(string path, IFormFile formFile)
        {
            return _uploadHelper.Put(path, formFile);
        }

        public bool Remove(string path, string filename)
        {
            return _uploadHelper.Remove(path, filename);
        }

        public async Task<string> UploadAsync (string path,IFormFile file, CancellationToken cancellationToken = default)
        {
          return await _uploadHelper.PutAsync(path,file, cancellationToken);
        } 
    }
}

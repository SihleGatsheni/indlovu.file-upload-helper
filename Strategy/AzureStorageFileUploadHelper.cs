using FileUploadHelper.IFileUploadHelper;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace FileUploadHelper.Strategy
{
    public class AzureStorageFileUploadHelper : IUploadHelper
    {
        private readonly BlobServiceClient _client;

        public AzureStorageFileUploadHelper(BlobServiceClient client)
        {
            _client = client;
        }
        
        public async Task<string> PutAsync(string dirPath, IFormFile image, CancellationToken cancellationToken = default)
        {
            if (image is null) return string.Empty;
            
            var filename = Guid.NewGuid() + "-" + image.FileName;

            var blobContainer = _client.GetBlobContainerClient(dirPath);

            var blobClient = blobContainer.GetBlobClient(filename);

            await blobClient.UploadAsync(image.OpenReadStream(), cancellationToken);
            return filename;
        }

        public bool Remove(string containerName, string filename)
        {
            var blobContainer = _client.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlobClient(filename);
            return blobClient.DeleteIfExistsAsync().Result;      
        }

        public string Put(string containerName, IFormFile fileToUpload)
        {
            if (fileToUpload is null) return string.Empty;
            var filename = Guid.NewGuid() + "-" + fileToUpload.FileName;

            var blobContainer = _client.GetBlobContainerClient(containerName);

            var blobClient = blobContainer.GetBlobClient(filename);

            blobClient.Upload(fileToUpload.OpenReadStream());
            return filename;
        }
    }
}

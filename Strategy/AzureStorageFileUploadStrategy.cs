﻿
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using FileUploadHelper.FileUploadHelperStrategy;

namespace FileUploadHelper.Strategy
{
    internal class AzureStorageFileUploadStrategy : IUploadHelperStrategy
    {
        private readonly BlobServiceClient _client;

        public AzureStorageFileUploadStrategy(BlobServiceClient client)
        {
            _client = client;
        }
        
        public async Task<string> PutAsync(IFormFile image,string dirPath, CancellationToken cancellationToken = default)
        { 
            if (image is null) return string.Empty;
            
            var filename = Guid.NewGuid() + "-" + image.FileName;

            var blobContainer = _client.GetBlobContainerClient(dirPath);

            var blobClient = blobContainer.GetBlobClient(filename);

            await blobClient.UploadAsync(image.OpenReadStream(), cancellationToken);
            return filename;
        }

        public async Task<bool> RemoveAsync(string containerName, string filename)
        {
            var blobContainer = _client.GetBlobContainerClient(containerName);
            var blobClient = blobContainer.GetBlobClient(filename);
            return await blobClient.DeleteIfExistsAsync();      
        }
    }
}

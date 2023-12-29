using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUploadHelper.IFileUploadHelper;
using Microsoft.AspNetCore.Http;
using Model;

namespace FileUploadHelper.Strategy;
public class AWSS3FileUploadStrategy : IUploadHelper
{
    private readonly AmazonS3Client _client;
    private readonly AWSS3Credentials _credentials;

    public AWSS3FileUploadStrategy(AWSS3Credentials credentials)
    {
        _credentials = credentials;
        _client = new AmazonS3Client(credentials.AccessKey, credentials.SecretKey, RegionEndpoint.GetBySystemName(credentials.Region));
    }

    public async Task<string> PutAsync(IFormFile image,string dirPath, CancellationToken cancellationToken)
    {
        if (image is null) return string.Empty;
        var filename = Guid.NewGuid() + "-" + image.FileName;

        var bucketExist = await AmazonS3Util.DoesS3BucketExistV2Async(_client, _credentials.BucketName);
        if (!bucketExist)
        {
            var createBucketRequest = new PutBucketRequest
            {
                BucketName = _credentials.BucketName,
                UseClientRegion = true
            };
            await _client.PutBucketAsync(createBucketRequest, cancellationToken);
        }

        var putRequest = await _client.PutObjectAsync(new PutObjectRequest
        {
            BucketName = _credentials.BucketName,
            Key = filename,
            InputStream = image.OpenReadStream(),
            ContentType = image.ContentType,
        }, cancellationToken);

        return putRequest.HttpStatusCode == System.Net.HttpStatusCode.OK ? _client.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            BucketName = _credentials.BucketName,
            Key = filename,
            Expires = DateTime.Now.AddDays(5)
        }) : string.Empty;
    }

    public async Task<bool> RemoveAsync(string filename, string dirPath)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _credentials.BucketName,
            Key = filename
        };
        var response = await _client.DeleteObjectAsync(deleteRequest);
        return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }
}
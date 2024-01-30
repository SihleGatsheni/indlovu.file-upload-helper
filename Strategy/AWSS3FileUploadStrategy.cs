using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using FileUploadHelper.FileUploadHelperStrategy;
using Microsoft.AspNetCore.Http;
using FileUploadHelper.Model;

namespace FileUploadHelper.Strategy;
internal class AWSS3FileUploadStrategy : IUploadHelperStrategy
{
    private readonly AmazonS3Client _client;
    private readonly AWSS3Credentials _credentials;
    private readonly Action<PutBucketNotificationRequest> _configureBucketNotificationEvents;

    public AWSS3FileUploadStrategy(AWSS3Credentials credentials, Action<PutBucketNotificationRequest> configureBucketNotificationEvents = null)
    {
        _credentials = credentials;
        _client = new AmazonS3Client(credentials.AccessKey, credentials.SecretKey, RegionEndpoint.GetBySystemName(credentials.Region));
        _configureBucketNotificationEvents = configureBucketNotificationEvents;
    }

    public async Task<string> PutAsync(IFormFile image, string dirPath, CancellationToken cancellationToken)
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

            if (_configureBucketNotificationEvents is not null)
            {

                var createBucketNotificationRequest = new PutBucketNotificationRequest
                {
                    BucketName = _credentials.BucketName,
                    LambdaFunctionConfigurations = new List<LambdaFunctionConfiguration>()
                };
                _configureBucketNotificationEvents?.Invoke(createBucketNotificationRequest);
                await _client.PutBucketNotificationAsync(createBucketNotificationRequest, cancellationToken);
            }
        }
        if (_credentials.IsPublicBucket)
        {
            var publicAccessBlockConfig = new PublicAccessBlockConfiguration
            {
                BlockPublicAcls = false,
                IgnorePublicAcls = false,
                BlockPublicPolicy = false,
                RestrictPublicBuckets = false
            };

            // Create a PutPublicAccessBlockRequest
            var request = new PutPublicAccessBlockRequest
            {
                BucketName = _credentials.BucketName,
                PublicAccessBlockConfiguration = publicAccessBlockConfig
            };

            // Set the bucket to be public
            await _client.PutPublicAccessBlockAsync(request, cancellationToken);
            var putRequestObj = await _client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _credentials.BucketName,
                Key = filename,
                InputStream = image.OpenReadStream(),
                ContentType = image.ContentType,
            }, cancellationToken);

            string objectUrl = $"https://{_credentials.BucketName}.s3.{_client.Config.RegionEndpoint.SystemName}.amazonaws.com/{filename}";
            return putRequestObj.HttpStatusCode == System.Net.HttpStatusCode.OK ? objectUrl :string.Empty;
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
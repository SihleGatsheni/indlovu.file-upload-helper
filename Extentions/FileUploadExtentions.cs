using System;
using Amazon.S3.Model;
using Azure.Storage.Blobs;
using FileUploadHelper.FileUploadHelperStrategy;
using FileUploadHelper.Model;
using FileUploadHelper.Strategy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploadHelper.Extentions;
public static class FileUploadExtentions
{

    public static IServiceCollection AddFileWithAWSS3trategy(this IServiceCollection services, AWSS3Credentials credentials, Action<PutBucketNotificationRequest> configureBucketNotificationEvents = null)
    {
        services.AddScoped<IUploadHelperStrategy, AWSS3FileUploadStrategy>(_ =>
        {
            return new AWSS3FileUploadStrategy(credentials,configureBucketNotificationEvents);
        });
        return services;
    }
    public static IServiceCollection AddFileWithAZureBlobStrategy(this IServiceCollection services, AzureCredentials azureCredentials)
    {
        services.AddScoped<IUploadHelperStrategy, AzureStorageFileUploadStrategy>(_=>
        {
            return new AzureStorageFileUploadStrategy(new BlobServiceClient(azureCredentials.BlobConnectionString));
        });
        return services;
    }
    public static IServiceCollection AddFileWithAFirebaseStorageStrategy(this IServiceCollection services,FirebaseStorageCredentials firebaseStorageCredentials)
    {
        services.AddScoped<IUploadHelperStrategy, FirebaseStorageFileUploadStrategy>(_ =>{
           return new FirebaseStorageFileUploadStrategy(firebaseStorageCredentials);
        });
        return services;
    }

    public static IServiceCollection AddFileWithLocalFileSystemStrategy(this IServiceCollection services, IWebHostEnvironment webHost)
    {
        services.AddScoped<IUploadHelperStrategy, LocalFileUploadStrategy>(_ => {
            return new LocalFileUploadStrategy(webHost);
        });
        return services;
    }
}
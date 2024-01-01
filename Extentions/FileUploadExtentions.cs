using System;
using Azure.Storage.Blobs;
using FileUploadHelper.FileUploadHelperStrategy;
using FileUploadHelper.Model;
using FileUploadHelper.Strategy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Model;

namespace Extentions;
public static class FileUploadExtentions
{

    public static IServiceCollection AddFileWithAWStrategy(this IServiceCollection services, AWSS3Credentials credentials)
    {
        services.AddScoped<IUploadHelperStrategy, AWSS3FileUploadStrategy>(provider =>
        {
            return new AWSS3FileUploadStrategy(credentials);
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
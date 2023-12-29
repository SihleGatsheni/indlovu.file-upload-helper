using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FileUploadHelper.IFileUploadHelper;
using FileUploadHelper.Model;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileUploadHelper.Strategy;

public class FirebaseStorageFileUploadStrategy: IUploadHelper
{
    private readonly FirebaseStorageCredentials _storageConfiguration;

    public FirebaseStorageFileUploadStrategy(FirebaseStorageCredentials configuration)
    {
        _storageConfiguration = configuration;
    }

    
    public async Task<string> PutAsync(IFormFile image,string dirPath,CancellationToken cancellationToken)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(_storageConfiguration.ApiKey));
        var authorized =
            await auth.SignInWithEmailAndPasswordAsync(_storageConfiguration.AuthEmail,
                _storageConfiguration.AuthPassword);
        var cancellation = new CancellationTokenSource();

        using var ms = new MemoryStream();
        var filename = Guid.NewGuid() + image.FileName;
        await image.CopyToAsync(ms, cancellationToken);
        ms.Seek(0, SeekOrigin.Begin);
        
        try
        {
            return await new FirebaseStorage(_storageConfiguration.Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(authorized.FirebaseToken),
                        ThrowOnCancel = true
                    })
                .Child(dirPath)
                .Child(filename)
                .PutAsync(ms, cancellation.Token);
        }
        catch
        {
            return string.Empty;
        }
    }

    public Task<bool> RemoveAsync(string filename,string dirPath)
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(_storageConfiguration.ApiKey));
        var authorized = 
            auth.SignInWithEmailAndPasswordAsync(_storageConfiguration.AuthEmail,
                _storageConfiguration.AuthPassword).Result;

        var storage = new FirebaseStorage(_storageConfiguration.Bucket, new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(authorized.FirebaseToken),
                ThrowOnCancel = true
            })
            .Child(dirPath)
            .Child(filename)
            .DeleteAsync();
        return Task.FromResult(storage.IsCompletedSuccessfully);
    }
}
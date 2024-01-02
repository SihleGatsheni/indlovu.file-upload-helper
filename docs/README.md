# FILE-UPLOAD-HELPER

## Welcome to the file-upload-helper Documentation!

### Added Features
The file-upload-helper now supports dependency injection with strategies for various cloud providers:

- AWS S3: `AddFileWithAWSS3Strategy`
- Azure Blob Storage: `AddFileWithAzureBlobStrategy`
- Google Firebase Storage: `AddFileWithFirebaseStorageStrategy`
- Local File System: `AddFileWithLocalFileSystemStrategy`

**file-upload-helper** accelerates .Net development by providing functionalities such as:

- File uploads to different servers and cloud providers.
- Support for AWS S3, Azure Blob Storage, and Firebase Cloud Storage.
- Writing files to the server's `wwwroot` directory/filesystem.

## Usage

1. Add the package to your project via [nuget.org](https://www.nuget.org/packages/file-upload-helper) or use the commands:

   - .NET CLI: `dotnet add package file-upload-helper --version 1.3.6`
   - Package Manager: `Install-Package file-upload-helper -Version 1.3.6`

## Inject IUploadHelperStrategy
```csharp
public class Demo
{
    private readonly IUploadHelperStrategy strategy;
    public Demo(IUploadHelperStrategy strategy){
        this.strategy = strategy
    }
}
```
## Set Up Desired Strategy Through DI(do not forget to set up or errors will the thrown)
```csharp
var builder = WebApplication.CreateBuilder(args);

- To use with AWS S3
builder.Services.AddFileWithAWSS3trategy(new AWSS3Credentials());

Also support the configuration of notification lambda to be triggered by this Bucket events for S3 operation

builder.Services.AddFileWithAWSS3trategy(new AWSS3Credentials(), configure =>
{
    configure.LambdaFunctionConfigurations = new List<LambdaFunctionConfiguration>{
            new LambdaFunctionConfiguration{
                Id = "lambda",
                Events = new List<EventType>{"s3:ObjectCreated:Put"},
                FunctionArn = builder.Configuration["AWS_LAMBDA_FUNCTION_ARN"]
            }
        };
}); 

-To Use with Firebase storage
builder.Services.AddFileWithAFirebaseStorageStrategy(new FirebaseStorageCredentials());

-To Use with Azure Blob storage
builder.Services.AddFileWithAZureBlobStrategy(new AzureCredentials());

-To Use With Local FileSysyem
builder.Services.AddFileWithLocalFileSystemStrategy(builder.Environment);
```



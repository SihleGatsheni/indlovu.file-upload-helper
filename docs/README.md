# <span style="color:#337ab7;">FILE-UPLOAD-HELPER</span>

## <span style="color:#337ab7;">Hi there! Welcome to the <span style="color:#ff7f50;">file-upload-helper</span> docs</span>

<span style="color:#ff7f50;">File-upload-helper</span> is a tool designed to help .Net developers speed up their development by offering functionalities like:

- Uploading files to different servers and cloud providers for storage.
- Currently, it supports 2 cloud storage providers: Google Firebase Storage and Azure Blob Storage.
- It also offers the ability to write your files on the server's `wwwroot` directory.

## <span style="color:#337ab7;">Usage</span>

The library provides a convenient interface for interacting with the code and allows you to switch between different providers easily. To use the library, follow these steps:

1. Add the nuget package to your project through [nuget.org](https://www.nuget.org/packages/file-upload-helper).

   Alternatively, you can use the following commands to reference the package to your project:

    - .NET CLI: <span style="color:#a71d5d;">`dotnet add package file-upload-helper --version 1.2.1`</span>
    - Package Manager: <span style="color:#a71d5d;">`Install-Package file-upload-helper -Version 1.2.1`</span>


## Interface Methods (IUploadFileStrategy)
```
public interface IUploadFileStrategy
{
Task<bool> Remove(string path, string filename);
Task<string> UploadAsync(string path, IFormFile file, CancellationToken cancellationToken = default);
}
```

#### - UploadAsync() : <span style="color: blue;">uploads</span> your file asynchronously using any of your chosen strategy and takes in <span style="color: purple;">containerName/filepath</span>, <span style="color: green;">IFormFile</span>, and a <span style="color: orange;">cancellationToken</span>.
#### - RemoveAsync() : <span style="color: blue;">removes</span> the file based on the <span style="color: purple;">filename</span>. Takes in the <span style="color: purple;">containerName/filepath</span> and <span style="color: purple;">filename</span>.

### The Concrete Implementation of the above interface constructor looks like below to support the swapping of multiple strategies
```
 public UploadFileStrategy(IUploadHelper uploadHelper)
        {
            _uploadHelper = uploadHelper;
        }
```
## There are three strategies to choose from namely
### - <span style="color: blue;">LocalFileUploadHelper</span>
### - <span style="color: purple;">AzureStorageFileUploadHelper</span>
### - <span style="color: green;">FirebaseStorageFileUploadHelper</span>

### _with future plans to support <span style="color: orange;">AWS S3 Bucket</span> storage in the next versions_


## How to Use the Library With the different Strategies

### 1) <span style="color: blue;">LocalFileUploadHelper</span>

To use the <span style="color: blue;">LocalFileUploadHelper</span> strategy, you need to inject the necessary dependencies.

```
 private readonly IUploadFileStrategy _fileStrategy;

    public UpdateUserImageRepository(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
        _fileStrategy = new UploadFileStrategy(new LocalFileUploadHelper(hostEnvironment));
    }
```

```
await _fileStrategy.UploadAsync("Images",image, cancellationToken); //returns filename
```
<span style="color: #4285F4; font-weight: bold;">After injecting the dependencies, you only need one line to upload your file.</span>

<span style="color: #4285F4; font-weight: bold;">2) AzureStorageFileUploadHelper:</span>  
<span style="color: #4285F4; font-weight: bold;">Need to inject the necessary dependencies.</span>

In `Program.cs`, add the following code:

```
 services.AddScoped(_ => {
                return new BlobServiceClient(BlobConfig.BlobConString);
            });
```


### Custom Object 
```
 public class BlobConfig
    {
        public  static string BlobConString { get { return "DefaultEndpointsProtocol=https;AccountName=yourAzureBlobStorageAccountName;AccountKey=yourApiKey"; } }
        public static string ContainerName { get { return "yourAzureBlobContainerName"; } }
    }
```

```
private IFileUpload<Accomodation> _fileUpLoad;
private readonly BlobServiceClient client;
public AccomodationRepo(BlobServiceClient client)
  {
         web = host;
         _fileUpLoad = new AccomodationUpload(client);
         db = context;
         this.client = client;
  }

```

```
await _fileUpLoad.UploadAsync("Products",Params.CoverImage, cancellationToken); //returns filename
```
<span style="color: #4285F4; font-weight: bold;">This is how convenient all the methods are just a one liner</span>

<span style="color: #4285F4; font-weight: bold;">2) FirebaseStorageFileUploadHelper:</span>  
<span style="color: #4285F4; font-weight: bold;">Need to inject the necessary dependencies.</span>

```
private readonly IUploadFileStrategy _uploadHelper;

    public ProductCategoryService(IConfiguration configuration)
    {
        _uploadHelper = new UploadFileStrategy(new FirebaseStorageFileUploadHelper(new FirebaseStorageConfiguration
        {
            ApiKey = configuration["Firebase:ApiKey"],
            Bucket = configuration["Firebase:Bucket"],
            AuthEmail = configuration["Firebase:AuthEmail"],
            AuthPassword = configuration["Firebase:AuthPassword"]
        }));
    }
```

```
public class FirebaseStorageConfiguration
{
    public string ApiKey { get; set; }
    public string Bucket { get; set; }
    public string AuthEmail { get; set; }
    public string AuthPassword { get; set; }
}
```
```
 await _uploadHelper.UploadAsync("CategoryPictures",category.CategoryImageUrl, cancellationToken) //returns downloadlink
```

<span style="color: #4285F4; font-weight: bold;">The above strategies offer one-liners and a few configs, and that's it. You have access to all the providers.</span>
<span style="color: #4285F4; font-weight: bold;">Below is an example of how to upload multiple images:</span>


```
private static async Task<IEnumerable<string>> AddProductPictures(IFormFileCollection collection)
    {
        var pictures = new List<string>();
        foreach (var image in collection)
        {
            pictures.Add(await _fileStrategy.UploadAsync("ProductPictures", image, cancellationToken));
        }
        return pictures;
    }
```

<span style="color: #4285F4; font-size: 24px;">That's it, thank you for the read...</span>

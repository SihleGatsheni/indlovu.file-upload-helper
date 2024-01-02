namespace FileUploadHelper.Model;

public class AWSS3Credentials
{
    public string AccessKey { get; init; }
    public string SecretKey { get; init; }
    public string BucketName { get; init; }
    public string Region { get; init; }
}
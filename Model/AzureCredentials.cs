namespace FileUploadHelper.Model;

public class AzureCredentials{
    public string BlobConnectionString { get{ return $"DefaultEndPointsProtocol=https;AccountName={AzureBlobStorageAccountName};AccountKey={AccountAccessKey}";}}
    public string AzureBlobStorageAccountName { get; init; }
    public string AccountAccessKey { get; init; }
}
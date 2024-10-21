using Microsoft.Extensions.Options;
using Amazon.S3.Transfer;
using UGHApi.Shared;
using Amazon.S3;
using Amazon;

public class S3Uploader
{
    private readonly string _bucketName;
    private readonly string _awsUrl;
    private readonly IAmazonS3 _s3Client;

    public S3Uploader(IOptions<AwsOptions> awsOptions)
    {
        var awsConfig = awsOptions.Value;

        _bucketName = awsConfig.BucketName;
        _awsUrl = awsConfig.AWS_Url;

        _s3Client = new AmazonS3Client(awsConfig.AccessKey, awsConfig.SecretKey, RegionEndpoint.GetBySystemName(awsConfig.Region));
    }

    public async Task<string> UploadFileAsync(byte[] fileData, string keyName, string contentType)
    {
        try
        {
            using (var stream = new MemoryStream(fileData))
            {
                var fileTransferUtility = new TransferUtility(_s3Client);

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = keyName,
                    BucketName = _bucketName,
                    ContentType = contentType
                };

                await fileTransferUtility.UploadAsync(uploadRequest);
                string fileLink = $"{_awsUrl}{keyName}";
                Console.WriteLine("File uploaded successfully.");
                return fileLink;
            }
        }
        catch (AmazonS3Exception e)
        {
            return $"Error encountered on server. Message:'{e.Message}' when writing an object";
        }
        catch (Exception e)
        {
            return $"Unknown error encountered on server. Message:'{e.Message}' when writing an object";
        }
    }
}

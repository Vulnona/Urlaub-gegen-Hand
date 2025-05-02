using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using UGHApi.Shared;

public class S3Service
{
    private readonly string _bucketName;
    private readonly string _awsUrl;
    private readonly IAmazonS3 _s3Client;

    public S3Service(IOptions<AwsOptions> awsOptions)
    {
        var awsConfig = awsOptions.Value;

        _bucketName = awsConfig.BucketName;
        _awsUrl = awsConfig.AWS_Url;

        _s3Client = new AmazonS3Client(
            awsConfig.AccessKey,
            awsConfig.SecretKey,
            RegionEndpoint.GetBySystemName(awsConfig.Region)
        );
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
                    ContentType = contentType,
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

    public async Task<(Stream FileStream, string ContentType)> GetFileAsync(string key)
    {
        try
        {
            var response = await _s3Client.GetObjectAsync(_bucketName, key);

            return (response.ResponseStream, response.Headers["Content-Type"]);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine($"Error encountered on server. Message:'{e.Message}'");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unknown error encountered on server. Message:'{e.Message}'");
            throw;
        }
    }

    public async Task<string> DeleteFileAsync(string keyName)
    {
        try
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = keyName,
            };

            var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("File deleted successfully.");
                return "File deleted successfully.";
            }
            else
            {
                return $"Error encountered. Status code: {response.HttpStatusCode}";
            }
        }
        catch (AmazonS3Exception e)
        {
            return $"Error encountered on server. Message:'{e.Message}' when deleting an object";
        }
        catch (Exception e)
        {
            return $"Unknown error encountered on server. Message:'{e.Message}' when deleting an object";
        }
    }
}

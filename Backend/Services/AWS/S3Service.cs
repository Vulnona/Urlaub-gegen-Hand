using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using UGHApi.Shared;
using Microsoft.Extensions.Logging;

public class S3Service
{
    private readonly string _bucketName;
    private readonly string _awsUrl;
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3Service> _logger;

    public S3Service(IOptions<AwsOptions> awsOptions, ILogger<S3Service> logger)
    {
        var awsConfig = awsOptions.Value;
        _logger = logger;

        _bucketName = awsConfig.BucketName;
        _awsUrl = awsConfig.AWS_Url;

        _logger.LogInformation("S3Service initialized successfully");

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
            _logger.LogInformation($"Starting file upload: {contentType}, size: {fileData.Length} bytes");

            // Validate input
            if (fileData == null || fileData.Length == 0)
            {
                throw new ArgumentException("File data cannot be null or empty");
            }

            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("Key name cannot be null or empty");
            }

            // Check file size (max 10MB)
            const int maxFileSize = 10 * 1024 * 1024; // 10MB
            if (fileData.Length > maxFileSize)
            {
                throw new ArgumentException($"File size {fileData.Length} bytes exceeds maximum allowed size of {maxFileSize} bytes");
            }

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
                
                _logger.LogInformation("File uploaded successfully");
                return fileLink;
            }
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError($"Amazon S3 error during upload: {e.Message}. Error code: {e.ErrorCode}, Request ID: {e.RequestId}");
            throw new Exception($"S3 upload failed: {e.Message}", e);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error during upload: {e.Message}. Stack trace: {e.StackTrace}");
            throw new Exception($"File upload failed: {e.Message}", e);
        }
    }

    public async Task<(Stream FileStream, string ContentType)> GetFileAsync(string key)
    {
        try
        {
            _logger.LogInformation("Retrieving file from S3");
            
            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            
            _logger.LogInformation($"File retrieved successfully. Content-Type: {response.Headers["Content-Type"]}");
            return (response.ResponseStream, response.Headers["Content-Type"]);
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError($"Amazon S3 error during file retrieval: {e.Message}. Error code: {e.ErrorCode}");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error during file retrieval: {e.Message}");
            throw;
        }
    }

    public async Task<string> DeleteFileAsync(string keyName)
    {
        try
        {
            _logger.LogInformation("Deleting file from S3");
            
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = keyName,
            };

            var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                _logger.LogInformation("File deleted successfully from S3");
                return "File deleted successfully.";
            }
            else
            {
                _logger.LogWarning($"Unexpected status code during file deletion: {response.HttpStatusCode}");
                return $"Error encountered. Status code: {response.HttpStatusCode}";
            }
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError($"Amazon S3 error during file deletion: {e.Message}. Error code: {e.ErrorCode}");
            return $"Error encountered on server. Message:'{e.Message}' when deleting an object";
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error during file deletion: {e.Message}");
            return $"Unknown error encountered on server. Message:'{e.Message}' when deleting an object";
        }
    }

    public string GetAwsUrl()
    {
        return _awsUrl;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string keyName, string contentType)
    {
        try
        {
            _logger.LogInformation($"Starting stream upload: {contentType}");

            // Validate input
            if (fileStream == null)
            {
                throw new ArgumentException("File stream cannot be null");
            }

            if (string.IsNullOrEmpty(keyName))
            {
                throw new ArgumentException("Key name cannot be null or empty");
            }

            var fileTransferUtility = new TransferUtility(_s3Client);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = keyName,
                BucketName = _bucketName,
                ContentType = contentType,
            };

            await fileTransferUtility.UploadAsync(uploadRequest);
            string fileLink = $"{_awsUrl}{keyName}";
            
            _logger.LogInformation("Stream uploaded successfully");
            return fileLink;
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError($"Amazon S3 error during upload: {e.Message}. Error code: {e.ErrorCode}, Request ID: {e.RequestId}");
            throw new Exception($"S3 upload failed: {e.Message}", e);
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error during upload: {e.Message}. Stack trace: {e.StackTrace}");
            throw new Exception($"File upload failed: {e.Message}", e);
        }
    }

    public async Task<List<S3Object>> ListFilesAsync(string prefix)
    {
        try
        {
            _logger.LogInformation("Listing files from S3");
            
            var listRequest = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsV2Async(listRequest);
            
            _logger.LogInformation($"Found {response.S3Objects.Count} files");
            return response.S3Objects;
        }
        catch (AmazonS3Exception e)
        {
            _logger.LogError($"Amazon S3 error during file listing: {e.Message}. Error code: {e.ErrorCode}");
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError($"Unexpected error during file listing: {e.Message}");
            throw;
        }
    }
}

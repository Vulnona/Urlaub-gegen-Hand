using Microsoft.AspNetCore.Mvc;
using Amazon.S3;

namespace UGHApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileManagementController : ControllerBase
    {
        private readonly S3Service _s3Uploader;
        public FileManagementController(S3Service s3Uploader)
        {
            _s3Uploader = s3Uploader;
        }

        [HttpGet("file/{key}")]
        public async Task<IActionResult> GetFile(string key)
        {
            try
            {
                var (fileStream, contentType) = await _s3Uploader.GetFileAsync(key);

                return File(fileStream, contentType, key);
            }
            catch (AmazonS3Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"S3 error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Server error: {ex.Message}");
            }
        }
    }
}

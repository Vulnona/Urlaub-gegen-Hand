using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UGHApi.Services.AWS;
using UGHApi.Services.BackgroundTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UGHApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Only admins can access backup operations
    public class BackupController : ControllerBase
    {
        private readonly ILogger<BackupController> _logger;
        private readonly S3Service _s3Service;
        private readonly DatabaseBackupService _backupService;

        public BackupController(
            ILogger<BackupController> logger,
            S3Service s3Service,
            DatabaseBackupService backupService)
        {
            _logger = logger;
            _s3Service = s3Service;
            _backupService = backupService;
        }

        /// <summary>
        /// Get list of available backups
        /// </summary>
        [HttpGet("list")]
        public async Task<IActionResult> GetBackups()
        {
            try
            {
                var backupPrefix = "backups/database/";
                var backupFiles = await _s3Service.ListFilesAsync(backupPrefix);

                var backups = backupFiles
                    .Where(f => f.Key.EndsWith(".sql"))
                    .Select(f => new
                    {
                        FileName = System.IO.Path.GetFileName(f.Key),
                        S3Key = f.Key,
                        Size = f.Size,
                        LastModified = f.LastModified,
                        SizeInMB = Math.Round(f.Size / 1024.0 / 1024.0, 2)
                    })
                    .OrderByDescending(b => b.LastModified)
                    .ToList();

                return Ok(new
                {
                    TotalBackups = backups.Count,
                    Backups = backups
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve backup list");
                return StatusCode(500, new { error = "Failed to retrieve backup list" });
            }
        }

        /// <summary>
        /// Trigger a manual backup
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                _logger.LogInformation("Manual backup requested by admin");
                
                // Trigger backup process
                await _backupService.CreateAndUploadBackupAsync();
                
                return Ok(new { message = "Backup created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create manual backup");
                return StatusCode(500, new { error = "Failed to create backup" });
            }
        }

        /// <summary>
        /// Get backup statistics
        /// </summary>
        [HttpGet("stats")]
        public async Task<IActionResult> GetBackupStats()
        {
            try
            {
                var backupPrefix = "backups/database/";
                var backupFiles = await _s3Service.ListFilesAsync(backupPrefix);

                var totalSize = backupFiles.Sum(f => f.Size);
                var totalSizeInMB = Math.Round(totalSize / 1024.0 / 1024.0, 2);
                var totalSizeInGB = Math.Round(totalSize / 1024.0 / 1024.0 / 1024.0, 2);

                var oldestBackup = backupFiles
                    .Where(f => f.Key.EndsWith(".sql"))
                    .OrderBy(f => f.LastModified)
                    .FirstOrDefault();

                var newestBackup = backupFiles
                    .Where(f => f.Key.EndsWith(".sql"))
                    .OrderByDescending(f => f.LastModified)
                    .FirstOrDefault();

                return Ok(new
                {
                    TotalBackups = backupFiles.Count(f => f.Key.EndsWith(".sql")),
                    TotalSizeMB = totalSizeInMB,
                    TotalSizeGB = totalSizeInGB,
                    OldestBackup = oldestBackup?.LastModified,
                    NewestBackup = newestBackup?.LastModified,
                    LastBackupFileName = newestBackup != null ? System.IO.Path.GetFileName(newestBackup.Key) : null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve backup statistics");
                return StatusCode(500, new { error = "Failed to retrieve backup statistics" });
            }
        }

        /// <summary>
        /// Download a specific backup file
        /// </summary>
        [HttpGet("download/{fileName}")]
        public async Task<IActionResult> DownloadBackup(string fileName)
        {
            try
            {
                var s3Key = $"backups/database/{fileName}";
                
                // Check if file exists
                var backupFiles = await _s3Service.ListFilesAsync("backups/database/");
                if (!backupFiles.Any(f => f.Key == s3Key))
                {
                    return NotFound(new { error = "Backup file not found" });
                }

                // Get file stream from S3
                var (fileStream, contentType) = await _s3Service.GetFileAsync(s3Key);
                
                return File(fileStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download backup file: {FileName}", fileName);
                return StatusCode(500, new { error = "Failed to download backup file" });
            }
        }

        /// <summary>
        /// Delete a specific backup file
        /// </summary>
        [HttpDelete("delete/{fileName}")]
        public async Task<IActionResult> DeleteBackup(string fileName)
        {
            try
            {
                var s3Key = $"backups/database/{fileName}";
                
                // Check if file exists
                var backupFiles = await _s3Service.ListFilesAsync("backups/database/");
                if (!backupFiles.Any(f => f.Key == s3Key))
                {
                    return NotFound(new { error = "Backup file not found" });
                }

                await _s3Service.DeleteFileAsync(s3Key);
                
                _logger.LogInformation("Backup file deleted by admin: {FileName}", fileName);
                return Ok(new { message = "Backup file deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete backup file: {FileName}", fileName);
                return StatusCode(500, new { error = "Failed to delete backup file" });
            }
        }
    }
} 
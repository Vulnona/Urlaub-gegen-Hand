using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UGHApi.Services.AWS;
using UGHApi.DATA;
using UGHApi.Models;

namespace UGHApi.Services.BackgroundTasks
{
    /// <summary>
    /// Background service that runs daily to create database backups and upload them to S3
    /// </summary>
    public class DatabaseBackupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseBackupService> _logger;
        private readonly IConfiguration _configuration;
        private readonly BackupSettings _backupSettings;
        private readonly TimeSpan _checkInterval;

        public DatabaseBackupService(
            IServiceProvider serviceProvider,
            ILogger<DatabaseBackupService> logger,
            IConfiguration configuration,
            IOptions<BackupSettings> backupSettings)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
            _backupSettings = backupSettings.Value;
            _checkInterval = TimeSpan.FromHours(_backupSettings.BackupIntervalHours);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Database Backup Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                                    if (_backupSettings.EnableAutomaticBackups)
                {
                    await CreateAndUploadBackupAsync();
                    await CleanupOldBackupsAsync();
                }
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Database Backup Service");
                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken); // Retry in 30 minutes
                }
            }

            _logger.LogInformation("Database Backup Service stopped");
        }

        public async Task CreateAndUploadBackupAsync()
        {
            _logger.LogInformation("Starting database backup process...");

            try
            {
                // Create backup file
                var backupFile = await CreateDatabaseBackupAsync();
                
                if (backupFile != null && File.Exists(backupFile))
                {
                    // Upload to S3
                    await UploadBackupToS3Async(backupFile);
                    
                    // Clean up local file
                    File.Delete(backupFile);
                    _logger.LogInformation($"Backup completed and uploaded successfully: {Path.GetFileName(backupFile)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create or upload backup");
                throw;
            }
        }

        private async Task<string> CreateDatabaseBackupAsync()
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
            var backupFileName = $"ugh-db-backup_{timestamp}.sql";
            var backupPath = Path.Combine(Path.GetTempPath(), backupFileName);

            _logger.LogInformation($"Creating backup: {backupFileName}");

            try
            {
                // Get database connection info from configuration
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                
                // Parse MySQL connection string manually
                var server = "db"; // Docker container name
                var database = "db";
                var userId = "user";
                var password = "password";
                
                // Extract from connection string if available
                if (connectionString.Contains("Server="))
                {
                    var serverMatch = System.Text.RegularExpressions.Regex.Match(connectionString, @"Server=([^;]+)");
                    if (serverMatch.Success) server = serverMatch.Groups[1].Value;
                }
                if (connectionString.Contains("Database="))
                {
                    var dbMatch = System.Text.RegularExpressions.Regex.Match(connectionString, @"Database=([^;]+)");
                    if (dbMatch.Success) database = dbMatch.Groups[1].Value;
                }
                if (connectionString.Contains("User ID="))
                {
                    var userMatch = System.Text.RegularExpressions.Regex.Match(connectionString, @"User ID=([^;]+)");
                    if (userMatch.Success) userId = userMatch.Groups[1].Value;
                }
                if (connectionString.Contains("Password="))
                {
                    var passwordMatch = System.Text.RegularExpressions.Regex.Match(connectionString, @"Password=([^;]+)");
                    if (passwordMatch.Success) password = passwordMatch.Groups[1].Value;
                }

                // Create mysqldump command
                var dumpArgs = new[]
                {
                    "--opt",
                    "--no-tablespaces",
                    $"-h{server}",
                    $"-u{userId}",
                    $"-p{password}",
                    database
                };

                // Execute mysqldump
                var startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "mysqldump",
                    Arguments = string.Join(" ", dumpArgs),
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = new System.Diagnostics.Process { StartInfo = startInfo };
                process.Start();

                using var outputFile = File.Create(backupPath);
                await process.StandardOutput.BaseStream.CopyToAsync(outputFile);

                await process.WaitForExitAsync();

                if (process.ExitCode == 0)
                {
                    var fileSize = new FileInfo(backupPath).Length / 1024.0 / 1024.0; // MB
                    _logger.LogInformation($"Backup created successfully: {fileSize:F2} MB");
                    return backupPath;
                }
                else
                {
                    var error = await process.StandardError.ReadToEndAsync();
                    _logger.LogError($"mysqldump failed with exit code {process.ExitCode}: {error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create database backup");
                return null;
            }
        }

        private async Task UploadBackupToS3Async(string backupFilePath)
        {
            using var scope = _serviceProvider.CreateScope();
            var s3Service = scope.ServiceProvider.GetRequiredService<S3Service>();

            try
            {
                var fileName = Path.GetFileName(backupFilePath);
                var s3Key = $"{_backupSettings.S3BackupPrefix}{fileName}";

                _logger.LogInformation($"Uploading backup to S3: {s3Key}");

                using var fileStream = File.OpenRead(backupFilePath);
                await s3Service.UploadFileAsync(fileStream, s3Key, "application/sql");

                _logger.LogInformation($"Backup uploaded successfully to S3: {s3Key}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload backup to S3");
                throw;
            }
        }

        private async Task CleanupOldBackupsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var s3Service = scope.ServiceProvider.GetRequiredService<S3Service>();

            try
            {
                _logger.LogInformation("Cleaning up old backups...");

                var cutoffDate = DateTime.UtcNow.AddDays(-_backupSettings.RetentionDays);
                var backupPrefix = _backupSettings.S3BackupPrefix;

                // List all backup files
                var backupFiles = await s3Service.ListFilesAsync(backupPrefix);

                var deletedCount = 0;
                foreach (var file in backupFiles)
                {
                    // Extract date from filename (ugh-db-backup_2025-01-27_14-30-00.sql)
                    if (TryParseBackupDate(file.Key, out var backupDate))
                    {
                        if (backupDate < cutoffDate)
                        {
                            await s3Service.DeleteFileAsync(file.Key);
                            deletedCount++;
                            _logger.LogInformation($"Deleted old backup: {file.Key}");
                        }
                    }
                }

                _logger.LogInformation($"Cleanup completed: {deletedCount} old backups deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to cleanup old backups");
            }
        }

        private bool TryParseBackupDate(string s3Key, out DateTime backupDate)
        {
            backupDate = DateTime.MinValue;
            
            try
            {
                var fileName = Path.GetFileName(s3Key);
                if (fileName.StartsWith("ugh-db-backup_") && fileName.EndsWith(".sql"))
                {
                    var datePart = fileName.Replace("ugh-db-backup_", "").Replace(".sql", "");
                    if (DateTime.TryParseExact(datePart, "yyyy-MM-dd_HH-mm-ss", 
                        System.Globalization.CultureInfo.InvariantCulture, 
                        System.Globalization.DateTimeStyles.None, out backupDate))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Ignore parsing errors
            }

            return false;
        }
    }
} 
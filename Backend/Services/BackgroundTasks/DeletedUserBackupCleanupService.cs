using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UGHApi.DATA;
using UGHApi.Models;

namespace UGHApi.Services.BackgroundTasks;

public class DeletedUserBackupCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DeletedUserBackupCleanupService> _logger;
    private readonly int _retentionDays;

    public DeletedUserBackupCleanupService(
        IServiceProvider serviceProvider,
        ILogger<DeletedUserBackupCleanupService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        
        // DSGVO-compliant retention period (30 days by default)
        _retentionDays = configuration.GetValue<int>("DSGVOSettings:DeletedUserRetentionDays", 30);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeletedUserBackup Cleanup Service started with {RetentionDays} days retention", _retentionDays);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredBackupsAsync();
                
                // Run every 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cleaning up deleted user backups");
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Retry after 1 hour
            }
        }
    }

    private async Task CleanupExpiredBackupsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<Ugh_Context>();
        
        var cutoffDate = DateTime.UtcNow.AddDays(-_retentionDays);
        
        // Find expired backups
        var expiredBackups = await db.DeletedUserBackups
            .Where(b => b.DeletedAt < cutoffDate)
            .ToListAsync();

        if (expiredBackups.Count > 0)
        {
            _logger.LogInformation("Found {Count} expired DeletedUserBackups older than {RetentionDays} days to delete", 
                expiredBackups.Count, _retentionDays);

            // Delete expired backups
            db.DeletedUserBackups.RemoveRange(expiredBackups);
            await db.SaveChangesAsync();
            
            _logger.LogInformation("Deleted {Count} expired DeletedUserBackups at {DateTime}", 
                expiredBackups.Count, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        else
        {
            _logger.LogInformation("No expired DeletedUserBackups to delete at {DateTime}", 
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
} 
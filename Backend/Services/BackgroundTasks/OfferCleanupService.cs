using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UGHApi.DATA;
using UGH.Domain.Entities;

namespace UGHApi.Services.BackgroundTasks
{
    /// <summary>
    /// Background service that runs daily to delete closed offers older than 3 months
    /// </summary>
    public class OfferCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OfferCleanupService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Check daily
        private readonly int _cleanupDays = 365; // 1 year

        public OfferCleanupService(
            IServiceProvider serviceProvider,
            ILogger<OfferCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Offer Cleanup Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CleanupOldOffersAsync();
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Offer Cleanup Service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Retry in 5 minutes
                }
            }

            _logger.LogInformation("Offer Cleanup Service stopped");
        }

        public async Task CleanupOldOffersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Ugh_Context>();
            
            var cutoffDate = DateTime.UtcNow.AddDays(-_cleanupDays);
            var cutoffDateOnly = DateOnly.FromDateTime(cutoffDate);
            
            // Find closed offers that are older than 3 months
            var oldClosedOffers = db.offers
                .Where(o => o.Status == OfferStatus.Closed && o.ToDate < cutoffDateOnly)
                .ToList();

            if (oldClosedOffers.Count > 0)
            {
                _logger.LogInformation($"Found {oldClosedOffers.Count} closed offers older than {_cleanupDays} days to delete");

                // Delete related pictures first (due to foreign key constraints)
                foreach (var offer in oldClosedOffers)
                {
                    var pictures = db.pictures.Where(p => p.OfferId == offer.Id).ToList();
                    if (pictures.Any())
                    {
                        db.pictures.RemoveRange(pictures);
                        _logger.LogInformation($"Removed {pictures.Count} pictures for offer {offer.Id}");
                    }
                }

                // Delete the offers
                db.offers.RemoveRange(oldClosedOffers);
                await db.SaveChangesAsync();
                
                _logger.LogInformation($"Deleted {oldClosedOffers.Count} old closed offers at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            }
            else
            {
                _logger.LogInformation($"No old closed offers to delete at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            }
        }
    }
} 
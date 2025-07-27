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
    /// Background service that runs daily to close expired offers (ToDate < today)
    /// </summary>
    public class OfferExpirationService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OfferExpirationService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Check daily

        public OfferExpirationService(
            IServiceProvider serviceProvider,
            ILogger<OfferExpirationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Offer Expiration Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ExpireOffersAsync();
                    await Task.Delay(_checkInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in Offer Expiration Service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Retry in 5 minutes
                }
            }

            _logger.LogInformation("Offer Expiration Service stopped");
        }

        public async Task ExpireOffersAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Ugh_Context>();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var expiredOffers = db.offers
                .Where(o => o.Status == OfferStatus.Active && o.ToDate < today)
                .ToList();

            if (expiredOffers.Count > 0)
            {
                foreach (var offer in expiredOffers)
                {
                    offer.Status = OfferStatus.Closed;
                }
                await db.SaveChangesAsync();
                _logger.LogInformation($"Closed {expiredOffers.Count} expired offers at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            }
            else
            {
                _logger.LogInformation($"No expired offers to close at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}");
            }
        }
    }
} 
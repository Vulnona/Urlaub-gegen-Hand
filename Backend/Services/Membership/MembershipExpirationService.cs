using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UGH.Domain.Interfaces;

namespace UGHApi.Services.Membership;

/// <summary>
/// Background service that runs daily to check for expired memberships and set users to inactive
/// </summary>
public class MembershipExpirationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MembershipExpirationService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromHours(24); // Check daily

    public MembershipExpirationService(
        IServiceProvider serviceProvider,
        ILogger<MembershipExpirationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Membership Expiration Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckExpiredMemberships();
                await Task.Delay(_checkInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Membership Expiration Service");
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); // Retry in 5 minutes
            }
        }

        _logger.LogInformation("Membership Expiration Service stopped");
    }

    private async Task CheckExpiredMemberships()
    {
        using var scope = _serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        
        _logger.LogInformation("Checking for expired memberships...");

        try
        {
            var expiredCount = await userRepository.DeactivateExpiredMembershipsAsync();
            
            if (expiredCount > 0)
            {
                _logger.LogInformation($"Deactivated {expiredCount} expired memberships");
            }
            else
            {
                _logger.LogDebug("No expired memberships found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check expired memberships");
        }
    }
}

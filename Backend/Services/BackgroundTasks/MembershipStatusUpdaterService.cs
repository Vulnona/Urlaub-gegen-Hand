//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;

//public class MembershipStatusUpdaterService : BackgroundService
//{
//    private readonly IServiceProvider _serviceProvider;
//    private readonly ILogger<MembershipStatusUpdaterService> _logger;

//    public MembershipStatusUpdaterService(IServiceProvider serviceProvider, ILogger<MembershipStatusUpdaterService> logger)
//    {
//        _serviceProvider = serviceProvider;
//        _logger = logger;
//    }

//    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            try
//            {
//                await UpdateMembershipStatusesAsync();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An error occurred while updating membership statuses.");
//            }

//            // Delay for a set period (e.g., run every 24 hours)
//            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
//        }
//    }

//    private async Task UpdateMembershipStatusesAsync()
//    {
//        using (var scope = _serviceProvider.CreateScope())
//        {
//            var context = scope.ServiceProvider.GetRequiredService<Ugh_Context>();

//            // Find memberships that are expired
//            var expiredMemberships = context.memberships
//                .Where(um => um.CreatedAt <= DateTime.Now && um.Status != "Expired")
//                .ToList();

//            foreach (var membership in expiredMemberships)
//            {
//                // Update status to "Expired"
//                membership.Status = "Expired";
//                membership.IsMembershipActive = false;
//                membership.UpdatedAt = DateTime.Now;
//            }

//            // Save changes to the database
//            await context.SaveChangesAsync();
//        }
//    }
//}

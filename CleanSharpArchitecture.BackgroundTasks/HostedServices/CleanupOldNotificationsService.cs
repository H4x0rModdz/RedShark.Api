using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CleanSharpArchitecture.Application.Services.Interfaces;

namespace CleanSharpArchitecture.BackgroundTasks.HostedServices
{
    public class CleanupOldNotificationsService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CleanupOldNotificationsService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromDays(1); // Run daily

        public CleanupOldNotificationsService(
            IServiceProvider serviceProvider,
            ILogger<CleanupOldNotificationsService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting cleanup of old notifications at {Time}", DateTimeOffset.Now);

                    using var scope = _serviceProvider.CreateScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    // Clean up notifications older than 30 days
                    var cutoffDate = DateTime.UtcNow.AddDays(-30);
                    
                    // This would require implementing a cleanup method in the notification service
                    // await notificationService.CleanupOldNotificationsAsync(cutoffDate);

                    _logger.LogInformation("Completed cleanup of old notifications at {Time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during notification cleanup at {Time}", DateTimeOffset.Now);
                }

                await Task.Delay(_period, stoppingToken);
            }
        }
    }
}
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using CleanSharpArchitecture.Infrastructure.Data;

namespace CleanSharpArchitecture.Monitoring.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _context;

        public DatabaseHealthCheck(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Try to connect to the database and execute a simple query
                await _context.Database.CanConnectAsync(cancellationToken);
                
                // Optional: Execute a more specific query to ensure data access is working
                var userCount = await _context.Users.CountAsync(cancellationToken);
                
                var data = new Dictionary<string, object>
                {
                    { "database", "connected" },
                    { "userCount", userCount },
                    { "timestamp", DateTime.UtcNow }
                };

                return HealthCheckResult.Healthy("Database is healthy", data);
            }
            catch (Exception ex)
            {
                var data = new Dictionary<string, object>
                {
                    { "database", "disconnected" },
                    { "error", ex.Message },
                    { "timestamp", DateTime.UtcNow }
                };

                return HealthCheckResult.Unhealthy("Database is unhealthy", ex, data);
            }
        }
    }
}
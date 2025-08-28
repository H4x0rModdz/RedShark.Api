using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CleanSharpArchitecture.Monitoring.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>(
                    name: "database",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "ready", "database" })
                .AddCheck<BlobStorageHealthCheck>(
                    name: "blob-storage",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "ready", "storage" })
                .AddCheck("application", () => HealthCheckResult.Healthy("Application is running"), tags: new[] { "live" });

            return services;
        }
    }
}
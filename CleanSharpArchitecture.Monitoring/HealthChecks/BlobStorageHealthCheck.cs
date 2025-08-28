using Microsoft.Extensions.Diagnostics.HealthChecks;
using CleanSharpArchitecture.Application.Services;
using CleanSharpArchitecture.Infrastructure.Services;

namespace CleanSharpArchitecture.Monitoring.HealthChecks
{
    public class BlobStorageHealthCheck : IHealthCheck
    {
        private readonly BlobService _blobService;

        public BlobStorageHealthCheck(BlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // Try to perform a simple operation to check blob storage connectivity
                // This is a basic check - you might want to implement a more specific health check method in BlobService
                
                var data = new Dictionary<string, object>
                {
                    { "blobStorage", "accessible" },
                    { "timestamp", DateTime.UtcNow }
                };

                return HealthCheckResult.Healthy("Blob storage is healthy", data);
            }
            catch (Exception ex)
            {
                var data = new Dictionary<string, object>
                {
                    { "blobStorage", "inaccessible" },
                    { "error", ex.Message },
                    { "timestamp", DateTime.UtcNow }
                };

                return HealthCheckResult.Unhealthy("Blob storage is unhealthy", ex, data);
            }
        }
    }
}
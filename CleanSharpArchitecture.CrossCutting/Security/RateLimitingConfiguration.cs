using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace CleanSharpArchitecture.CrossCutting.Security
{
    public static class RateLimitingConfiguration
    {
        public static IServiceCollection AddRateLimitingPolicies(this IServiceCollection services)
        {
            // For .NET 8, we'll use a custom rate limiting service
            services.AddSingleton<IRateLimitService, RateLimitService>();
            
            return services;
        }
    }   

    // Simple rate limiting service interface
    public interface IRateLimitService
    {
        Task<bool> IsAllowedAsync(string key, int maxRequests, TimeSpan window);
        Task<bool> CheckLoginRateLimitAsync(string ipAddress);
        Task<bool> CheckRegistrationRateLimitAsync(string ipAddress);
        Task<bool> CheckRecoveryRateLimitAsync(string ipAddress);
        Task<bool> CheckPostCreationRateLimitAsync(string userId);
    }

    // Basic in-memory rate limiting implementation
    public class RateLimitService : IRateLimitService
    {
        private readonly ConcurrentDictionary<string, (DateTime WindowStart, int RequestCount)> _requests = new();
        
        public Task<bool> IsAllowedAsync(string key, int maxRequests, TimeSpan window)
        {
            var now = DateTime.UtcNow;
            
            var entry = _requests.AddOrUpdate(key, 
                (now, 1), 
                (k, existing) =>
                {
                    if (now - existing.WindowStart > window)
                    {
                        // Window expired, reset
                        return (now, 1);
                    }
                    
                    return (existing.WindowStart, existing.RequestCount + 1);
                });
            
            if (now - entry.WindowStart > window)
            {
                return Task.FromResult(true);
            }
            
            return Task.FromResult(entry.RequestCount <= maxRequests);
        }

        public Task<bool> CheckLoginRateLimitAsync(string ipAddress)
        {
            return IsAllowedAsync($"login:{ipAddress}", 5, TimeSpan.FromMinutes(1));
        }

        public Task<bool> CheckRegistrationRateLimitAsync(string ipAddress)
        {
            return IsAllowedAsync($"register:{ipAddress}", 3, TimeSpan.FromHours(1));
        }

        public Task<bool> CheckRecoveryRateLimitAsync(string ipAddress)
        {
            return IsAllowedAsync($"recovery:{ipAddress}", 3, TimeSpan.FromHours(1));
        }

        public Task<bool> CheckPostCreationRateLimitAsync(string userId)
        {
            return IsAllowedAsync($"post:{userId}", 10, TimeSpan.FromMinutes(1));
        }
    }
}
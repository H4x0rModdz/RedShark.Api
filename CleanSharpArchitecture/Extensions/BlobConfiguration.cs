using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Infrastructure.Interfaces.Services;
using CleanSharpArchitecture.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSharpArchitecture.API.Extensions
{
    public static class BlobConfiguration
    {
        public static IServiceCollection AddBlobConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<BlobService>();
            services.AddScoped<IBlobService, BlobService>();
            services.Configure<BlobSettings>(configuration.GetSection("BlobSettings"));
            services.AddSingleton<IBlobClientFactory, BlobClientFactory>();

            return services;
        }
    }
}

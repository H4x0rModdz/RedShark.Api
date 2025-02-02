using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSharpArchitecture.API.Extensions
{
    public static class AzureAdConfiguration
    {
        public static IServiceCollection AddAzureAdConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddMicrosoftGraph(configuration.GetSection("MicrosoftGraph"))
                .AddInMemoryTokenCaches();

            return services;
        }
    }
}

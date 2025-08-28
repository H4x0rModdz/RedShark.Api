using FluentValidation;
using CleanSharpArchitecture.Application.Validations;
using CleanSharpArchitecture.CrossCutting.ExceptionHandling;
using CleanSharpArchitecture.CrossCutting.Security;
using CleanSharpArchitecture.Monitoring.HealthChecks;
using CleanSharpArchitecture.BackgroundTasks.HostedServices;
using CleanSharpArchitecture.Application.UseCases;
using FluentValidation.AspNetCore;

namespace CleanSharpArchitecture.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddArchitecturalComponents(this IServiceCollection services, IConfiguration configuration)
        {
            // Exception Handling
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            // FluentValidation
            services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            // Rate Limiting
            services.AddRateLimitingPolicies();

            // Health Checks
            var connectionString = configuration.GetConnectionString("Default");
            services.AddCustomHealthChecks(connectionString ?? string.Empty);

            // Background Services
            services.AddHostedService<CleanupOldNotificationsService>();

            // Use Cases (CQRS)
            services.AddScoped<ICreatePostUseCase, CreatePostUseCase>();

            return services;
        }
    }
}
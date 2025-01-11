using RedShark.Domain.Factories;
using RedShark.Domain.Policies;
using RedShark.Shared.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace RedShark.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddCommands();
        services.AddSingleton<ISampleEntityFactory, SampleEntityFactory>();

        services.Scan(b => b.FromAssemblies(typeof(ISampleEntityItemsPolicy).Assembly)
            .AddClasses(c => c.AssignableTo<ISampleEntityItemsPolicy>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        return services;
    }
}

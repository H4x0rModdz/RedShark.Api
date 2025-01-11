using RedShark.Application.Services;
using RedShark.Domain.Repositories;
using RedShark.Infrastructure.EF.Contexts;
using RedShark.Infrastructure.EF.Options;
using RedShark.Infrastructure.EF.Repositories;
using RedShark.Infrastructure.EF.Services;
using RedShark.Shared.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RedShark.Infrastructure.EF;

internal static class Extensions
{
    public static IServiceCollection AddSQLDB(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISampleEntityRepository, SampleEntityRepository>();
        services.AddScoped<ISampleEntityReadService, SampleEntityReadService>();

        var options = configuration.GetOptions<DataBaseOptions>("DataBaseConnectionString");
        services.AddDbContext<ReadDbContext>(ctx =>
        ctx.UseSqlServer(options.ConnectionString));

        services.AddDbContext<WriteDbContext>(ctx =>
            ctx.UseSqlServer(options.ConnectionString));

        return services;
    }
}

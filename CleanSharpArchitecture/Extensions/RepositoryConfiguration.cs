using CleanSharpArchitecture.Application.Interfaces.Repositories;
using CleanSharpArchitecture.Application.Repositories.Interfaces;
using CleanSharpArchitecture.Domain.Interfaces;
using CleanSharpArchitecture.Infrastructure.Repositories;
using CleanSharpArchitecture.Infrastructure.Repositories.CleanSharpArchitecture.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CleanSharpArchitecture.API.Extensions
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositoryConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ILikeRepository, LikeRepository>();
            services.AddScoped<IFeedRepository, FeedRepository>();
            services.AddScoped<IFollowerRepository, FollowerRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }
    }
}

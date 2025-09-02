using CleanSharpArchitecture.Application.Mappings;
using CleanSharpArchitecture.Application.Services;
using CleanSharpArchitecture.Application.Services.Interfaces;
using CleanSharpArchitecture.Infrastructure.Services;

namespace CleanSharpArchitecture.API.Extensions
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServiceConfigurations(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserPhotoService, UserPhotoService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IFeedService, FeedService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFollowerService, FollowerService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddSignalR();

            services.AddAutoMapper(typeof(SourceMapping));

            return services;
        }
    }
}

using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Check if database already has data - if so, skip seeding
            if (await context.Users.AnyAsync())
                return; // Database already seeded

            // Seed in order of dependencies - each seeder checks if data already exists
            var users = await UserSeeder.SeedAsync(context);
            
            // User related data
            await UserPhotoSeeder.SeedAsync(context, users);
            
            var interests = await InterestSeeder.SeedAsync(context);
            await UserInterestSeeder.SeedAsync(context, users, interests);
            
            // Posts and related data
            var posts = await PostSeeder.SeedAsync(context, users);
            await PostImageSeeder.SeedAsync(context, posts);
            await CommentSeeder.SeedAsync(context, users, posts);
            await LikeSeeder.SeedAsync(context, users, posts);
            
            // Social features
            await FollowerSeeder.SeedAsync(context, users);
            
            // Badges and gamification
            var badges = await BadgeSeeder.SeedAsync(context);
            await UserBadgeSeeder.SeedAsync(context, users, badges);
            
            // Chat system
            var chats = await ChatSeeder.SeedAsync(context, users);
            await UserChatSeeder.SeedAsync(context, users, chats);
            await ChatMessageSeeder.SeedAsync(context, users, chats);
            
            // Notifications (should be last)
            await NotificationSeeder.SeedAsync(context, users);
        }
    }
}
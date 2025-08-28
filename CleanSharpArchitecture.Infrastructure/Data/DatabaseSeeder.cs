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

            // Check if data already exists
            if (await context.Users.AnyAsync())
            {
                return; // Database has been seeded
            }

            // Seed in order of dependencies
            var users = await UserSeeder.SeedAsync(context);
            var interests = await InterestSeeder.SeedAsync(context);
            await UserInterestSeeder.SeedAsync(context, users, interests);
            
            var posts = await PostSeeder.SeedAsync(context, users);
            await CommentSeeder.SeedAsync(context, users, posts);
            await LikeSeeder.SeedAsync(context, users, posts);
            
            await FollowerSeeder.SeedAsync(context, users);
            
            var badges = await BadgeSeeder.SeedAsync(context);
            await UserBadgeSeeder.SeedAsync(context, users, badges);
            
            await NotificationSeeder.SeedAsync(context, users);
        }
    }
}
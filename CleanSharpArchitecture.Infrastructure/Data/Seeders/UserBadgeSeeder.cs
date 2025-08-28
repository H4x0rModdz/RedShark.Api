using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class UserBadgeSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users, List<Badge> badges)
        {
            var userBadges = new List<UserBadge>
            {
                // Early Adopter badges for first 5 users
                new UserBadge { UserId = users[0].Id, BadgeId = badges[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-30) }, // John - Early Adopter
                new UserBadge { UserId = users[1].Id, BadgeId = badges[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-25) }, // Jane - Early Adopter
                new UserBadge { UserId = users[2].Id, BadgeId = badges[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-20) }, // Mike - Early Adopter
                new UserBadge { UserId = users[3].Id, BadgeId = badges[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-15) }, // Sarah - Early Adopter
                new UserBadge { UserId = users[4].Id, BadgeId = badges[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-10) }, // Alex - Early Adopter
                
                // Content Creator badges
                new UserBadge { UserId = users[0].Id, BadgeId = badges[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) }, // John - Content Creator
                
                // Tech Expert badges for technical contributors
                new UserBadge { UserId = users[2].Id, BadgeId = badges[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },  // Mike - Tech Expert
                new UserBadge { UserId = users[7].Id, BadgeId = badges[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) },  // Sophia - Tech Expert
                new UserBadge { UserId = users[8].Id, BadgeId = badges[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },  // Ryan - Tech Expert
                new UserBadge { UserId = users[4].Id, BadgeId = badges[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },  // Alex - Tech Expert
                
                // Community Leader badge
                new UserBadge { UserId = users[1].Id, BadgeId = badges[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },  // Jane - Community Leader
                
                // Social Butterfly badges for active followers
                new UserBadge { UserId = users[0].Id, BadgeId = badges[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },  // John - Social Butterfly
                new UserBadge { UserId = users[1].Id, BadgeId = badges[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) }   // Jane - Social Butterfly
            };

            context.UserBadges.AddRange(userBadges);
            await context.SaveChangesAsync();
        }
    }
}
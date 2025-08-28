using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class BadgeSeeder
    {
        public static async Task<List<Badge>> SeedAsync(ApplicationDbContext context)
        {
            var badges = new List<Badge>
            {
                new Badge
                {
                    Name = "Early Adopter",
                    Description = "One of the first users to join Red-Shark",
                    IconUrl = "🚀",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Badge
                {
                    Name = "Content Creator",
                    Description = "Created 10+ posts",
                    IconUrl = "✍️",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Badge
                {
                    Name = "Social Butterfly",
                    Description = "Following 20+ users",
                    IconUrl = "🦋",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Badge
                {
                    Name = "Tech Expert",
                    Description = "Recognized for technical expertise and helpful insights",
                    IconUrl = "🔧",
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new Badge
                {
                    Name = "Community Leader",
                    Description = "Active member who helps grow the community",
                    IconUrl = "👑",
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                }
            };

            context.Badges.AddRange(badges);
            await context.SaveChangesAsync();
            
            return badges;
        }
    }
}
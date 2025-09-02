using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class ChatSeeder
    {
        public static async Task<List<Chat>> SeedAsync(ApplicationDbContext context, List<User> users)
        {
            if (context.Chats.Any()) return new List<Chat>();

            var chats = new List<Chat>
            {
                new Chat
                {
                    Name = "Development Team",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Chat
                {
                    Name = "Design Collaboration",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Chat
                {
                    Name = "Project Alpha",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddHours(-3)
                },
                new Chat
                {
                    Name = "Private Chat", // Direct chat between John and Jane
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddHours(-6)
                },
                new Chat
                {
                    Name = "Private Chat", // Direct chat between Mike and Alex
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Chat
                {
                    Name = "Private Chat", // Direct chat between Emma and Sarah
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddHours(-12)
                },
                new Chat
                {
                    Name = "Security Team",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Chat
                {
                    Name = "Private Chat", // Direct chat between Lucas and Sophia
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddHours(-4)
                }
            };

            context.Chats.AddRange(chats);
            await context.SaveChangesAsync();
            
            return chats;
        }
    }
}
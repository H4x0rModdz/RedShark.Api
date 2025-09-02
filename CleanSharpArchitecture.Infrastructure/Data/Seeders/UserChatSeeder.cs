using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class UserChatSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users, List<Chat> chats)
        {
            if (context.UserChats.Any()) return;

            var userChats = new List<UserChat>
            {
                // Development Team Chat - John, Mike, Alex, Ryan, Olivia
                new UserChat
                {
                    UserId = users[0].Id, // John
                    ChatId = chats[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new UserChat
                {
                    UserId = users[2].Id, // Mike
                    ChatId = chats[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new UserChat
                {
                    UserId = users[4].Id, // Alex
                    ChatId = chats[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-19),
                    UpdatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new UserChat
                {
                    UserId = users[8].Id, // Ryan
                    ChatId = chats[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new UserChat
                {
                    UserId = users[9].Id, // Olivia
                    ChatId = chats[0].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-17),
                    UpdatedAt = DateTime.UtcNow.AddDays(-17)
                },

                // Design Collaboration Chat - Jane, Sarah, Lucas
                new UserChat
                {
                    UserId = users[1].Id, // Jane
                    ChatId = chats[1].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new UserChat
                {
                    UserId = users[3].Id, // Sarah
                    ChatId = chats[1].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new UserChat
                {
                    UserId = users[6].Id, // Lucas
                    ChatId = chats[1].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-14),
                    UpdatedAt = DateTime.UtcNow.AddDays(-14)
                },

                // Project Alpha Chat - John, Jane, Sarah, Emma
                new UserChat
                {
                    UserId = users[0].Id, // John
                    ChatId = chats[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new UserChat
                {
                    UserId = users[1].Id, // Jane
                    ChatId = chats[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new UserChat
                {
                    UserId = users[3].Id, // Sarah
                    ChatId = chats[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new UserChat
                {
                    UserId = users[5].Id, // Emma
                    ChatId = chats[2].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = DateTime.UtcNow.AddDays(-9)
                },

                // Direct chat: John and Jane
                new UserChat
                {
                    UserId = users[0].Id, // John
                    ChatId = chats[3].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new UserChat
                {
                    UserId = users[1].Id, // Jane
                    ChatId = chats[3].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },

                // Direct chat: Mike and Alex
                new UserChat
                {
                    UserId = users[2].Id, // Mike
                    ChatId = chats[4].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new UserChat
                {
                    UserId = users[4].Id, // Alex
                    ChatId = chats[4].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },

                // Direct chat: Emma and Sarah
                new UserChat
                {
                    UserId = users[5].Id, // Emma
                    ChatId = chats[5].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-12)
                },
                new UserChat
                {
                    UserId = users[3].Id, // Sarah
                    ChatId = chats[5].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-12)
                },

                // Security Team Chat - Sophia, Alex, Ryan
                new UserChat
                {
                    UserId = users[7].Id, // Sophia
                    ChatId = chats[6].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new UserChat
                {
                    UserId = users[4].Id, // Alex
                    ChatId = chats[6].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new UserChat
                {
                    UserId = users[8].Id, // Ryan
                    ChatId = chats[6].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-7)
                },

                // Direct chat: Lucas and Sophia
                new UserChat
                {
                    UserId = users[6].Id, // Lucas
                    ChatId = chats[7].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new UserChat
                {
                    UserId = users[7].Id, // Sophia
                    ChatId = chats[7].Id,
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                }
            };

            context.UserChats.AddRange(userChats);
            await context.SaveChangesAsync();
        }
    }
}
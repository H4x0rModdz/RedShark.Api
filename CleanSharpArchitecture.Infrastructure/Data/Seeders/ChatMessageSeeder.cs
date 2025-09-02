using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class ChatMessageSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users, List<Chat> chats)
        {
            if (context.ChatMessages.Any()) return;

            var chatMessages = new List<ChatMessage>
            {
                // Development Team Chat Messages
                new ChatMessage
                {
                    ChatId = chats[0].Id,
                    UserId = users[0].Id, // John
                    Content = "Good morning team! Let's discuss today's sprint goals.",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new ChatMessage
                {
                    ChatId = chats[0].Id,
                    UserId = users[2].Id, // Mike
                    Content = "I've completed the user authentication module. Ready for code review!",
                    CreatedAt = DateTime.UtcNow.AddDays(-5).AddMinutes(15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5).AddMinutes(15)
                },
                new ChatMessage
                {
                    ChatId = chats[0].Id,
                    UserId = users[4].Id, // Alex
                    Content = "Great work Mike! I'll review it after lunch. Also, the CI/CD pipeline is now fully automated.",
                    CreatedAt = DateTime.UtcNow.AddDays(-5).AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5).AddHours(1)
                },
                new ChatMessage
                {
                    ChatId = chats[0].Id,
                    UserId = users[8].Id, // Ryan
                    Content = "The new microservice architecture is performing well. Latency reduced by 40%!",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new ChatMessage
                {
                    ChatId = chats[0].Id,
                    UserId = users[9].Id, // Olivia
                    Content = "Frontend components are now fully accessible. WCAG 2.1 AA compliant! 🎉",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },

                // Design Collaboration Chat Messages
                new ChatMessage
                {
                    ChatId = chats[1].Id,
                    UserId = users[1].Id, // Jane
                    Content = "I've updated the design system with new color tokens. Please check the Figma file.",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new ChatMessage
                {
                    ChatId = chats[1].Id,
                    UserId = users[3].Id, // Sarah
                    Content = "The new user flow looks amazing! This will definitely improve conversion rates.",
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new ChatMessage
                {
                    ChatId = chats[1].Id,
                    UserId = users[6].Id, // Lucas
                    Content = "I can start implementing the mobile designs next week. React Native components are ready.",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },

                // Project Alpha Chat Messages
                new ChatMessage
                {
                    ChatId = chats[2].Id,
                    UserId = users[0].Id, // John
                    Content = "Project Alpha kickoff meeting scheduled for tomorrow at 10 AM.",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new ChatMessage
                {
                    ChatId = chats[2].Id,
                    UserId = users[5].Id, // Emma
                    Content = "I've prepared the initial data analysis. The ML models show promising results!",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new ChatMessage
                {
                    ChatId = chats[2].Id,
                    UserId = users[1].Id, // Jane
                    Content = "Wireframes are ready for review. The user interface is clean and intuitive.",
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new ChatMessage
                {
                    ChatId = chats[2].Id,
                    UserId = users[3].Id, // Sarah
                    Content = "Stakeholders are very excited about the progress. Demo scheduled for next Friday!",
                    CreatedAt = DateTime.UtcNow.AddHours(-3),
                    UpdatedAt = DateTime.UtcNow.AddHours(-3)
                },

                // Direct Chat: John and Jane
                new ChatMessage
                {
                    ChatId = chats[3].Id,
                    UserId = users[0].Id, // John
                    Content = "Hey Jane! Do you have a moment to discuss the API integration?",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new ChatMessage
                {
                    ChatId = chats[3].Id,
                    UserId = users[1].Id, // Jane
                    Content = "Sure! I'm free now. The endpoints are well documented, should be straightforward.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2).AddMinutes(5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2).AddMinutes(5)
                },
                new ChatMessage
                {
                    ChatId = chats[3].Id,
                    UserId = users[0].Id, // John
                    Content = "Perfect! I'll set up a quick call. Thanks for the quick response! 👍",
                    CreatedAt = DateTime.UtcNow.AddHours(-6),
                    UpdatedAt = DateTime.UtcNow.AddHours(-6)
                },

                // Direct Chat: Mike and Alex
                new ChatMessage
                {
                    ChatId = chats[4].Id,
                    UserId = users[2].Id, // Mike
                    Content = "Alex, the deployment failed again. Can you check the Docker configuration?",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new ChatMessage
                {
                    ChatId = chats[4].Id,
                    UserId = users[4].Id, // Alex
                    Content = "On it! I think it's a memory limit issue. Increasing the allocation now.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1).AddMinutes(10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1).AddMinutes(10)
                },
                new ChatMessage
                {
                    ChatId = chats[4].Id,
                    UserId = users[4].Id, // Alex
                    Content = "Fixed! The deployment is now successful. Updated the Kubernetes config as well.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1).AddHours(1)
                },

                // Security Team Chat Messages
                new ChatMessage
                {
                    ChatId = chats[6].Id,
                    UserId = users[7].Id, // Sophia
                    Content = "Security audit completed. Found 2 minor vulnerabilities, already patched.",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new ChatMessage
                {
                    ChatId = chats[6].Id,
                    UserId = users[4].Id, // Alex
                    Content = "Thanks Sophia! I've updated the security policies in our infrastructure as code.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },

                // Direct Chat: Lucas and Sophia
                new ChatMessage
                {
                    ChatId = chats[7].Id,
                    UserId = users[6].Id, // Lucas
                    Content = "Sophia, can you review the mobile app security before we release to beta?",
                    CreatedAt = DateTime.UtcNow.AddHours(-8),
                    UpdatedAt = DateTime.UtcNow.AddHours(-8)
                },
                new ChatMessage
                {
                    ChatId = chats[7].Id,
                    UserId = users[7].Id, // Sophia
                    Content = "Absolutely! I'll run a penetration test this afternoon and send you the report.",
                    CreatedAt = DateTime.UtcNow.AddHours(-4),
                    UpdatedAt = DateTime.UtcNow.AddHours(-4)
                }
            };

            context.ChatMessages.AddRange(chatMessages);
            await context.SaveChangesAsync();
        }
    }
}
using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class NotificationSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users)
        {
            var notifications = new List<Notification>
            {
                // Welcome notifications
                new Notification
                {
                    UserId = users[0].Id,
                    Content = "Welcome to Red-Shark! Start by following some users and sharing your first post.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                
                // Follow notifications
                new Notification
                {
                    UserId = users[0].Id,
                    Content = "Jane Smith started following you!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new Notification
                {
                    UserId = users[0].Id,
                    Content = "Mike Wilson started following you!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new Notification
                {
                    UserId = users[1].Id,
                    Content = "John Doe started following you!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-19)
                },
                
                // Like notifications
                new Notification
                {
                    UserId = users[0].Id,
                    Content = "Your post received a like from Mike Wilson!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Notification
                {
                    UserId = users[1].Id,
                    Content = "John Doe liked your UI design post!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                
                // Comment notifications
                new Notification
                {
                    UserId = users[1].Id,
                    Content = "John Doe commented on your post!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Notification
                {
                    UserId = users[2].Id,
                    Content = "Sarah Johnson commented on your security post!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                
                // Badge notifications
                new Notification
                {
                    UserId = users[2].Id,
                    Content = "You earned the 'Early Adopter' badge!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new Notification
                {
                    UserId = users[7].Id,
                    Content = "You earned the 'Tech Expert' badge for your security insights!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Notification
                {
                    UserId = users[1].Id,
                    Content = "You earned the 'Community Leader' badge! Keep up the great work!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                
                // Recent activity notifications
                new Notification
                {
                    UserId = users[5].Id,
                    Content = "Lucas Martinez commented on your machine learning post!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-90)
                },
                new Notification
                {
                    UserId = users[4].Id,
                    Content = "Your DevOps post got 5 new likes!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddHours(-10)
                },
                new Notification
                {
                    UserId = users[9].Id,
                    Content = "Your accessibility post received recognition from the community!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5)
                },
                new Notification
                {
                    UserId = users[4].Id,
                    Content = "Your observability post reached 100 views!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5)
                }
            };

            context.Notifications.AddRange(notifications);
            await context.SaveChangesAsync();
        }
    }
}
using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class UserPhotoSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users)
        {
            if (context.UserPhotos.Any()) return;

            var userPhotos = new List<UserPhoto>
            {
                // John Doe's photos
                new UserPhoto
                {
                    UserId = users[0].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1551434678-e076c223a692?w=800",
                    Description = "Working on my latest project",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new UserPhoto
                {
                    UserId = users[0].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?w=800",
                    Description = "Team building event at the office",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },

                // Jane Smith's photos
                new UserPhoto
                {
                    UserId = users[1].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=800",
                    Description = "UI design workshop with the team",
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },
                new UserPhoto
                {
                    UserId = users[1].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=800",
                    Description = "Sketching new ideas for the mobile app",
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-12)
                },

                // Mike Wilson's photos
                new UserPhoto
                {
                    UserId = users[2].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800",
                    Description = "Late night coding session",
                    CreatedAt = DateTime.UtcNow.AddDays(-16),
                    UpdatedAt = DateTime.UtcNow.AddDays(-16)
                },
                new UserPhoto
                {
                    UserId = users[2].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1531297484001-80022131f5a1?w=800",
                    Description = "New setup for better productivity",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },

                // Sarah Johnson's photos
                new UserPhoto
                {
                    UserId = users[3].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1560472354-b33ff0c44a43?w=800",
                    Description = "Strategic planning meeting",
                    CreatedAt = DateTime.UtcNow.AddDays(-14),
                    UpdatedAt = DateTime.UtcNow.AddDays(-14)
                },

                // Alex Brown's photos
                new UserPhoto
                {
                    UserId = users[4].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=800",
                    Description = "Monitoring server infrastructure",
                    CreatedAt = DateTime.UtcNow.AddDays(-9),
                    UpdatedAt = DateTime.UtcNow.AddDays(-9)
                },
                new UserPhoto
                {
                    UserId = users[4].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1667372393119-3d4c48d07fc9?w=800",
                    Description = "Kubernetes cluster deployment successful",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },

                // Emma Davis's photos
                new UserPhoto
                {
                    UserId = users[5].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=800",
                    Description = "Data visualization dashboard",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    UpdatedAt = DateTime.UtcNow.AddDays(-7)
                },

                // Lucas Martinez's photos
                new UserPhoto
                {
                    UserId = users[6].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=800",
                    Description = "Mobile app testing on different devices",
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },

                // Sophia Garcia's photos
                new UserPhoto
                {
                    UserId = users[7].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=800",
                    Description = "Security audit in progress",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                }
            };

            context.UserPhotos.AddRange(userPhotos);
            await context.SaveChangesAsync();
        }
    }
}
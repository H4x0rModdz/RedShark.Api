using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Posts;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

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

            var password = BCrypt.Net.BCrypt.HashPassword("Password123!");

            // Seed Users
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    UserName = "john_doe",
                    Name = "John Doe",
                    Email = "john.doe@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://github.com/shadcn.png",
                    Biography = "Software developer passionate about clean architecture and social media.",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new User
                {
                    Id = 2,
                    UserName = "jane_smith",
                    Name = "Jane Smith",
                    Email = "jane.smith@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://github.com/shadcn.png",
                    Biography = "UI/UX Designer who loves creating beautiful user experiences.",
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new User
                {
                    Id = 3,
                    UserName = "mike_wilson",
                    Name = "Mike Wilson",
                    Email = "mike.wilson@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://github.com/shadcn.png",
                    Biography = "Full-stack developer and tech enthusiast.",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new User
                {
                    Id = 4,
                    UserName = "sarah_johnson",
                    Name = "Sarah Johnson",
                    Email = "sarah.johnson@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://github.com/shadcn.png",
                    Biography = "Product Manager with a passion for innovation.",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new User
                {
                    Id = 5,
                    UserName = "alex_brown",
                    Name = "Alex Brown",
                    Email = "alex.brown@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://github.com/shadcn.png",
                    Biography = "DevOps engineer focused on scalable infrastructure.",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();

            // Seed Posts
            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    UserId = 1,
                    Content = "Welcome to Red-Shark! This is my first post on this amazing social media platform. Excited to connect with everyone! 🚀",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Post
                {
                    Id = 2,
                    UserId = 2,
                    Content = "Just finished designing the new user interface for our project. Clean Architecture really helps with maintainable code! 💻",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Post
                {
                    Id = 3,
                    UserId = 3,
                    Content = "Working on implementing JWT authentication with refresh tokens. Security is crucial for any social media platform! 🔐",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Post
                {
                    Id = 4,
                    UserId = 1,
                    Content = "Today I learned about FluentValidation and how it makes input validation so much cleaner. Love discovering new tools! 📚",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Post
                {
                    Id = 5,
                    UserId = 4,
                    Content = "Product roadmap is looking great! We're planning some exciting features for the next quarter. Stay tuned! ✨",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Post
                {
                    Id = 6,
                    UserId = 5,
                    Content = "Setting up Docker containers and Kubernetes for our deployment pipeline. DevOps automation saves so much time! ⚙️",
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    UpdatedAt = DateTime.UtcNow.AddHours(-12)
                },
                new Post
                {
                    Id = 7,
                    UserId = 2,
                    Content = "User experience is everything! Spent the day improving the mobile responsiveness of our design system. 📱",
                    CreatedAt = DateTime.UtcNow.AddHours(-6),
                    UpdatedAt = DateTime.UtcNow.AddHours(-6)
                },
                new Post
                {
                    Id = 8,
                    UserId = 3,
                    Content = "Rate limiting implementation is complete! Now our API is protected against abuse and performs much better. 🛡️",
                    CreatedAt = DateTime.UtcNow.AddHours(-3),
                    UpdatedAt = DateTime.UtcNow.AddHours(-3)
                }
            };

            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();

            // Seed Likes
            var likes = new List<Like>
            {
                new Like { Id = 1, UserId = 2, PostId = 1, CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new Like { Id = 2, UserId = 3, PostId = 1, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new Like { Id = 3, UserId = 4, PostId = 1, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new Like { Id = 4, UserId = 1, PostId = 2, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new Like { Id = 5, UserId = 3, PostId = 2, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new Like { Id = 6, UserId = 1, PostId = 3, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new Like { Id = 7, UserId = 2, PostId = 3, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Like { Id = 8, UserId = 4, PostId = 3, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Like { Id = 9, UserId = 5, PostId = 4, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Like { Id = 10, UserId = 2, PostId = 4, CreatedAt = DateTime.UtcNow.AddDays(-1) }
            };

            context.Likes.AddRange(likes);
            await context.SaveChangesAsync();

            // Seed Comments
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    UserId = 2,
                    PostId = 1,
                    Content = "Welcome to the community! Great to have you here! 👋",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Comment
                {
                    Id = 2,
                    UserId = 3,
                    PostId = 1,
                    Content = "Looking forward to your future posts!",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Comment
                {
                    Id = 3,
                    UserId = 1,
                    PostId = 2,
                    Content = "Your designs are always amazing! Can't wait to see the final result.",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Comment
                {
                    Id = 4,
                    UserId = 4,
                    PostId = 3,
                    Content = "Security is indeed crucial. Great work on the JWT implementation!",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Comment
                {
                    Id = 5,
                    UserId = 3,
                    PostId = 4,
                    Content = "FluentValidation is fantastic! It makes validation so much cleaner and testable.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.Comments.AddRange(comments);
            await context.SaveChangesAsync();

            // Seed Followers (Following relationships)
            var followers = new List<Follower>
            {
                new Follower { Id = 1, UserId = 1, FollowerId = 2, CreatedAt = DateTime.UtcNow.AddDays(-20) }, // Jane follows John
                new Follower { Id = 2, UserId = 1, FollowerId = 3, CreatedAt = DateTime.UtcNow.AddDays(-18) }, // Mike follows John
                new Follower { Id = 3, UserId = 1, FollowerId = 4, CreatedAt = DateTime.UtcNow.AddDays(-15) }, // Sarah follows John
                new Follower { Id = 4, UserId = 2, FollowerId = 1, CreatedAt = DateTime.UtcNow.AddDays(-19) }, // John follows Jane
                new Follower { Id = 5, UserId = 2, FollowerId = 3, CreatedAt = DateTime.UtcNow.AddDays(-17) }, // Mike follows Jane
                new Follower { Id = 6, UserId = 3, FollowerId = 1, CreatedAt = DateTime.UtcNow.AddDays(-16) }, // John follows Mike
                new Follower { Id = 7, UserId = 3, FollowerId = 2, CreatedAt = DateTime.UtcNow.AddDays(-14) }, // Jane follows Mike
                new Follower { Id = 8, UserId = 4, FollowerId = 5, CreatedAt = DateTime.UtcNow.AddDays(-12) }, // Alex follows Sarah
                new Follower { Id = 9, UserId = 5, FollowerId = 1, CreatedAt = DateTime.UtcNow.AddDays(-10) }, // John follows Alex
                new Follower { Id = 10, UserId = 5, FollowerId = 3, CreatedAt = DateTime.UtcNow.AddDays(-8) }   // Mike follows Alex
            };

            context.Followers.AddRange(followers);
            await context.SaveChangesAsync();

            // Seed Badges
            var badges = new List<Badge>
            {
                new Badge
                {
                    Id = 1,
                    Name = "Early Adopter",
                    Description = "One of the first users to join Red-Shark",
                    IconUrl = "https://github.com/shadcn.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Badge
                {
                    Id = 2,
                    Name = "Content Creator",
                    Description = "Created 10+ posts",
                    IconUrl = "https://github.com/shadcn.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Badge
                {
                    Id = 3,
                    Name = "Social Butterfly",
                    Description = "Following 20+ users",
                    IconUrl = "https://github.com/shadcn.png",
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                }
            };

            context.Badges.AddRange(badges);
            await context.SaveChangesAsync();

            // Seed User Badges
            var userBadges = new List<UserBadge>
            {
                new UserBadge { Id = 1, UserId = 1, BadgeId = 1, CreatedAt = DateTime.UtcNow.AddDays(-30) },
                new UserBadge { Id = 2, UserId = 2, BadgeId = 1, CreatedAt = DateTime.UtcNow.AddDays(-25) },
                new UserBadge { Id = 3, UserId = 3, BadgeId = 1, CreatedAt = DateTime.UtcNow.AddDays(-20) },
                new UserBadge { Id = 4, UserId = 1, BadgeId = 2, CreatedAt = DateTime.UtcNow.AddDays(-2) }
            };

            context.UserBadges.AddRange(userBadges);
            await context.SaveChangesAsync();

            // Seed Notifications
            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = 1,
                    UserId = 1,
                    Content = "Welcome to Red-Shark! Start by following some users and sharing your first post.",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new Notification
                {
                    Id = 2,
                    UserId = 1,
                    Content = "Jane Smith started following you!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new Notification
                {
                    Id = 3,
                    UserId = 1,
                    Content = "Your post received a like from Mike Wilson!",
                    IsRead = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Notification
                {
                    Id = 4,
                    UserId = 2,
                    Content = "John Doe commented on your post!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Notification
                {
                    Id = 5,
                    UserId = 3,
                    Content = "You earned the 'Early Adopter' badge!",
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                }
            };

            context.Notifications.AddRange(notifications);
            await context.SaveChangesAsync();
        }
    }
}
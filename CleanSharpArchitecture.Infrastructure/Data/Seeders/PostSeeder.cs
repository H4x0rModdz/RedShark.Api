using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class PostSeeder
    {
        public static async Task<List<Post>> SeedAsync(ApplicationDbContext context, List<User> users)
        {
            var posts = new List<Post>
            {
                new Post
                {
                    UserId = users[0].Id, // John Doe
                    Content = "Welcome to Red-Shark! This is my first post on this amazing social media platform. Excited to connect with everyone and share my journey in software development! 🚀",
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UpdatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Post
                {
                    UserId = users[1].Id, // Jane Smith
                    Content = "Just finished designing the new user interface for our project. Clean Architecture really helps with maintainable code! The separation of concerns makes everything so much clearer. 💻",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Post
                {
                    UserId = users[2].Id, // Mike Wilson
                    Content = "Working on implementing JWT authentication with refresh tokens. Security is crucial for any social media platform! Always remember to validate tokens properly and implement proper rotation. 🔐",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Post
                {
                    UserId = users[0].Id, // John Doe
                    Content = "Today I learned about FluentValidation and how it makes input validation so much cleaner. Love discovering new tools that improve code quality and maintainability! 📚",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Post
                {
                    UserId = users[3].Id, // Sarah Johnson
                    Content = "Product roadmap is looking great! We're planning some exciting features for the next quarter. Real-time notifications, advanced analytics, and mobile app improvements. Stay tuned! ✨",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Post
                {
                    UserId = users[4].Id, // Alex Brown
                    Content = "Setting up Docker containers and Kubernetes for our deployment pipeline. DevOps automation saves so much time! CI/CD is now fully automated with zero-downtime deployments. ⚙️",
                    CreatedAt = DateTime.UtcNow.AddHours(-12),
                    UpdatedAt = DateTime.UtcNow.AddHours(-12)
                },
                new Post
                {
                    UserId = users[1].Id, // Jane Smith
                    Content = "User experience is everything! Spent the day improving the mobile responsiveness of our design system. Every pixel matters when creating inclusive interfaces. 📱",
                    CreatedAt = DateTime.UtcNow.AddHours(-6),
                    UpdatedAt = DateTime.UtcNow.AddHours(-6)
                },
                new Post
                {
                    UserId = users[2].Id, // Mike Wilson
                    Content = "Rate limiting implementation is complete! Now our API is protected against abuse and performs much better. Used Redis for distributed rate limiting across multiple instances. 🛡️",
                    CreatedAt = DateTime.UtcNow.AddHours(-3),
                    UpdatedAt = DateTime.UtcNow.AddHours(-3)
                },
                new Post
                {
                    UserId = users[5].Id, // Emma Davis
                    Content = "Machine learning models are fascinating! Just deployed a recommendation system that analyzes user behavior patterns. The accuracy improvements are incredible! 🤖",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    UpdatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new Post
                {
                    UserId = users[6].Id, // Lucas Martinez
                    Content = "React Native vs Flutter debate continues... Both are great for cross-platform development. Choice depends on team expertise and project requirements. What's your preference? 📱",
                    CreatedAt = DateTime.UtcNow.AddHours(-1),
                    UpdatedAt = DateTime.UtcNow.AddHours(-1)
                },
                new Post
                {
                    UserId = users[7].Id, // Sophia Garcia
                    Content = "Cybersecurity tip: Always implement proper input sanitization! XSS and SQL injection are still among the most common vulnerabilities. Security should be built-in, not bolted-on. 🔒",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-45),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-45)
                },
                new Post
                {
                    UserId = users[8].Id, // Ryan Thompson
                    Content = "Event sourcing pattern is a game-changer for complex domains! The ability to replay events and maintain complete audit trails is invaluable for business-critical applications. 📊",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-30)
                },
                new Post
                {
                    UserId = users[9].Id, // Olivia White
                    Content = "Web accessibility matters! Just implemented ARIA attributes and keyboard navigation for our new component library. Making the web inclusive for everyone should be our priority. ♿",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-15),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-15)
                },
                new Post
                {
                    UserId = users[4].Id, // Alex Brown
                    Content = "Monitoring and observability are crucial! Set up distributed tracing with OpenTelemetry. Now we can track requests across all microservices and identify bottlenecks quickly. 📈",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new Post
                {
                    UserId = users[3].Id, // Sarah Johnson
                    Content = "User research insights: 85% of users abandon forms that take more than 3 steps. Keep it simple and progressive! UX data should drive product decisions. 📋",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-5),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-5)
                }
            };

            context.Posts.AddRange(posts);
            await context.SaveChangesAsync();
            
            return posts;
        }
    }
}
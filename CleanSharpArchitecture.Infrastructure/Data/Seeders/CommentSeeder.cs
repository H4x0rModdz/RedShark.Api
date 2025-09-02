using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class CommentSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users, List<Post> posts)
        {
            var comments = new List<Comment>
            {
                new Comment
                {
                    UserId = users[1].Id, // Jane
                    PostId = posts[0].Id, // John's welcome post
                    Content = "Welcome to the community! Great to have you here! 👋",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Comment
                {
                    UserId = users[2].Id, // Mike
                    PostId = posts[0].Id, // John's welcome post
                    Content = "Looking forward to your future posts! The community is growing fast.",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new Comment
                {
                    UserId = users[0].Id, // John
                    PostId = posts[1].Id, // Jane's UI post
                    Content = "Your designs are always amazing! Can't wait to see the final result.",
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Comment
                {
                    UserId = users[3].Id, // Sarah
                    PostId = posts[2].Id, // Mike's JWT post
                    Content = "Security is indeed crucial. Great work on the JWT implementation! Have you considered implementing refresh token rotation?",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Comment
                {
                    UserId = users[2].Id, // Mike
                    PostId = posts[3].Id, // John's FluentValidation post
                    Content = "FluentValidation is fantastic! It makes validation so much cleaner and testable. The rule chaining feature is my favorite.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Comment
                {
                    UserId = users[5].Id, // Emma
                    PostId = posts[4].Id, // Sarah's roadmap post
                    Content = "Excited about the analytics features! Will there be machine learning insights included?",
                    CreatedAt = DateTime.UtcNow.AddHours(-20),
                    UpdatedAt = DateTime.UtcNow.AddHours(-20)
                },
                new Comment
                {
                    UserId = users[6].Id, // Lucas
                    PostId = posts[5].Id, // Alex's DevOps post
                    Content = "Kubernetes is powerful but complex. How do you handle configuration management across environments?",
                    CreatedAt = DateTime.UtcNow.AddHours(-10),
                    UpdatedAt = DateTime.UtcNow.AddHours(-10)
                },
                new Comment
                {
                    UserId = users[7].Id, // Sophia
                    PostId = posts[6].Id, // Jane's mobile post
                    Content = "Mobile-first design is the way to go! Are you using any specific framework for responsive components?",
                    CreatedAt = DateTime.UtcNow.AddHours(-5),
                    UpdatedAt = DateTime.UtcNow.AddHours(-5)
                },
                new Comment
                {
                    UserId = users[8].Id, // Ryan
                    PostId = posts[7].Id, // Mike's rate limiting post
                    Content = "Redis for rate limiting is smart! How do you handle the sliding window algorithm implementation?",
                    CreatedAt = DateTime.UtcNow.AddHours(-2),
                    UpdatedAt = DateTime.UtcNow.AddHours(-2)
                },
                new Comment
                {
                    UserId = users[4].Id, // Alex
                    PostId = posts[8].Id, // Emma's ML post
                    Content = "ML recommendations can be tricky to get right. What algorithms are you using for collaborative filtering?",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-90),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-90)
                },
                new Comment
                {
                    UserId = users[0].Id, // John
                    PostId = posts[9].Id, // Lucas's mobile frameworks post
                    Content = "I prefer React Native for JavaScript teams, but Flutter has better performance. Context matters!",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-50),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-50)
                },
                new Comment
                {
                    UserId = users[1].Id, // Jane
                    PostId = posts[10].Id, // Sophia's security post
                    Content = "OWASP Top 10 should be mandatory reading for all developers. Prevention is better than remediation!",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-35),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-35)
                },
                new Comment
                {
                    UserId = users[3].Id, // Sarah
                    PostId = posts[11].Id, // Ryan's event sourcing post
                    Content = "Event sourcing is powerful but adds complexity. Do you use snapshots to optimize read performance?",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-20),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-20)
                },
                new Comment
                {
                    UserId = users[5].Id, // Emma
                    PostId = posts[12].Id, // Olivia's accessibility post
                    Content = "Accessibility is often overlooked. Thanks for championing inclusive design! Screen reader testing is crucial.",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-10),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new Comment
                {
                    UserId = users[6].Id, // Lucas
                    PostId = posts[13].Id, // Alex's observability post
                    Content = "OpenTelemetry is a game-changer for observability! How do you handle trace sampling in high-traffic scenarios?",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-3),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-3)
                }
            };

            context.Comments.AddRange(comments);
            await context.SaveChangesAsync();

            // Now add replies (nested comments) using the real IDs of the already saved comments
            var replies = new List<Comment>
            {
                // Reply to Jane's welcome comment (comments[0])
                new Comment
                {
                    UserId = users[0].Id, // John
                    PostId = posts[0].Id,
                    ParentCommentId = comments[0].Id, // Reply to Jane's comment
                    Content = "Thanks Jane! Looking forward to collaborating with everyone here! 🤝",
                    CreatedAt = DateTime.UtcNow.AddDays(-4).AddHours(2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4).AddHours(2)
                },

                // Reply to Sarah's security comment (comments[3])
                new Comment
                {
                    UserId = users[2].Id, // Mike
                    PostId = posts[2].Id,
                    ParentCommentId = comments[3].Id, // Reply to Sarah's comment
                    Content = "@sarah_johnson Yes! I'm implementing rotation every 24 hours for extra security. Great suggestion! 🔄",
                    CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(3),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1).AddHours(3)
                },

                // Reply to Emma's analytics question (comments[5])
                new Comment
                {
                    UserId = users[3].Id, // Sarah
                    PostId = posts[4].Id,
                    ParentCommentId = comments[5].Id, // Reply to Emma's comment
                    Content = "@emma_davis Absolutely! We're planning predictive analytics and user behavior insights. Perfect timing! 📊",
                    CreatedAt = DateTime.UtcNow.AddHours(-18),
                    UpdatedAt = DateTime.UtcNow.AddHours(-18)
                },

                // Reply to Lucas's Kubernetes question (comments[6])
                new Comment
                {
                    UserId = users[4].Id, // Alex
                    PostId = posts[5].Id,
                    ParentCommentId = comments[6].Id, // Reply to Lucas's comment
                    Content = "Great question! We use Helm charts with separate values files for dev/staging/prod. ConfigMaps and Secrets are environment-specific. 🎯",
                    CreatedAt = DateTime.UtcNow.AddHours(-8),
                    UpdatedAt = DateTime.UtcNow.AddHours(-8)
                },

                // Reply to Ryan's Redis question (comments[8])
                new Comment
                {
                    UserId = users[2].Id, // Mike
                    PostId = posts[7].Id,
                    ParentCommentId = comments[8].Id, // Reply to Ryan's comment
                    Content = "We implement token bucket with Redis sorted sets. Each user has a bucket that refills over time. Works like a charm! ⚡",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-90),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-90)
                }
            };

            context.Comments.AddRange(replies);
            await context.SaveChangesAsync();
        }
    }
}
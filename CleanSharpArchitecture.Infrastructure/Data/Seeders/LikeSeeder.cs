using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class LikeSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users, List<Post> posts)
        {
            var likes = new List<Like>
            {
                // First post likes (John's welcome post)
                new Like { UserId = users[1].Id, PostId = posts[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new Like { UserId = users[2].Id, PostId = posts[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new Like { UserId = users[3].Id, PostId = posts[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new Like { UserId = users[4].Id, PostId = posts[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                
                // Second post likes (Jane's UI post)
                new Like { UserId = users[0].Id, PostId = posts[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new Like { UserId = users[2].Id, PostId = posts[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new Like { UserId = users[5].Id, PostId = posts[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                
                // Third post likes (Mike's JWT post)
                new Like { UserId = users[0].Id, PostId = posts[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new Like { UserId = users[1].Id, PostId = posts[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Like { UserId = users[3].Id, PostId = posts[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Like { UserId = users[7].Id, PostId = posts[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                
                // Fourth post likes (John's FluentValidation post)
                new Like { UserId = users[4].Id, PostId = posts[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new Like { UserId = users[1].Id, PostId = posts[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new Like { UserId = users[2].Id, PostId = posts[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                
                // Fifth post likes (Sarah's roadmap post)
                new Like { UserId = users[5].Id, PostId = posts[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new Like { UserId = users[0].Id, PostId = posts[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                
                // Sixth post likes (Alex's DevOps post)
                new Like { UserId = users[6].Id, PostId = posts[5].Id, CreatedAt = DateTime.UtcNow.AddHours(-11) },
                new Like { UserId = users[8].Id, PostId = posts[5].Id, CreatedAt = DateTime.UtcNow.AddHours(-10) },
                new Like { UserId = users[0].Id, PostId = posts[5].Id, CreatedAt = DateTime.UtcNow.AddHours(-9) },
                
                // More recent posts
                new Like { UserId = users[7].Id, PostId = posts[6].Id, CreatedAt = DateTime.UtcNow.AddHours(-5) },
                new Like { UserId = users[8].Id, PostId = posts[7].Id, CreatedAt = DateTime.UtcNow.AddHours(-2) },
                new Like { UserId = users[9].Id, PostId = posts[8].Id, CreatedAt = DateTime.UtcNow.AddHours(-1) },
                new Like { UserId = users[0].Id, PostId = posts[9].Id, CreatedAt = DateTime.UtcNow.AddMinutes(-50) },
                new Like { UserId = users[1].Id, PostId = posts[10].Id, CreatedAt = DateTime.UtcNow.AddMinutes(-40) },
                new Like { UserId = users[2].Id, PostId = posts[11].Id, CreatedAt = DateTime.UtcNow.AddMinutes(-25) },
                new Like { UserId = users[3].Id, PostId = posts[12].Id, CreatedAt = DateTime.UtcNow.AddMinutes(-12) },
                new Like { UserId = users[4].Id, PostId = posts[13].Id, CreatedAt = DateTime.UtcNow.AddMinutes(-8) },
                new Like { UserId = users[6].Id, PostId = posts[14].Id, CreatedAt = DateTime.UtcNow.AddMinutes(-3) }
            };

            context.Likes.AddRange(likes);
            await context.SaveChangesAsync();

            // Adicionar likes em comentários (precisa buscar comentários do banco)
            var comments = context.Comments.ToList();
            
            var commentLikes = new List<Like>
            {
                // Likes nos comentários principais (não respostas)
                new Like { UserId = users[0].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(0).Id, CreatedAt = DateTime.UtcNow.AddDays(-4).AddMinutes(10) }, // John likes Jane's welcome comment
                new Like { UserId = users[2].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(0).Id, CreatedAt = DateTime.UtcNow.AddDays(-4).AddMinutes(15) }, // Mike likes Jane's welcome comment
                new Like { UserId = users[3].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(0).Id, CreatedAt = DateTime.UtcNow.AddDays(-4).AddMinutes(20) }, // Sarah likes Jane's welcome comment
                
                new Like { UserId = users[1].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(2).Id, CreatedAt = DateTime.UtcNow.AddDays(-3).AddMinutes(30) }, // Jane likes John's design comment
                new Like { UserId = users[5].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(2).Id, CreatedAt = DateTime.UtcNow.AddDays(-3).AddMinutes(45) }, // Emma likes John's design comment
                
                new Like { UserId = users[2].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(3).Id, CreatedAt = DateTime.UtcNow.AddDays(-2).AddMinutes(10) }, // Mike likes Sarah's security comment
                new Like { UserId = users[4].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(3).Id, CreatedAt = DateTime.UtcNow.AddDays(-2).AddMinutes(15) }, // Alex likes Sarah's security comment
                
                new Like { UserId = users[0].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(4).Id, CreatedAt = DateTime.UtcNow.AddDays(-1).AddMinutes(5) }, // John likes Mike's FluentValidation comment
                new Like { UserId = users[1].Id, CommentId = comments.Where(c => c.ParentCommentId == null).ElementAt(4).Id, CreatedAt = DateTime.UtcNow.AddDays(-1).AddMinutes(10) }, // Jane likes Mike's FluentValidation comment
                
                // Likes em algumas respostas (comentários aninhados)
                new Like { UserId = users[1].Id, CommentId = comments.Where(c => c.ParentCommentId != null).ElementAt(0).Id, CreatedAt = DateTime.UtcNow.AddDays(-4).AddMinutes(30) }, // Jane likes John's reply
                new Like { UserId = users[3].Id, CommentId = comments.Where(c => c.ParentCommentId != null).ElementAt(1).Id, CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(1) }, // Sarah likes Mike's security reply
                new Like { UserId = users[6].Id, CommentId = comments.Where(c => c.ParentCommentId != null).ElementAt(3).Id, CreatedAt = DateTime.UtcNow.AddHours(-7) }, // Lucas likes Alex's Kubernetes reply
                new Like { UserId = users[8].Id, CommentId = comments.Where(c => c.ParentCommentId != null).ElementAt(4).Id, CreatedAt = DateTime.UtcNow.AddMinutes(-85) } // Ryan likes Mike's Redis reply
            };

            context.Likes.AddRange(commentLikes);
            await context.SaveChangesAsync();
        }
    }
}
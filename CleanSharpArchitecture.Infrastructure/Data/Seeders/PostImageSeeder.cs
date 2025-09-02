using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class PostImageSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<Post> posts)
        {
            if (context.PostImages.Any()) return;

            var postImages = new List<PostImage>
            {
                // Images for first few posts
                new PostImage
                {
                    PostId = posts[0].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1551434678-e076c223a692?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new PostImage
                {
                    PostId = posts[0].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1516321318423-f06f85e504b3?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },

                new PostImage
                {
                    PostId = posts[1].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1561070791-2526d30994b5?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-23),
                    UpdatedAt = DateTime.UtcNow.AddDays(-23)
                },

                new PostImage
                {
                    PostId = posts[3].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-21),
                    UpdatedAt = DateTime.UtcNow.AddDays(-21)
                },
                new PostImage
                {
                    PostId = posts[3].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1531297484001-80022131f5a1?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-21),
                    UpdatedAt = DateTime.UtcNow.AddDays(-21)
                },

                new PostImage
                {
                    PostId = posts[5].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1558494949-ef010cbdcc31?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-18),
                    UpdatedAt = DateTime.UtcNow.AddDays(-18)
                },

                new PostImage
                {
                    PostId = posts[7].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new PostImage
                {
                    PostId = posts[7].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1507146426996-ef05306b995a?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },

                new PostImage
                {
                    PostId = posts[9].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1512941937669-90a1b58e7e9c?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-12),
                    UpdatedAt = DateTime.UtcNow.AddDays(-12)
                },

                new PostImage
                {
                    PostId = posts[12].Id,
                    ImageUrl = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=800",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                }
            };

            context.PostImages.AddRange(postImages);
            await context.SaveChangesAsync();
        }
    }
}
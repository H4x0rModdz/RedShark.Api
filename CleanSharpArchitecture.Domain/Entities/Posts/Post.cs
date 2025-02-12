using System.Text.Json.Serialization;

namespace CleanSharpArchitecture.Domain.Entities.Posts
{
    public class Post : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        // Relationships
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<PostImage> Images { get; set; } = new List<PostImage>();
    }
}

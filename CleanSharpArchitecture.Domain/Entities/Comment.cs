using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }

        public long? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
    }
}

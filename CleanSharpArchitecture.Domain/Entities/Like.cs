using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class Like : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        
        // Like pode ser em um Post OU em um Comment (nullable para ambos)
        public long? PostId { get; set; }
        public Post Post { get; set; }
        
        public long? CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}

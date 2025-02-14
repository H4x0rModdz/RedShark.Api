using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class Like : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
    }
}

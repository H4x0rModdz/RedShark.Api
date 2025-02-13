namespace CleanSharpArchitecture.Domain.Entities
{
    public class Follower : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid FollowerId { get; set; }
        public User FollowerUser { get; set; }
    }
}

namespace CleanSharpArchitecture.Domain.Entities
{
    public class UserBadge : BaseEntity
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long BadgeId { get; set; }
        public Badge Badge { get; set; }
    }
}

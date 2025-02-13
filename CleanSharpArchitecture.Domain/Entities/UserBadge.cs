namespace CleanSharpArchitecture.Domain.Entities
{
    public class UserBadge : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid BadgeId { get; set; }
        public Badge Badge { get; set; }
    }
}

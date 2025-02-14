namespace CleanSharpArchitecture.Application.DTOs.Badges.UserBadges
{
    public class UserBadgeDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BadgeId { get; set; }
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public string BadgeIconUrl { get; set; }
    }
}

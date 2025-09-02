namespace CleanSharpArchitecture.Application.DTOs.Followers
{
    public class FollowingDto
    {
        public long RelationshipId { get; set; }

        public long ProfileUserId { get; set; }

        public long FollowingId { get; set; }

        public string FollowingDisplayName { get; set; } = string.Empty;

        public string FollowingUsername { get; set; } = string.Empty;

        public string? FollowingProfileImage { get; set; }

        public bool DoIFollowThisPerson { get; set; }

        public bool DoesThisPersonFollowMe { get; set; }
    }
}

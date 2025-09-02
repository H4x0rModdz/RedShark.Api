namespace CleanSharpArchitecture.Application.DTOs.Followers
{
    public class FollowerDto
    {
        public long RelationshipId { get; set; }

        public long ProfileUserId { get; set; }

        public long FollowerId { get; set; }

        public string FollowerDisplayName { get; set; } = string.Empty;

        public string FollowerUsername { get; set; } = string.Empty;

        public string? FollowerProfileImage { get; set; }

        public bool DoIFollowThisPerson { get; set; }

        public bool DoesThisPersonFollowMe { get; set; }
    }
}

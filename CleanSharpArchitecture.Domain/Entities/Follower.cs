using System.ComponentModel.DataAnnotations.Schema;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class Follower : BaseEntity
    {
        public long UserId { get; set; }

        [InverseProperty(nameof(User.Followers))]
        public User User { get; set; }

        public long FollowerId { get; set; }

        [InverseProperty(nameof(User.Following))]
        public User FollowerUser { get; set; }
    }
}

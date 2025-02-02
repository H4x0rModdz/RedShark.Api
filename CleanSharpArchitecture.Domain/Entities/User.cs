using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Biography { get; set; }
        public string? RecoveryCode { get; set; }
        public DateTime? RecoveryCodeExpiration { get; set; }

        // Relationships
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();
        public ICollection<Follower> Followers { get; set; } = new List<Follower>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<UserChat> Chats { get; set; } = new List<UserChat>();

        public User()
        {
            if (string.IsNullOrWhiteSpace(ProfileImageUrl))
                ProfileImageUrl = "https://github.com/shadcn.png";
        }
    }
}

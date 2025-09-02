using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Entities.Posts;
using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class User : BaseEntity
    {
        public Username UserName { get; set; }
        public string Name { get; set; }
        public Email Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string Biography { get; set; }
        public string? Location { get; set; }
        public string? Website { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public string? Profession { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? ProfileMusic { get; set; }
        public string? RecoveryCode { get; set; }
        public DateTime? RecoveryCodeExpiration { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LastFailedLoginAttempt { get; private set; }
        public bool IsVerified { get; set; } = false;

        // Relationships
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();

        [InverseProperty(nameof(Follower.User))]
        public ICollection<Follower> Followers { get; set; } = new List<Follower>();

        [InverseProperty(nameof(Follower.FollowerUser))]
        public ICollection<Follower> Following { get; set; } = new List<Follower>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<UserChat> Chats { get; set; } = new List<UserChat>();
        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
        public ICollection<UserPhoto> Photos { get; set; } = new List<UserPhoto>();

        public bool IsAccountLocked() => IsLocked && LockedUntil > DateTime.UtcNow;

        public void RegisterFailedAttempt()
        {
            FailedLoginAttempts++;
            LastFailedLoginAttempt = DateTime.UtcNow;

            if (FailedLoginAttempts >= 5 && LastFailedLoginAttempt.Value.AddMinutes(-30) <= DateTime.UtcNow)
                LockAccount();
        }

        private void LockAccount()
        {
            IsLocked = true;
            LockedUntil = DateTime.UtcNow.AddMinutes(30);
        }

        public void ResetFailedAttempts()
        {
            FailedLoginAttempts = 0;
            LastFailedLoginAttempt = null;
        }

        public void UnlockAccount()
        {
            IsLocked = false;
            LockedUntil = null;
            ResetFailedAttempts();
        }

        public User()
        {
            if (string.IsNullOrWhiteSpace(ProfileImageUrl))
                ProfileImageUrl = "https://github.com/shadcn.png";

            if (string.IsNullOrWhiteSpace(Biography))
                Biography = "This user has not provided a biography yet.";

            if (string.IsNullOrWhiteSpace(CoverImageUrl))
                CoverImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200&h=400&fit=crop";
        }
    }
}

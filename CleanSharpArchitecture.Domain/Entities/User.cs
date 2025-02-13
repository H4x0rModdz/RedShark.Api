﻿using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Entities.Posts;

namespace CleanSharpArchitecture.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Biography { get; set; }
        public string? RecoveryCode { get; set; }
        public DateTime? RecoveryCodeExpiration { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }
        public int FailedLoginAttempts { get; private set; }
        public DateTime? LastFailedLoginAttempt { get; private set; }

        // Relationships
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();
        public ICollection<Follower> Followers { get; set; } = new List<Follower>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<UserChat> Chats { get; set; } = new List<UserChat>();

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
        }
    }
}

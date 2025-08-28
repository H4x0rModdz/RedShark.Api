using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Entities.Chats;
using CleanSharpArchitecture.Domain.Entities.Posts;
using Microsoft.EntityFrameworkCore;

namespace CleanSharpArchitecture.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserBadge> UserBadges { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Interest> Interests { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.ProfileImageUrl)
                .HasMaxLength(500);

            modelBuilder.Entity<User>()
                .Property(u => u.Biography)
                .HasMaxLength(1000);

            modelBuilder.Entity<User>()
                .Property(u => u.RecoveryCode)
                .HasMaxLength(6);

            // Post
            modelBuilder.Entity<Post>()
                .Property(p => p.Content)
                .IsRequired()
                .HasMaxLength(2000);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Images)
                .WithOne(pi => pi.Post)
                .HasForeignKey(pi => pi.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // PostImage
            modelBuilder.Entity<PostImage>()
                .Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<PostImage>()
                .HasKey(pi => pi.Id);

            // Like
            modelBuilder.Entity<Like>()
                .HasKey(l => l.Id);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Evitar exclusão em cascata do User

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Comment)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Constraint: Like deve ser EM um Post OU em um Comment, mas não ambos
            modelBuilder.Entity<Like>()
                .HasCheckConstraint("CK_Like_PostOrComment", "(PostId IS NOT NULL AND CommentId IS NULL) OR (PostId IS NULL AND CommentId IS NOT NULL)");

            // Comment
            modelBuilder.Entity<Comment>()
                .Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Certifique-se de que está como Restrict

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Self-referencing relationship for Comment replies
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Follower
            modelBuilder.Entity<Follower>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<Follower>()
                .HasOne(f => f.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follower>()
                 .HasOne(f => f.FollowerUser)
                 .WithMany(u => u.Following)
                 .HasForeignKey(f => f.FollowerId)
                 .OnDelete(DeleteBehavior.Restrict);

            // Badge
            modelBuilder.Entity<Badge>()
                .Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Badge>()
                .Property(b => b.Description)
                .HasMaxLength(500);

            modelBuilder.Entity<Badge>()
                .Property(b => b.IconUrl)
                .HasMaxLength(500);

            // UserBadge
            modelBuilder.Entity<UserBadge>()
                .HasKey(ub => ub.Id);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.User)
                .WithMany(u => u.Badges)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBadge>()
                .HasOne(ub => ub.Badge)
                .WithMany()
                .HasForeignKey(ub => ub.BadgeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification
            modelBuilder.Entity<Notification>()
                .Property(n => n.Content)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Notification>()
                .Property(n => n.IsRead)
                .IsRequired();

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Chat
            modelBuilder.Entity<Chat>()
                .Property(c => c.Name)
                .HasMaxLength(100);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(cm => cm.Chat)
                .HasForeignKey(cm => cm.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserChat
            modelBuilder.Entity<UserChat>()
                .HasKey(uc => uc.Id);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.Chats)
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.Participants)
                .HasForeignKey(uc => uc.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            // ChatMessage
            modelBuilder.Entity<ChatMessage>()
                .Property(cm => cm.Content)
                .IsRequired()
                .HasMaxLength(2000);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(cm => cm.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChatMessage>()
                .HasOne(cm => cm.User)
                .WithMany()
                .HasForeignKey(cm => cm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Interest
            modelBuilder.Entity<Interest>()
                .Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(100);

            // UserInterest
            modelBuilder.Entity<UserInterest>()
                .HasKey(ui => ui.Id);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.Interest)
                .WithMany()
                .HasForeignKey(ui => ui.InterestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

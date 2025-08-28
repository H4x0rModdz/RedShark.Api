using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class FollowerSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users)
        {
            var followers = new List<Follower>
            {
                // John Doe followers
                new Follower { UserId = users[0].Id, FollowerId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-20) }, // Jane follows John
                new Follower { UserId = users[0].Id, FollowerId = users[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-18) }, // Mike follows John
                new Follower { UserId = users[0].Id, FollowerId = users[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-15) }, // Sarah follows John
                new Follower { UserId = users[0].Id, FollowerId = users[5].Id, CreatedAt = DateTime.UtcNow.AddDays(-10) }, // Emma follows John
                
                // Jane Smith followers and following
                new Follower { UserId = users[1].Id, FollowerId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-19) }, // John follows Jane
                new Follower { UserId = users[1].Id, FollowerId = users[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-17) }, // Mike follows Jane
                new Follower { UserId = users[1].Id, FollowerId = users[6].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },  // Lucas follows Jane
                new Follower { UserId = users[1].Id, FollowerId = users[9].Id, CreatedAt = DateTime.UtcNow.AddDays(-2) },  // Olivia follows Jane
                
                // Mike Wilson followers and following
                new Follower { UserId = users[2].Id, FollowerId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-16) }, // John follows Mike
                new Follower { UserId = users[2].Id, FollowerId = users[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-14) }, // Jane follows Mike
                new Follower { UserId = users[2].Id, FollowerId = users[7].Id, CreatedAt = DateTime.UtcNow.AddDays(-6) },  // Sophia follows Mike
                new Follower { UserId = users[2].Id, FollowerId = users[9].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },  // Olivia follows Mike
                
                // Sarah Johnson followers and following
                new Follower { UserId = users[3].Id, FollowerId = users[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-12) }, // Alex follows Sarah
                new Follower { UserId = users[3].Id, FollowerId = users[7].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) },  // Sophia follows Sarah
                
                // Alex Brown followers and following
                new Follower { UserId = users[4].Id, FollowerId = users[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-10) }, // John follows Alex
                new Follower { UserId = users[4].Id, FollowerId = users[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-8) },  // Mike follows Alex
                new Follower { UserId = users[4].Id, FollowerId = users[8].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },  // Ryan follows Alex
                
                // Emma Davis followers and following
                new Follower { UserId = users[5].Id, FollowerId = users[6].Id, CreatedAt = DateTime.UtcNow.AddDays(-7) }, // Lucas follows Emma
                new Follower { UserId = users[5].Id, FollowerId = users[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-6) }, // Sarah follows Emma
                
                // Cross connections
                new Follower { UserId = users[6].Id, FollowerId = users[7].Id, CreatedAt = DateTime.UtcNow.AddDays(-6) }, // Sophia follows Lucas
                new Follower { UserId = users[7].Id, FollowerId = users[8].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) }, // Ryan follows Sophia
                new Follower { UserId = users[8].Id, FollowerId = users[9].Id, CreatedAt = DateTime.UtcNow.AddDays(-4) }, // Olivia follows Ryan
                new Follower { UserId = users[9].Id, FollowerId = users[5].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) }  // Emma follows Olivia
            };

            context.Followers.AddRange(followers);
            await context.SaveChangesAsync();
        }
    }
}
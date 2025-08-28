using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class UserInterestSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, List<User> users, List<Interest> interests)
        {
            var userInterests = new List<UserInterest>
            {
                // John Doe - Software Development & DevOps
                new UserInterest { UserId = users[0].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-29) },
                new UserInterest { UserId = users[0].Id, InterestId = interests[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-29) },
                
                // Jane Smith - Web Design & Software Development
                new UserInterest { UserId = users[1].Id, InterestId = interests[1].Id, CreatedAt = DateTime.UtcNow.AddDays(-24) },
                new UserInterest { UserId = users[1].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-24) },
                
                // Mike Wilson - Software Development & Cybersecurity
                new UserInterest { UserId = users[2].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-19) },
                new UserInterest { UserId = users[2].Id, InterestId = interests[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-19) },
                
                // Sarah Johnson - Software Development & Machine Learning
                new UserInterest { UserId = users[3].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-14) },
                new UserInterest { UserId = users[3].Id, InterestId = interests[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-14) },
                
                // Alex Brown - DevOps & Cloud Computing
                new UserInterest { UserId = users[4].Id, InterestId = interests[2].Id, CreatedAt = DateTime.UtcNow.AddDays(-9) },
                new UserInterest { UserId = users[4].Id, InterestId = interests[6].Id, CreatedAt = DateTime.UtcNow.AddDays(-9) },
                
                // Emma Davis - Machine Learning & Data Science
                new UserInterest { UserId = users[5].Id, InterestId = interests[3].Id, CreatedAt = DateTime.UtcNow.AddDays(-7) },
                new UserInterest { UserId = users[5].Id, InterestId = interests[7].Id, CreatedAt = DateTime.UtcNow.AddDays(-7) },
                
                // Lucas Martinez - Mobile Development & Software Development
                new UserInterest { UserId = users[6].Id, InterestId = interests[5].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new UserInterest { UserId = users[6].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-5) },
                
                // Sophia Garcia - Cybersecurity & Software Development
                new UserInterest { UserId = users[7].Id, InterestId = interests[4].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new UserInterest { UserId = users[7].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-3) },
                
                // Ryan Thompson - Software Development & Cloud Computing
                new UserInterest { UserId = users[8].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new UserInterest { UserId = users[8].Id, InterestId = interests[6].Id, CreatedAt = DateTime.UtcNow.AddDays(-1) },
                
                // Olivia White - Web Design & Software Development
                new UserInterest { UserId = users[9].Id, InterestId = interests[1].Id, CreatedAt = DateTime.UtcNow.AddHours(-12) },
                new UserInterest { UserId = users[9].Id, InterestId = interests[0].Id, CreatedAt = DateTime.UtcNow.AddHours(-12) }
            };

            context.UserInterests.AddRange(userInterests);
            await context.SaveChangesAsync();
        }
    }
}
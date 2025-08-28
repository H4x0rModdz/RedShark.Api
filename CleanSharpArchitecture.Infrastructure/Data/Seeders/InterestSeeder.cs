using CleanSharpArchitecture.Domain.Entities;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class InterestSeeder
    {
        public static async Task<List<Interest>> SeedAsync(ApplicationDbContext context)
        {
            var interests = new List<Interest>
            {
                new Interest 
                { 
                    Name = "Software Development", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "Web Design", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "DevOps", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "Machine Learning", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "Cybersecurity", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "Mobile Development", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "Cloud Computing", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                },
                new Interest 
                { 
                    Name = "Data Science", 
                    CreatedAt = DateTime.UtcNow.AddDays(-30) 
                }
            };

            context.Interests.AddRange(interests);
            await context.SaveChangesAsync();
            
            return interests;
        }
    }
}
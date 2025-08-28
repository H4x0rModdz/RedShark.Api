using CleanSharpArchitecture.Domain.Entities;
using BCrypt.Net;

namespace CleanSharpArchitecture.Infrastructure.Data.Seeders
{
    public static class UserSeeder
    {
        public static async Task<List<User>> SeedAsync(ApplicationDbContext context)
        {
            var password = BCrypt.Net.BCrypt.HashPassword("Password123!");

            var users = new List<User>
            {
                new User
                {
                    UserName = "john_doe",
                    Name = "John Doe",
                    Email = "john.doe@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=400",
                    Biography = "Software developer passionate about clean architecture and social media. Currently working on scalable microservices and distributed systems.",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new User
                {
                    UserName = "jane_smith",
                    Name = "Jane Smith",
                    Email = "jane.smith@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1494790108755-2616b612b8dd?w=400",
                    Biography = "UI/UX Designer who loves creating beautiful user experiences. Specialized in design systems and user research methodologies.",
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new User
                {
                    UserName = "mike_wilson",
                    Name = "Mike Wilson",
                    Email = "mike.wilson@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400",
                    Biography = "Full-stack developer and tech enthusiast. Love working with React, Node.js, and exploring new technologies in the JavaScript ecosystem.",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new User
                {
                    UserName = "sarah_johnson",
                    Name = "Sarah Johnson",
                    Email = "sarah.johnson@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400",
                    Biography = "Product Manager with a passion for innovation. Leading digital transformation initiatives and agile product development teams.",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new User
                {
                    UserName = "alex_brown",
                    Name = "Alex Brown",
                    Email = "alex.brown@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400",
                    Biography = "DevOps engineer focused on scalable infrastructure. Kubernetes, Docker, and cloud-native technologies are my playground.",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new User
                {
                    UserName = "emma_davis",
                    Name = "Emma Davis",
                    Email = "emma.davis@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1544725176-7c40e5a71c5e?w=400",
                    Biography = "Data Scientist passionate about machine learning and AI. Working on predictive analytics and natural language processing projects.",
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new User
                {
                    UserName = "lucas_martinez",
                    Name = "Lucas Martinez",
                    Email = "lucas.martinez@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1519345182560-3f2917c472ef?w=400",
                    Biography = "Mobile app developer specializing in React Native and Flutter. Building cross-platform solutions for startups and enterprises.",
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new User
                {
                    UserName = "sophia_garcia",
                    Name = "Sophia Garcia",
                    Email = "sophia.garcia@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1487412720507-e7ab37603c6f?w=400",
                    Biography = "Cybersecurity specialist and ethical hacker. Focused on application security, penetration testing, and security awareness training.",
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new User
                {
                    UserName = "ryan_thompson",
                    Name = "Ryan Thompson",
                    Email = "ryan.thompson@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1463453091185-61582044d556?w=400",
                    Biography = "Backend engineer with expertise in microservices architecture. Passionate about distributed systems, event sourcing, and CQRS patterns.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new User
                {
                    UserName = "olivia_white",
                    Name = "Olivia White",
                    Email = "olivia.white@redshark.com",
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1498551172505-8ee7ad69f235?w=400",
                    Biography = "Frontend architect and accessibility advocate. Specializing in modern web technologies and creating inclusive user interfaces.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UpdatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.Users.AddRange(users);
            await context.SaveChangesAsync();
            
            return users;
        }
    }
}
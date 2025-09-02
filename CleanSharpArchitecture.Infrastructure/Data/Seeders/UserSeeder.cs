using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.ValueObjects;
using CleanSharpArchitecture.Domain.Enums;
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
                    UserName = Username.Create("john_doe"),
                    Name = "John Doe",
                    Email = Email.Create("john.doe@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1506905925346-21bda4d32df4?w=1200&h=400&fit=crop",
                    Biography = "Software developer passionate about clean architecture and social media. Currently working on scalable microservices and distributed systems.",
                    Location = "San Francisco, CA",
                    Website = "https://johndoe.dev",
                    MaritalStatus = MaritalStatus.Single,
                    Profession = "Senior Software Engineer",
                    BirthDate = new DateTime(1990, 5, 15),
                    ProfileMusic = "11dFghVXANMlKmJXsNCbNl", // Bohemian Rhapsody - Queen
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow.AddDays(-30)
                },
                new User
                {
                    UserName = Username.Create("jane_smith"),
                    Name = "Jane Smith",
                    Email = Email.Create("jane.smith@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1494790108755-2616b612b8dd?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=1200&h=400&fit=crop",
                    Biography = "UI/UX Designer who loves creating beautiful user experiences. Specialized in design systems and user research methodologies.",
                    Location = "New York, NY",
                    Website = "https://janesmith.design",
                    MaritalStatus = MaritalStatus.Married,
                    Profession = "Senior UX Designer",
                    BirthDate = new DateTime(1988, 8, 22),
                    ProfileMusic = "1301WleyT98MSxVHPZCA6M", // Someone Like You - Adele
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    UpdatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new User
                {
                    UserName = Username.Create("mike_wilson"),
                    Name = "Mike Wilson",
                    Email = Email.Create("mike.wilson@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?w=1200&h=400&fit=crop",
                    Biography = "Full-stack developer and tech enthusiast. Love working with React, Node.js, and exploring new technologies in the JavaScript ecosystem.",
                    Location = "Austin, TX",
                    Website = "https://mikewilson.tech",
                    MaritalStatus = MaritalStatus.InARelationship,
                    Profession = "Full Stack Developer",
                    BirthDate = new DateTime(1992, 11, 8),
                    ProfileMusic = "4uLU6hMCjMI75M1A2tKUQC", // Blinding Lights - The Weeknd
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    UpdatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new User
                {
                    UserName = Username.Create("sarah_johnson"),
                    Name = "Sarah Johnson",
                    Email = Email.Create("sarah.johnson@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1557804506-669a67965ba0?w=1200&h=400&fit=crop",
                    Biography = "Product Manager with a passion for innovation. Leading digital transformation initiatives and agile product development teams.",
                    Location = "Seattle, WA",
                    Website = "https://sarahjohnson.pm",
                    MaritalStatus = MaritalStatus.Divorced,
                    Profession = "Senior Product Manager",
                    BirthDate = new DateTime(1985, 3, 14),
                    ProfileMusic = "0tgVpDi06FyKpA1z0VMD4v", // Perfect - Ed Sheeran
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    UpdatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new User
                {
                    UserName = Username.Create("alex_brown"),
                    Name = "Alex Brown",
                    Email = Email.Create("alex.brown@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1518709268805-4e9042af2176?w=1200&h=400&fit=crop",
                    Biography = "DevOps engineer focused on scalable infrastructure. Kubernetes, Docker, and cloud-native technologies are my playground.",
                    Location = "Denver, CO",
                    Website = "https://alexbrown.cloud",
                    MaritalStatus = MaritalStatus.Single,
                    Profession = "DevOps Engineer",
                    BirthDate = new DateTime(1991, 7, 30),
                    ProfileMusic = "5ghIJDpPoe3CfHMGu71E6T", // Thunderstruck - AC/DC
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    UpdatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new User
                {
                    UserName = Username.Create("emma_davis"),
                    Name = "Emma Davis",
                    Email = Email.Create("emma.davis@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1544725176-7c40e5a71c5e?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1507146426996-ef05306b995a?w=1200&h=400&fit=crop",
                    Biography = "Data Scientist passionate about machine learning and AI. Working on predictive analytics and natural language processing projects.",
                    Location = "Boston, MA",
                    Website = "https://emmadavis.data",
                    MaritalStatus = MaritalStatus.Married,
                    Profession = "Senior Data Scientist",
                    BirthDate = new DateTime(1989, 12, 5),
                    ProfileMusic = "37BZB0z9T8Xu7U8scQX835", // Shape of You - Ed Sheeran
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-8),
                    UpdatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new User
                {
                    UserName = Username.Create("lucas_martinez"),
                    Name = "Lucas Martinez",
                    Email = Email.Create("lucas.martinez@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1519345182560-3f2917c472ef?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1493612276216-ee3925520721?w=1200&h=400&fit=crop",
                    Biography = "Mobile app developer specializing in React Native and Flutter. Building cross-platform solutions for startups and enterprises.",
                    Location = "Miami, FL",
                    Website = "https://lucasmartinez.mobile",
                    MaritalStatus = MaritalStatus.ItsComplicated,
                    Profession = "Mobile Developer",
                    BirthDate = new DateTime(1993, 9, 18),
                    ProfileMusic = "6DCZcSspjsKoFjzjrWoCdn", // God's Plan - Drake
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-6),
                    UpdatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new User
                {
                    UserName = Username.Create("sophia_garcia"),
                    Name = "Sophia Garcia",
                    Email = Email.Create("sophia.garcia@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1487412720507-e7ab37603c6f?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?w=1200&h=400&fit=crop",
                    Biography = "Cybersecurity specialist and ethical hacker. Focused on application security, penetration testing, and security awareness training.",
                    Location = "Phoenix, AZ",
                    Website = "https://sophiagarcia.security",
                    MaritalStatus = MaritalStatus.Single,
                    Profession = "Cybersecurity Specialist",
                    BirthDate = new DateTime(1987, 4, 25),
                    ProfileMusic = "0VjIjW4GlUZAMYd2vXMi3b", // Bad Guy - Billie Eilish
                    IsVerified = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UpdatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new User
                {
                    UserName = Username.Create("ryan_thompson"),
                    Name = "Ryan Thompson",
                    Email = Email.Create("ryan.thompson@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1463453091185-61582044d556?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1526374965328-7f61d4dc18c5?w=1200&h=400&fit=crop",
                    Biography = "Backend engineer with expertise in microservices architecture. Passionate about distributed systems, event sourcing, and CQRS patterns.",
                    Location = "Portland, OR",
                    Website = "https://ryanthompson.backend",
                    MaritalStatus = MaritalStatus.Married,
                    Profession = "Backend Architect",
                    BirthDate = new DateTime(1986, 10, 12),
                    ProfileMusic = "2takcwOaAZWiXQijPHIx7B", // Time - Pink Floyd
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new User
                {
                    UserName = Username.Create("olivia_white"),
                    Name = "Olivia White",
                    Email = Email.Create("olivia.white@redshark.com"),
                    Password = password,
                    ProfileImageUrl = "https://images.unsplash.com/photo-1498551172505-8ee7ad69f235?w=400",
                    CoverImageUrl = "https://images.unsplash.com/photo-1586953208448-b95a79798f07?w=1200&h=400&fit=crop",
                    Biography = "Frontend architect and accessibility advocate. Specializing in modern web technologies and creating inclusive user interfaces.",
                    Location = "Nashville, TN",
                    Website = "https://oliviawhite.frontend",
                    MaritalStatus = MaritalStatus.Widowed,
                    Profession = "Frontend Architect",
                    BirthDate = new DateTime(1984, 6, 7),
                    ProfileMusic = "4Dvkj6JhhA12EX05fT7y2e", // Rolling in the Deep - Adele
                    IsVerified = true,
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
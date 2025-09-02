using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Snowflakes;
using CleanSharpArchitecture.Domain.ValueObjects;
using FluentAssertions;
using System.Collections.Concurrent;
using Xunit;

namespace CleanSharpArchitecture.Testing.UnitTests
{
    public class SnowflakeIdGeneratorTests
    {
        [Fact]
        public void SnowflakeIdGenerator_ShouldGenerateUniqueIds_WhenCalledSequentially()
        {
            // Arrange
            var generator = new SnowflakeIdGenerator(1, 1);
            var ids = new List<long>();

            // Act
            for (int i = 0; i < 1000; i++)
            {
                ids.Add(generator.NextId());
            }

            // Assert
            var uniqueIds = ids.Distinct().Count();
            uniqueIds.Should().Be(1000, "all IDs should be unique");
        }

        [Fact]
        public void SnowflakeIdGenerator_ShouldGenerateUniqueIds_WhenCalledInParallel()
        {
            // Arrange
            var generator = new SnowflakeIdGenerator(1, 1);
            var ids = new ConcurrentBag<long>();

            // Act
            Parallel.For(0, 1000, i =>
            {
                ids.Add(generator.NextId());
            });

            // Assert
            var idList = ids.ToList();
            var uniqueIds = idList.Distinct().Count();
            uniqueIds.Should().Be(1000, "all IDs should be unique even in parallel execution");
        }

        [Fact]
        public void BaseEntity_ShouldGenerateUniqueIds_WhenCreatingMultipleUsers()
        {
            // Arrange & Act
            var users = new List<User>();
            for (int i = 0; i < 100; i++)
            {
                users.Add(new User
                {
                    UserName = Username.Create($"user{i}"),
                    Name = $"User {i}",
                    Email = Email.Create($"user{i}@test.com"),
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
            }

            // Assert
            var ids = users.Select(u => u.Id).ToList();
            var uniqueIds = ids.Distinct().Count();
            uniqueIds.Should().Be(100, "each user should have a unique ID");

            // Check if there are no duplicates
            var duplicates = ids.GroupBy(id => id).Where(g => g.Count() > 1).ToList();
            duplicates.Should().BeEmpty("there should be no duplicate IDs");
        }

        [Fact]
        public void BaseEntity_ShouldGenerateUniqueIds_WhenCreatingInParallel()
        {
            // Arrange & Act
            var users = new ConcurrentBag<User>();
            
            Parallel.For(0, 100, i =>
            {
                users.Add(new User
                {
                    UserName = Username.Create($"user{i}"),
                    Name = $"User {i}",
                    Email = Email.Create($"user{i}@test.com"),
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
            });

            // Assert
            var userList = users.ToList();
            var ids = userList.Select(u => u.Id).ToList();
            var uniqueIds = ids.Distinct().Count();
            uniqueIds.Should().Be(100, "each user should have a unique ID even when created in parallel");

            // Check if there are no duplicates
            var duplicates = ids.GroupBy(id => id).Where(g => g.Count() > 1).ToList();
            duplicates.Should().BeEmpty("there should be no duplicate IDs in parallel creation");
        }

        [Fact]
        public void SnowflakeId_ShouldBePositive_AndIncreasing()
        {
            // Arrange
            var users = new List<User>();

            // Act
            for (int i = 0; i < 10; i++)
            {
                users.Add(new User
                {
                    UserName = Username.Create($"user{i}"),
                    Name = $"User {i}",
                    Email = Email.Create($"user{i}@test.com"),
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
                
                // Small delay to ensure different timestamps
                Thread.Sleep(1);
            }

            // Assert
            foreach (var user in users)
            {
                user.Id.Should().BeGreaterThan(0, "IDs should be positive");
            }

            // IDs should be in ascending order (since they were created sequentially)
            var ids = users.Select(u => u.Id).ToList();
            for (int i = 1; i < ids.Count; i++)
            {
                ids[i].Should().BeGreaterThan(ids[i - 1], "IDs created later should be greater");
            }
        }

        [Fact]
        public void SnowflakeIdGenerator_ShouldGenerateUniqueIds_InMassiveCreation()
        {
            // Arrange
            var users = new List<User>();

            // Act - Simulate massive creation as in frontend (5000 entities very quickly)
            for (int i = 0; i < 5000; i++)
            {
                users.Add(new User
                {
                    UserName = Username.Create($"user{i}"),
                    Name = $"User {i}",
                    Email = Email.Create($"user{i}@test.com"),
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
            }

            // Assert
            var ids = users.Select(u => u.Id).ToList();
            var uniqueIds = ids.Distinct().Count();
            
            uniqueIds.Should().Be(5000, "all 5000 IDs should be unique");

            // Check specific duplicates
            var duplicates = ids.GroupBy(id => id)
                                .Where(g => g.Count() > 1)
                                .Select(g => new { Id = g.Key, Count = g.Count() })
                                .ToList();

            if (duplicates.Any())
            {
                var duplicateInfo = string.Join(", ", duplicates.Take(5).Select(d => $"ID {d.Id}: {d.Count}x"));
                Assert.True(false, $"Found {duplicates.Count} duplicates: {duplicateInfo}");
            }
        }

        [Fact]
        public void SnowflakeIdGenerator_ShouldHandleSequenceOverflow()
        {
            // Arrange
            var generator = new SnowflakeIdGenerator(1, 1);
            var ids = new HashSet<long>();

            // Act - Force sequence overflow (4096 IDs in the same millisecond)
            var startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            for (int i = 0; i < 5000; i++)
            {
                var id = generator.NextId();
                
                // Assert - Check uniqueness
                if (!ids.Add(id))
                {
                    Assert.True(false, $"Duplicate ID found: {id} at iteration {i}");
                }
            }

            // Assert
            ids.Count.Should().Be(5000, "all IDs should be unique even with overflow");
        }
    }
}
using CleanSharpArchitecture.Domain.Entities;
using CleanSharpArchitecture.Domain.Snowflakes;
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
            uniqueIds.Should().Be(1000, "todos os IDs devem ser únicos");
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
            uniqueIds.Should().Be(1000, "todos os IDs devem ser únicos mesmo em execução paralela");
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
                    UserName = $"user{i}",
                    Name = $"User {i}",
                    Email = $"user{i}@test.com",
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
            }

            // Assert
            var ids = users.Select(u => u.Id).ToList();
            var uniqueIds = ids.Distinct().Count();
            uniqueIds.Should().Be(100, "cada usuário deve ter um ID único");

            // Verificar se não há duplicatas
            var duplicates = ids.GroupBy(id => id).Where(g => g.Count() > 1).ToList();
            duplicates.Should().BeEmpty("não deve haver IDs duplicados");
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
                    UserName = $"user{i}",
                    Name = $"User {i}",
                    Email = $"user{i}@test.com",
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
            });

            // Assert
            var userList = users.ToList();
            var ids = userList.Select(u => u.Id).ToList();
            var uniqueIds = ids.Distinct().Count();
            uniqueIds.Should().Be(100, "cada usuário deve ter um ID único mesmo criado em paralelo");

            // Verificar se não há duplicatas
            var duplicates = ids.GroupBy(id => id).Where(g => g.Count() > 1).ToList();
            duplicates.Should().BeEmpty("não deve haver IDs duplicados em criação paralela");
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
                    UserName = $"user{i}",
                    Name = $"User {i}",
                    Email = $"user{i}@test.com",
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
                
                // Pequeno delay para garantir timestamps diferentes
                Thread.Sleep(1);
            }

            // Assert
            foreach (var user in users)
            {
                user.Id.Should().BeGreaterThan(0, "IDs devem ser positivos");
            }

            // IDs devem estar em ordem crescente (já que foram criados sequencialmente)
            var ids = users.Select(u => u.Id).ToList();
            for (int i = 1; i < ids.Count; i++)
            {
                ids[i].Should().BeGreaterThan(ids[i - 1], "IDs criados posteriormente devem ser maiores");
            }
        }

        [Fact]
        public void SnowflakeIdGenerator_ShouldGenerateUniqueIds_InMassiveCreation()
        {
            // Arrange
            var users = new List<User>();

            // Act - Simula criação massiva como no frontend (5000 entidades muito rápido)
            for (int i = 0; i < 5000; i++)
            {
                users.Add(new User
                {
                    UserName = $"user{i}",
                    Name = $"User {i}",
                    Email = $"user{i}@test.com",
                    Password = "test123",
                    ProfileImageUrl = "https://example.com/image.jpg",
                    Biography = "Test user"
                });
            }

            // Assert
            var ids = users.Select(u => u.Id).ToList();
            var uniqueIds = ids.Distinct().Count();
            
            uniqueIds.Should().Be(5000, "todos os 5000 IDs devem ser únicos");

            // Verificar duplicatas específicas
            var duplicates = ids.GroupBy(id => id)
                                .Where(g => g.Count() > 1)
                                .Select(g => new { Id = g.Key, Count = g.Count() })
                                .ToList();

            if (duplicates.Any())
            {
                var duplicateInfo = string.Join(", ", duplicates.Take(5).Select(d => $"ID {d.Id}: {d.Count}x"));
                Assert.True(false, $"Encontradas {duplicates.Count} duplicatas: {duplicateInfo}");
            }
        }

        [Fact]
        public void SnowflakeIdGenerator_ShouldHandleSequenceOverflow()
        {
            // Arrange
            var generator = new SnowflakeIdGenerator(1, 1);
            var ids = new HashSet<long>();

            // Act - Força overflow da sequência (4096 IDs no mesmo milissegundo)
            var startTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            
            for (int i = 0; i < 5000; i++)
            {
                var id = generator.NextId();
                
                // Assert - Verifica unicidade
                if (!ids.Add(id))
                {
                    Assert.True(false, $"ID duplicado encontrado: {id} na iteração {i}");
                }
            }

            // Assert
            ids.Count.Should().Be(5000, "todos os IDs devem ser únicos mesmo com overflow");
        }
    }
}
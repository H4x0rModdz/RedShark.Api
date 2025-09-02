using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Snowflakes;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanSharpArchitecture.Domain.Entities
{
    /// <summary>
    /// Base class for all entities, using Snowflake algorithm to generate unique IDs.
    /// </summary>
    public abstract class BaseEntity
    {
        // Ideally, the generator should be configured through dependency injection,
        // but for simplicity, we use a static generator with fixed workerId and datacenterId.
        private static readonly SnowflakeIdGenerator IdGenerator = new SnowflakeIdGenerator(workerId: 1, datacenterId: 1);

        /// <summary>
        /// Constructor that generates the unique ID immediately
        /// </summary>
        protected BaseEntity()
        {
            Id = IdGenerator.NextId();
        }

        /// <summary>
        /// Unique identifier generated based on the Snowflake algorithm.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
    }
}
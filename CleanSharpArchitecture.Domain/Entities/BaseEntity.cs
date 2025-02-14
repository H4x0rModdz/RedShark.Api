using CleanSharpArchitecture.Domain.Enums;
using CleanSharpArchitecture.Domain.Snowflakes;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanSharpArchitecture.Domain.Entities
{
    /// <summary>
    /// Base para todas as entidades, utilizando Snowflake para gerar IDs únicos.
    /// </summary>
    public abstract class BaseEntity
    {
        // Idealmente, o gerador deve ser configurado por meio de injeção de dependências,
        // mas para simplificar, usamos um gerador estático com workerId e datacenterId fixos.
        private static readonly SnowflakeIdGenerator IdGenerator = new SnowflakeIdGenerator(workerId: 1, datacenterId: 1);

        /// <summary>
        /// Identificador único gerado com base no algoritmo Snowflake.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; } = IdGenerator.NextId();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
    }
}
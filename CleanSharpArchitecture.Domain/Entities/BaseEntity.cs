using CleanSharpArchitecture.Domain.Enums;

namespace CleanSharpArchitecture.Domain.Entities
{
    public abstract class BaseEntity
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public EntityStatus Status { get; set; } = EntityStatus.Active;
    }
}
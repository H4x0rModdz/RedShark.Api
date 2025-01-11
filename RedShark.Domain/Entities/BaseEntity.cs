using RedShark.Shared.Abstractions.Domains;

namespace RedShark.Domain.Entities;

public abstract class BaseEntity<TId> : AggregateRoot<TId>
{
    public TId Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected BaseEntity()
    {
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateTimestamp()
    {
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdateTimestamp();
    }

    protected void AddDomainEvent(IDomainEvent eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}


using RedShark.Domain.Entities;
using RedShark.Shared.Abstractions.Domains;
using RedShark.Domain.ValueObjects;

namespace RedShark.Domain.Events;

public record SampleEntityItemAdded(SampleEntity sampleEntity, SampleEntityItem sampleEntityItem) : IDomainEvent;

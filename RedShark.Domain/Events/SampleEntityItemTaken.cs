using RedShark.Domain.Entities;
using RedShark.Shared.Abstractions.Domains;
using RedShark.Domain.ValueObjects;

namespace RedShark.Domain.Events;

public record SampleEntityItemTaken(SampleEntity sampleEntity, SampleEntityItem sampleEntityItem) : IDomainEvent;

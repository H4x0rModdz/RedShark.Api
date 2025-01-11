using RedShark.Shared.Abstractions.Commands;

namespace RedShark.Application.Commands;

public record RemoveSampleEntityItem(Guid sampleEntityId, string Name) : ICommand;

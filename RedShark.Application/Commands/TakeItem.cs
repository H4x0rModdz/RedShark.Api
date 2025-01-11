using RedShark.Shared.Abstractions.Commands;

namespace RedShark.Application.Commands;

public record TakeItem(Guid sampleEntityId, string Name) : ICommand;
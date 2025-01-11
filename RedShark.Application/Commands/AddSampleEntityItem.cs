using RedShark.Shared.Abstractions.Commands;

namespace RedShark.Application.Commands.Handlers;

public record AddSampleEntityItem(Guid sampleEntityId, string Name, uint Quantity) : ICommand;

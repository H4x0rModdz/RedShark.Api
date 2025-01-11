using RedShark.Shared.Abstractions.Commands;

namespace RedShark.Application.Commands;

public record RemoveSampleEntity(Guid Id) : ICommand;

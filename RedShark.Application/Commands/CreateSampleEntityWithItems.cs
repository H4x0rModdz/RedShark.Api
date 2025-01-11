using RedShark.Domain.Consts;
using RedShark.Shared.Abstractions.Commands;

namespace RedShark.Application.Commands;

public record CreateSampleEntityWithItems(Guid Id, string Name, Gender Gender,
   DestinationWriteModel Destionation) : ICommand;

public record DestinationWriteModel(string City, string Country);

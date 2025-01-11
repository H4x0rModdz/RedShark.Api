using RedShark.Application.DTOs;
using RedShark.Infrastructure.EF.Models;

namespace RedShark.Infrastructure.EF.Queries;

internal static class Extensions
{
    public static SampleEntityDto AsDto(this SampleEntityReadModel readModel)
        => new SampleEntityDto(

            Id: readModel.Id,
            Name: readModel.Name,
            Destination: new DestinationDto
            (
                City: readModel.Destination?.City,
                Country: readModel.Destination?.Country
            ),
            Items: readModel.Items?.Select(pi => new SampleEntityItemDto
            (
                Name: pi.Name,
                Quantity: pi.Quantity,
                IsTaken: pi.IsTaken
            )
            ));
}

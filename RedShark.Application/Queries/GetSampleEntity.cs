using RedShark.Application.DTOs;
using RedShark.Shared.Abstractions.Queries;

namespace RedShark.Application.Queries;

public class GetSampleEntity : IQuery<SampleEntityDto>
{
    public Guid Id { get; set; }
}

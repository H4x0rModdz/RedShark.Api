using RedShark.Application.DTOs;
using RedShark.Shared.Abstractions.Queries;

namespace RedShark.Application.Queries;

public class SearchSampleEntity : IQuery<IEnumerable<SampleEntityDto>>
{
    public string SearchPhrase { get; set; }
}

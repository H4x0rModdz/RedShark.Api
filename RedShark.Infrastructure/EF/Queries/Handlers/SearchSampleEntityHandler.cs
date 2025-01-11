using RedShark.Application.DTOs;
using RedShark.Application.Queries;
using RedShark.Infrastructure.EF.Contexts;
using RedShark.Infrastructure.EF.Models;
using RedShark.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace RedShark.Infrastructure.EF.Queries.Handlers;

internal sealed class SearchSampleEntityHandler : IQueryHandler<SearchSampleEntity, IEnumerable<SampleEntityDto>>
{
    private readonly DbSet<SampleEntityReadModel> _SampleEntities;

    public SearchSampleEntityHandler(ReadDbContext context)
        => _SampleEntities = context.SampleEntities;

    public async Task<IEnumerable<SampleEntityDto>> HandleAsync(SearchSampleEntity query)
    {
        var dbQuery = _SampleEntities
            .Include(pl => pl.Items)
        .AsQueryable();

        if (query.SearchPhrase is not null)
        {
            dbQuery = dbQuery.Where(pl =>
                Microsoft.EntityFrameworkCore.EF.Functions.Like(pl.Name, $"%{query.SearchPhrase}%"));
        }

        return await dbQuery
            .Select(pl => pl.AsDto())
            .AsNoTracking()
            .ToListAsync();
    }
}


using RedShark.Application.DTOs;
using RedShark.Application.Queries;
using RedShark.Domain.Entities;
using RedShark.Infrastructure.EF.Contexts;
using RedShark.Infrastructure.EF.Models;
using RedShark.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace RedShark.Infrastructure.EF.Queries.Handlers;

internal sealed class GetSampleEntityHandler : IQueryHandler<GetSampleEntity, SampleEntityDto>
{
    private readonly DbSet<SampleEntityReadModel> _SampleEntities;

    public GetSampleEntityHandler(ReadDbContext context)
        => _SampleEntities = context.SampleEntities;

    public Task<SampleEntityDto> HandleAsync(GetSampleEntity query)
        => _SampleEntities
            .Include(pl => pl.Items)
            .Where(pl => pl.Id == query.Id)
            .Select(pl => pl.AsDto())
            .AsNoTracking()
            .SingleOrDefaultAsync();
}

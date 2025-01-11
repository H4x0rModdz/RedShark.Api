using RedShark.Application.Services;
using RedShark.Infrastructure.EF.Contexts;
using RedShark.Infrastructure.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace RedShark.Infrastructure.EF.Services;

internal sealed class SampleEntityReadService : ISampleEntityReadService
{
    private readonly DbSet<SampleEntityReadModel> _sampleEntity;

    public SampleEntityReadService(ReadDbContext context)
        => _sampleEntity = context.SampleEntities;

    public Task<bool> ExistsByNameAsync(string name)
        => _sampleEntity.AnyAsync(pl => pl.Name == name);
}

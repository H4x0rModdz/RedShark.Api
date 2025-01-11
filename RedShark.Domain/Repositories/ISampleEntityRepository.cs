using RedShark.Domain.Entities;
using RedShark.Domain.ValueObjects;

namespace RedShark.Domain.Repositories;

    public interface ISampleEntityRepository
    {
        Task<SampleEntity> GetAsync(SampleEntityId id);
        Task AddAsync(SampleEntity sampleEntity);
        Task UpdateAsync(SampleEntity sampleEntity);
        Task DeleteAsync(SampleEntity sampleEntity);
    }

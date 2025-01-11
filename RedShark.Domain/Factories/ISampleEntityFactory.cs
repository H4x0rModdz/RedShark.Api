using RedShark.Domain.Consts;
using RedShark.Domain.Entities;
using RedShark.Domain.ValueObjects;

namespace RedShark.Domain.Factories;

    public interface ISampleEntityFactory
    {
        SampleEntity Create(SampleEntityId id, SampleEntityName name, SampleEntityDestination destination);

        SampleEntity CreateWithDefaultItems(SampleEntityId id, SampleEntityName name, Gender gender,
            SampleEntityDestination destination);
    }

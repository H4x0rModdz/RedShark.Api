using RedShark.Domain.ValueObjects;

namespace RedShark.Domain.Policies;

    public interface ISampleEntityItemsPolicy
    {
        bool IsApplicable(PolicyData data);
        IEnumerable<SampleEntityItem> GenerateItems(PolicyData data);
    }

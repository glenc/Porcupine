using Porcupine.Domain.Events;

namespace Porcupine.Domain.Entities;

public class MarketSegment : BaseChangeTrackingEntity
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";

    public MarketSegment(string name) : this(name, "") {}

    public MarketSegment(string name, string description)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        Description = description;

        AddDomainEvent(new MarketSegmentCreatedEvent(this));
    }

    public void Update(string name, string description)
    {
        Guard.Against.NullOrWhiteSpace(name);

        ApplyChange(() => Name, x => Name = x, name);
        ApplyChange(() => Description, x => Description = x, description);

        if (HasChanges())
        {
            AddDomainEvent(new MarketSegmentUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    // for ef rehydration
    private MarketSegment() {}
}
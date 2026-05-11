using Porcupine.Domain.Events;

namespace Porcupine.Domain.Entities;

public class MarketSegment : BaseAuditableEntity
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

        var originalState = new Dictionary<string, object>();

        if (name != Name)
        {
            originalState.Add("Name", Name);
            Name = name;
        }

        if (description != Description)
        {
            originalState.Add("Description", Description);
            Description = description;
        }

        if (originalState.Keys.Count > 0)
        {
            AddDomainEvent(new MarketSegmentUpdatedEvent(this, originalState));
        }
    }

    // for ef rehydration
    private MarketSegment() {}
}
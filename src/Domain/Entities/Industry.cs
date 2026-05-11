using Porcupine.Domain.Events;

namespace Porcupine.Domain.Entities;

public class Industry : BaseAuditableEntity
{
    public string Name { get; private set; } = "";
    public string Description { get; private set; } = "";

    public Industry(string name) : this(name, "") {}

    public Industry(string name, string description)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        Description = description;

        AddDomainEvent(new IndustryCreatedEvent(this));
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
            AddDomainEvent(new IndustryUpdatedEvent(this, originalState));
        }
    }

    // for ef rehydration
    private Industry() {}
}
using Porcupine.Domain.Events;

namespace Porcupine.Domain.Entities;

public class Industry : BaseChangeTrackingEntity
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

        ApplyChange(() => Name, x => Name = x, name);
        ApplyChange(() => Description, x => Description = x, description);

        if (HasChanges())
        {
            AddDomainEvent(new IndustryUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    // for ef rehydration
    private Industry() {}
}
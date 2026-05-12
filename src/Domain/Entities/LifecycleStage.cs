using Porcupine.Domain.Events;

namespace Porcupine.Domain.Entities;

public class LifecycleStage : BaseChangeTrackingEntity
{
    public string Name { get; private set; } = "";
    public double Order { get; private set; } = 1.0;

    public LifecycleStage(string name) : this(name, 1.0) {}

    public LifecycleStage(string name, double order)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        Order = order;

        AddDomainEvent(new LifecycleStageCreatedEvent(this));
    }

    public void Update(string name, double order)
    {
        Guard.Against.NullOrWhiteSpace(name);

        ApplyChange(() => Name, x => Name = x, name);
        ApplyChange(() => Order, x => Order = x, order);

        if (HasChanges())
        {
            AddDomainEvent(new LifecycleStageUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    // for ef rehydration
    private LifecycleStage() {}
}
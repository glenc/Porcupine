using Porcupine.Domain.Events;

namespace Porcupine.Domain.Entities;

public class Organization : BaseChangeTrackingEntity
{
    public string Name { get; private set; } = "";

    public MarketSegment? MarketSegment { get; private set; }
    public Industry? Industry { get; private set; }
    public LifecycleStage LifecycleStage { get; private set; }

    public Organization(string name, LifecycleStage lifecycleStage)
    {
        Guard.Against.NullOrWhiteSpace(name);

        Name = name;
        LifecycleStage = lifecycleStage;

        AddDomainEvent(new OrganizationCreatedEvent(this));
    }

    public void Update(string name)
    {
        ApplyChange(() => Name, x => Name = x, name);

        if (HasChanges())
        {
            AddDomainEvent(new OrganizationUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    public void SetMarketSegment(MarketSegment? marketSegment)
    {
        ApplyChange(() => MarketSegment, x => MarketSegment = x, marketSegment);

        if (HasChanges())
        {
            AddDomainEvent(new OrganizationUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    public void SetIndustry(Industry? industry)
    {
        ApplyChange(() => Industry, x => Industry = x, industry);

        if (HasChanges())
        {
            AddDomainEvent(new OrganizationUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    public void ChangeLifecycleStage(LifecycleStage lifecycleStage)
    {
        ApplyChange(() => LifecycleStage, x => LifecycleStage = x, lifecycleStage);

        if (HasChanges())
        {
            AddDomainEvent(new OrganizationLifecycleStageChangedEvent(this, GetAndClearChanges()));
        }
    }

    // for ef rehydration
    private Organization() 
    {
        LifecycleStage = null!;
    }
}
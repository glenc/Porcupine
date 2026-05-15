using Porcupine.Domain.Events;
using Porcupine.Domain.Triggers;

namespace Porcupine.Domain.Entities;

public class Rule : BaseChangeTrackingEntity
{
    public TriggerType TriggerType { get; private set; }
    public string TriggerName { get; private set; }
    public string Name { get; private set; }
    public string? Criteria { get; private set; }

    private Rule(string name, TriggerType triggerType, string triggerName, string? criteria)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(triggerName);
        Guard.Against.Null(triggerType);

        Name = name;
        TriggerType = triggerType;
        TriggerName = triggerName;
        Criteria = criteria;

        AddDomainEvent(new RuleCreatedEvent(this));
    }

    public void Update(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);
        
        ApplyChange(() => Name, x => Name = x, name);

        if (HasChanges())
        {
            AddDomainEvent(new RuleUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    public void ChangeCriteria(string? criteria)
    {
        ApplyChange(() => Criteria, x => Criteria = x, criteria);

        if (HasChanges())
        {
            AddDomainEvent(new RuleUpdatedEvent(this, GetAndClearChanges()));
        }
    }

    public static Rule DomainEventRuleFor<TEvent>(string name)
    {
        return DomainEventRuleFor<TEvent>(name, null);
    }
    public static Rule DomainEventRuleFor<TEvent>(string name, string? criteria)
    {
        return DomainEventRuleFor(typeof(TEvent), name, criteria);
    }

    public static Rule DomainEventRuleFor(Type eventType, string name)
    {
        return DomainEventRuleFor(eventType, name, null);
    }

    public static Rule DomainEventRuleFor(Type eventType, string name, string? criteria)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.Null(eventType);
        Guard.Against.Null(eventType.AssemblyQualifiedName);

        return new Rule(
            name,
            TriggerType.DomainEvent,
            eventType.AssemblyQualifiedName,
            criteria
        );
    }

    // for ef rehydration
    private Rule() 
    {
        Name = null!;
        TriggerName = null!;
    }
}
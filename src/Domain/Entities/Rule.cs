using Porcupine.Domain.Enums;

namespace Porcupine.Domain.Entities;

public class Rule : BaseAuditableEntity
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
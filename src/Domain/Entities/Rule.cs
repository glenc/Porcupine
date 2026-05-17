using Porcupine.Domain.Events;
using Porcupine.Domain.Triggers;
using Porcupine.Domain.ValueObjects;

namespace Porcupine.Domain.Entities;

public class Rule : BaseChangeTrackingEntity
{
    public TriggerType TriggerType { get; private set; }
    public string TriggerName { get; private set; }
    public string Name { get; private set; }
    public string? Criteria { get; private set; }
    public IReadOnlyCollection<RuleAction> Actions => _actions.AsReadOnly<RuleAction>();

    private readonly List<RuleAction> _actions = [];

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

    public void AddAction(RuleAction action)
    {
        if (!_actions.Contains(action))
        {
            _actions.Add(action);

            AddDomainEvent(new ActionAddedToRuleEvent(this, action));
        }
    }

    public void RemoveAction(RuleAction action)
    {
        if (_actions.Contains(action))
        {
            _actions.Remove(action);

            AddDomainEvent(new ActionRemovedFromRuleEvent(this, action));
        }
    }

    public void RemoveAllActions()
    {
        if (_actions.Count > 0)
        {
            var actionsRemoved = new List<RuleAction>(_actions);

            _actions.Clear();

            AddDomainEvent(new AllActionsRemovedFromRuleEvent(this, actionsRemoved));
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
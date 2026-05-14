namespace Porcupine.Domain.Entities;

public class Rule : BaseAuditableEntity
{
    public string Name { get; private set; } = "";
    public string EventName { get; private set; } = "";
    public string? Criteria { get; private set; }

    private Rule(string name, string eventName, string? criteria)
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NullOrWhiteSpace(eventName);

        Name = name;
        EventName = eventName;
        Criteria = criteria;
    }

    public static Rule RuleFor<TEventType>(string name)
    {
        return Rule.RuleFor<TEventType>(name, null);
    }

    public static Rule RuleFor<TEventType>(string name, string? criterial)
    {
        return RuleFor(typeof(TEventType), name, criterial);
    }

    public static Rule RuleFor(Type type, string name)
    {
        return RuleFor(type, name, null);
    }

    public static Rule RuleFor(Type type, string name, string? criteria)
    {
        return new Rule(name, type.GetFriendlyName(), criteria);
    }

    // for ef rehydration
    private Rule() {}
}
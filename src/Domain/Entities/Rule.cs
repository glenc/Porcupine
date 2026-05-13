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
        return new Rule(name, typeof(TEventType).GetFriendlyName(), criterial);
    }

    // for ef rehydration
    private Rule() {}
}
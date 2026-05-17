namespace Porcupine.Domain.ValueObjects;

public class RuleAction : ValueObject
{
    public Type CommandType { get; init; }
    public string CommandTemplate { get; set; }

    public RuleAction(Type commandType, string commandTemplate)
    {
        Guard.Against.NullOrWhiteSpace(commandTemplate);

        CommandType = commandType;
        CommandTemplate = commandTemplate;
    }

    public static RuleAction For<T>(string commandTemplate)
    {
        return new RuleAction(typeof(T), commandTemplate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CommandType;
        yield return CommandTemplate;
    }

    // for ef rehydration
    private RuleAction() 
    { 
        CommandType = null!;
        CommandTemplate = null!;
    }
}
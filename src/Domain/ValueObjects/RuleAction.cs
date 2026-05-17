using System.Text.Json.Serialization;

namespace Porcupine.Domain.ValueObjects;

public class RuleAction : ValueObject
{
    public string CommandTypeName { get; init; }
    public string CommandTemplate { get; set; }

    public RuleAction(Type commandType, string commandTemplate) : this(commandType.AssemblyQualifiedName!, commandTemplate)
    {
    }

    [JsonConstructor]
    public RuleAction(string commandTypeName, string commandTemplate)
    {
        Guard.Against.NullOrWhiteSpace(commandTemplate);
        Guard.Against.NullOrWhiteSpace(commandTypeName);

        var type = Type.GetType(commandTypeName) 
            ?? throw new ArgumentException("CommandTypeName could not be resolved to a valid Type");

        CommandTypeName = commandTypeName;
        CommandTemplate = commandTemplate;
    }

    public static RuleAction For<T>(string commandTemplate)
    {
        return new RuleAction(typeof(T), commandTemplate);
    }

    public Type GetCommandType()
    {
        return Type.GetType(CommandTypeName)
            ?? throw new Exception("CommandTypeName could not be resolved to a valid Type");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CommandTypeName;
        yield return CommandTemplate;
    }

    // for ef rehydration
    private RuleAction() 
    { 
        CommandTypeName = null!;
        CommandTemplate = null!;
    }
}
namespace Porcupine.Domain.Triggers;

[AttributeUsage(AttributeTargets.Class)]
public class TriggerAttribute(string displayName, string description = "") : Attribute
{
    public string DisplayName { get; init; } = displayName;
    public string Description { get; init;} = description;
}
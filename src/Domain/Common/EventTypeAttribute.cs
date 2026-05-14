namespace Porcupine.Domain.Common;

[AttributeUsage(AttributeTargets.Class)]
public class EventTypeAttribute(string displayName, string description = "") : Attribute
{
    public string DisplayName { get; init; } = displayName;
    public string Description { get; init;} = description;
}
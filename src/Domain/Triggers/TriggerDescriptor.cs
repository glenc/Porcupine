namespace Porcupine.Domain.Triggers;

public record TriggerDescriptor(string Name, Type Type, string? DisplayName, string? Description);
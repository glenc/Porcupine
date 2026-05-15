namespace Porcupine.Domain.Events;

public record IndustryCreatedEvent(Industry Entity) : PorcupineEvent<Industry>(Entity) {}
public record IndustryUpdatedEvent(Industry Entity, IDictionary<string, object?> OriginalState) : PorcupineEntityChangedEvent<Industry>(Entity, OriginalState) {}
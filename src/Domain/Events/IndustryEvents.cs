namespace Porcupine.Domain.Events;

[EventType("Industry Created", "Occurrs every time a new industry is craeted.")]
public record IndustryCreatedEvent(Industry Entity) 
    : PorcupineEvent<Industry>(Entity), IEventTypeNotification {}

[EventType("Industry Update", "Occurrs every time an existing industry is updated.")]
public record IndustryUpdatedEvent(Industry Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<Industry>(Entity, OriginalState), IEventTypeNotification {}
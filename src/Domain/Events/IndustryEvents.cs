using Porcupine.Domain.Triggers;

namespace Porcupine.Domain.Events;

[Trigger("Industry Created", "Occurrs when a new industry is created.")]
public record IndustryCreatedEvent(Industry Entity) 
    : PorcupineEvent<Industry>(Entity), IDomainEventTrigger<Industry> {}

[Trigger("Industry Updated", "Occures when an existing industry is updated.")]
public record IndustryUpdatedEvent(Industry Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<Industry>(Entity, OriginalState), IDomainEventTrigger<Industry> {}
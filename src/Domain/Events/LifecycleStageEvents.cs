using Porcupine.Domain.Triggers;

namespace Porcupine.Domain.Events;

[Trigger("Lifecycle Stage Created", "Occurrs when a new Opportunity Lifecycle Stage is created.")]
public record LifecycleStageCreatedEvent(LifecycleStage Entity) 
    : PorcupineEvent<LifecycleStage>(Entity), IDomainEventTrigger<LifecycleStage> {}

[Trigger("Lifecycle Stage Updated", "Occurrs when an Opportunity Lifecycle Stage is updated.")]
public record LifecycleStageUpdatedEvent(LifecycleStage Entity, IDictionary<string, object?> OriginalState) 
    : PorcupineEntityChangedEvent<LifecycleStage>(Entity, OriginalState), IDomainEventTrigger<LifecycleStage> {}
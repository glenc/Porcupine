namespace Porcupine.Domain.Events;

public record LifecycleStageCreatedEvent(LifecycleStage Entity) : PorcupineEvent<LifecycleStage>(Entity) {}
public record LifecycleStageUpdatedEvent(LifecycleStage Entity, IDictionary<string, object?> OriginalState) : PorcupineEntityChangedEvent<LifecycleStage>(Entity, OriginalState) {}
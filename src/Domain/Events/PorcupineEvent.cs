namespace Porcupine.Domain.Events;

public record PorcupineEvent<T>(T Entity) : BaseEvent {}
public record PorcupineEntityChangedEvent<T>(T Entity, IDictionary<string, object?> OriginalState) : PorcupineEvent<T>(Entity) {}
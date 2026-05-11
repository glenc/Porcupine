namespace Porcupine.Domain.Events;

public record PorcupineEvent<T>(T Entity) : BaseEvent {}
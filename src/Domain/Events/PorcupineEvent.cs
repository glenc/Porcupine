namespace Porcupine.Domain.Events;

public class PorcupineEvent<T> : BaseEvent 
{ 
    public T Entity { get; }

    public PorcupineEvent(T entity)
    {
        Entity = entity;
    }
}
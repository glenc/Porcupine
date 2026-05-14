namespace Porcupine.Application.Common.Interfaces;

public interface IEventTypeService
{
    void AddEventTypesFromAssembly<T>();
    void AddEventTypesFromAppDomain();
    bool IsTypeRegistered(Type eventType);
    
    IEnumerable<IEventType> EventTypes { get; }
}
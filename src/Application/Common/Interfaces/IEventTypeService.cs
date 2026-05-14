namespace Porcupine.Application.Common.Interfaces;

public interface IEventTypeService
{
    void AddEventTypesFromAssembly<T>();
    void AddEventTypesFromAppDomain();
    
    IEnumerable<IEventType> EventTypes { get; }
}
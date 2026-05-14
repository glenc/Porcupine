using System.Reflection;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Common;

namespace Porcupine.Application.Common.Services;

public class EventTypeService : IEventTypeService
{
    private readonly List<IEventType> _types = [];

    public IEnumerable<IEventType> EventTypes => _types;

    public void AddEventTypesFromAssembly<T>()
    {
        var baseEventType = typeof(BaseEvent);

        var types = typeof(T).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && baseEventType.IsAssignableFrom(t))
            .Select(PopulateEventType)
            .ToList();
        
        _types.AddRange(types);
    }

    public void AddEventTypesFromAppDomain()
    {
        var baseEventType = typeof(BaseEvent);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try
                {
                    return a.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.OfType<Type>();
                }
            })
            .Where(t => 
                t != null &&
                t.IsClass &&
                !t.IsAbstract &&
                baseEventType.IsAssignableFrom(t))
            .Select(PopulateEventType)
            .ToList();
        
        _types.AddRange(types);
    }

    private IEventType PopulateEventType(Type type)
    {
            var attr = type.GetCustomAttribute<EventTypeAttribute>();

            return new EventType
            (
                type,
                attr?.DisplayName ?? "", 
                attr?.Description ?? ""
            );
    }
}
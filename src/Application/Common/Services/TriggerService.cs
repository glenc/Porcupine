using System.Reflection;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.Services;

public class TriggerService : ITriggerService
{
    private readonly List<Type> _triggers = [];

    public IReadOnlyCollection<Type> Triggers => _triggers.AsReadOnly<Type>();

    public void AddTrigger<T>() where T : ITrigger
    {
        if (!_triggers.Contains(typeof(T)))
            _triggers.Add(typeof(T));
    }

    public void AddTriggersFromAppDomain()
    {
        var baseEventType = typeof(ITrigger);

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
            .ToList();
        
        _triggers.AddRange(types);
    }

    public void AddTriggersFromAssembly<T>()
    {
        var baseEventType = typeof(ITrigger);

        var types = typeof(T).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && baseEventType.IsAssignableFrom(t))
            .ToList();

        _triggers.AddRange(types);
    }

    public void ClearTriggers()
    {
        _triggers.Clear();
    }
}
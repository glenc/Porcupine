using System.Reflection;
using Porcupine.Application.Common.Interfaces;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.Services;

public class TriggerService : ITriggerService
{
    private readonly List<TriggerDescriptor> _triggers = [];

    public IReadOnlyCollection<TriggerDescriptor> Triggers => _triggers.AsReadOnly<TriggerDescriptor>();

    public void AddTrigger<T>() where T : ITrigger
    {
        if (!_triggers.Where(x => x.Type == typeof(T)).Any())
            _triggers.Add(GetDescriptor(typeof(T)));
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
            .Select(GetDescriptor)
            .ToList();
        
        _triggers.AddRange(types);
    }

    public void AddTriggersFromAssembly<T>()
    {
        var baseEventType = typeof(ITrigger);

        var types = typeof(T).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && baseEventType.IsAssignableFrom(t))
            .Select(GetDescriptor)
            .ToList();

        _triggers.AddRange(types);
    }

    public void ClearTriggers()
    {
        _triggers.Clear();
    }

    private TriggerDescriptor GetDescriptor(Type type)
    {
        var attr = type.GetCustomAttribute<TriggerAttribute>();

        return new TriggerDescriptor
        (
            type.AssemblyQualifiedName ?? "Unknown",
            type,
            attr?.DisplayName ?? "", 
            attr?.Description ?? ""
        );
    }
}
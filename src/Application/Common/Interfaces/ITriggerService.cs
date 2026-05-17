using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.Interfaces;

public interface ITriggerService
{
    IReadOnlyCollection<TriggerDescriptor> Triggers { get; }
    void AddTriggersFromAssembly<T>();
    void AddTriggersFromAppDomain();
    void AddTrigger<T>() where T : ITrigger;
    void ClearTriggers();
}
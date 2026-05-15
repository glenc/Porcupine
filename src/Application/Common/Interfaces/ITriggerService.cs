using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Common.Interfaces;

public interface ITriggerService
{
    IReadOnlyCollection<Type> Triggers { get; }
    void AddTriggersFromAssembly<T>();
    void AddTriggersFromAppDomain();
    void AddTrigger<T>() where T : ITrigger;
    void ClearTriggers();
}
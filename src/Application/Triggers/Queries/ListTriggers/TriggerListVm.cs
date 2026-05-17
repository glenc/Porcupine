using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Triggers.Queries.ListTriggers;

public record TriggerListVm(IReadOnlyCollection<TriggerDto> Items)
{
}
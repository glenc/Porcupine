namespace Porcupine.Application.EventTypes.Queries.ListEventTypes;

public record EventTypeListVm(IReadOnlyCollection<EventTypeDto> Items)
{
}
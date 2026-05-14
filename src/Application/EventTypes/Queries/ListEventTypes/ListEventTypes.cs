using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Security;

namespace Porcupine.Application.EventTypes.Queries.ListEventTypes;

[Authorize]
public record ListEventTypesQuery : IRequest<EventTypeListVm>
{
    public int Id { get; init; }
}

public class ListEventTypesQueryValidator : AbstractValidator<ListEventTypesQuery>
{
    public ListEventTypesQueryValidator()
    {
    }
}

public class ListEventTypesQueryHandler(IEventTypeService eventTypeService) : IRequestHandler<ListEventTypesQuery, EventTypeListVm>
{
    private readonly IEventTypeService _eventTypeService = eventTypeService;

    public async Task<EventTypeListVm> Handle(ListEventTypesQuery request, CancellationToken cancellationToken)
    {
        var eventTypes = _eventTypeService.EventTypes
            .Select(x => new EventTypeDto(x.TypeName, x.DisplayName, x.Description))
            .ToList();
        
        return new EventTypeListVm(eventTypes);
    }
}

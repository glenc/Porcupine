using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Mappings;
using Porcupine.Application.Common.Security;
using Porcupine.Domain.Triggers;

namespace Porcupine.Application.Triggers.Queries.ListTriggers;

[Authorize]
public record ListTriggersQuery : IRequest<TriggerListVm>
{
}

public class ListTriggersQueryValidator : AbstractValidator<ListTriggersQuery>
{
    public ListTriggersQueryValidator()
    {
    }
}

public class ListTriggersQueryHandler(ITriggerService triggerService, IMapper mapper) : IRequestHandler<ListTriggersQuery, TriggerListVm>
{
    private readonly ITriggerService _triggerService = triggerService;
    private readonly IMapper _mapper = mapper;

    public async Task<TriggerListVm> Handle(ListTriggersQuery request, CancellationToken cancellationToken)
    {
        var triggers =  _triggerService.Triggers
            .Select(x => _mapper.Map<TriggerDto>(x))
            .ToList();
        
        return new TriggerListVm(triggers);
    }
}

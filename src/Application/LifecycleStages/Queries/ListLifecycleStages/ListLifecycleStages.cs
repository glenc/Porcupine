using Porcupine.Application.Common.Interfaces;
using Porcupine.Application.Common.Mappings;
using Porcupine.Application.Common.Security;

namespace Porcupine.Application.LifecycleStages.Queries.ListLifecycleStages;

[Authorize]
public record ListLifecycleStagesQuery : IRequest<LifecycleStageListVm>
{
}

public class ListLifecycleStagesQueryValidator : AbstractValidator<ListLifecycleStagesQuery>
{
    public ListLifecycleStagesQueryValidator()
    {
    }
}

public class ListLifecycleStagesQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<ListLifecycleStagesQuery, LifecycleStageListVm>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<LifecycleStageListVm> Handle(ListLifecycleStagesQuery request, CancellationToken cancellationToken)
    {
        var stages = await _context.LifecycleStages
            .AsNoTracking()
            .OrderBy(x => x.Order)
            .ProjectToListAsync<LifecycleStageSummaryDto>(_mapper.ConfigurationProvider);

        return new LifecycleStageListVm(stages);
    }
}
